using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentTest.Classes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TournamentTest.Pages.Tournaments
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Tournaments_AllInfo : TabbedPage
    {
        private int intTournID;

        private TournamentMain objTournMain = new TournamentMain();
        private List<int> lstPlayerIds = new List<int>();

        //New
        public Tournaments_AllInfo (string strTitle, int tournamentId)
        {
            InitializeComponent();
            Title = strTitle;
            intTournID = tournamentId;
        }

        //Opening
        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                Utilities.CreateTournamentMain(conn);
                conn.CreateTable<Player>();

                objTournMain = new TournamentMain();
                objTournMain = conn.GetWithChildren<TournamentMain>(intTournID);
                Title = objTournMain.Name;

                UpdatePlayerList(conn);

                //Get full list of players
                List<Player> lstPlayers = conn.Query<Player>("SELECT * FROM Player WHERE (Active = 1 AND DateDeleted IS NULL) OR Id IN (" + objTournMain.PlayersList() + ")");

                //Get list of currently active players in tournament
                string[] arrActivePlayers = objTournMain.PlayersList().Split(',');

                //Get this list of player's active status
                foreach (Player player in lstPlayers)
                {
                    player.Active = false;
                    if (arrActivePlayers.Contains<string>(player.Id.ToString())) player.Active = true;
                }

                playersListView.ItemsSource = lstPlayers;

            }


            //Remove all the "Round" tabs and then repopulate them
            for (int index = this.Children.Count - 1; index > 0; index--)
            {
                if (index > 0)
                {
                    this.Children.RemoveAt(index);
                }
            }

            foreach (TournamentMainRound round in objTournMain.Rounds)
            {
                Children.Add(new Pages.Tournaments.Tournaments_RoundInfo("Round " + Children.Count));
            }


        }

        //Open add player popup
        void addPlayers()
        {
            addPlayersPopup.IsVisible = true;        
        }


        //Hide add player popup when hitting the back button
        protected override bool OnBackButtonPressed()
        {
            if (addPlayersPopup.IsVisible)
            {
                addPlayersPopup.IsVisible = false;
                OnAppearing();  //Refresh page so that the popup has all the correct players selected and set
                return true;    //Prevent back button from continuing
            }
            else
            {
                return base.OnBackButtonPressed();
            }
        }

        //Save player popup
        private void addPlayers_Button_Clicked(object sender, EventArgs e)
        {

            objTournMain.Players.Clear();

            foreach (int ID in lstPlayerIds)
            {
                TournamentMainPlayer tournamentPlayer = new TournamentMainPlayer();

                tournamentPlayer.TournmentId = intTournID;
                tournamentPlayer.PlayerId = ID;
                tournamentPlayer.Active = true;
                objTournMain.Players.Add(tournamentPlayer);
            }

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                Utilities.CreateTournamentMain(conn);

                conn.DeleteAll(objTournMain.Players);

                try
                {
                    conn.InsertOrReplaceWithChildren(objTournMain);
                    UpdatePlayerList(conn);
                }
                catch
                {
                    DisplayAlert("Warning!", "Error adding players to tournament!", "OK");
                }

            }

            addPlayersPopup.IsVisible = false;
        }


        //Track which players are to be added/removed
        private void SwitchCell_OnChanged(SwitchCell sender, ToggledEventArgs e)
        {
            int intPlayerID = Convert.ToInt32(sender.ClassId);
            if (sender.On)
            {
                if (!lstPlayerIds.Contains(intPlayerID)) lstPlayerIds.Add(intPlayerID);
            }
            else
            {
                if (lstPlayerIds.Contains(intPlayerID)) lstPlayerIds.Remove(intPlayerID);
            }
        }

        private void UpdatePlayerList(SQLite.SQLiteConnection conn)
        {
            List<Player> lstActivePlayers = conn.Query<Player>("SELECT * FROM Player WHERE Id IN (" + objTournMain.PlayersList() + ")");
            activePlayersListView.ItemsSource = lstActivePlayers;
        }



        //Secondary toolbar items
        private void editTournmentBtn_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Tournaments_AddEdit(intTournID));
        }

        private void startRoundBtn_ToolbarItem_Activated(object sender, EventArgs e)
        {
            if (objTournMain.Players.Count == 0)
            {
                DisplayAlert("Warning!", "You must add players first to this tournament!", "D'oh!");
                return;
            }

            //How to proceed when no rounds have started (Will likely reuse much/most/all for subsequent rounds, but just to start)
            if (objTournMain.Rounds.Count == 0)
            {
                //Grab list of currently active players in the tournament
                List<TournamentMainPlayer> lstActiveTournamentPlayers = new List<TournamentMainPlayer>();
                foreach(TournamentMainPlayer player in objTournMain.Players)
                {
                    if (player.Active) lstActiveTournamentPlayers.Add(player);
                }

                //Create a new round
                TournamentMainRound round = new TournamentMainRound();
                round.Number = objTournMain.Rounds.Count + 1;

                //First round, completely random
                lstActiveTournamentPlayers.Shuffle();


                TournamentMainRoundTable roundTable = new TournamentMainRoundTable();
                foreach (TournamentMainPlayer player in lstActiveTournamentPlayers)
                {     
                    if (roundTable.Player1Id != 0 && roundTable.Player2Id != 0)
                    {
                        round.Tables.Add(roundTable);
                        roundTable = new TournamentMainRoundTable();
                    }
                    
                    if (roundTable.Player1Id == 0)
                    {
                        roundTable.Number = round.Tables.Count + 1;
                        roundTable.Player1Id = player.Id;
                    }
                    else if (roundTable.Player2Id == 0)
                    {
                        roundTable.Player2Id = player.Id;
                    }
                }

                round.Players.AddRange(lstActiveTournamentPlayers);
                round.Tables.Add(roundTable);

                objTournMain.Rounds.Add(round);

                using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
                {
                    Utilities.CreateTournamentMain(conn);

                    try
                    {

                        conn.InsertOrReplaceWithChildren(round);
                        conn.InsertOrReplaceWithChildren(objTournMain);

                    }
                    catch
                    {
                        DisplayAlert("Warning!", "Error adding round to tournament!", "OK");
                    }

                    OnAppearing();
                }
            }
        }

        
    }
}