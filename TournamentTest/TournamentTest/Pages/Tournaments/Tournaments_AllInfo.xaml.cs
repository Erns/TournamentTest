using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentTest.Classes;
using TournamentTest.ViewModel;
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

        private ObservableCollection<PlayerToTournamentMainPlayer_ViewModel> lstViewPlayers = new ObservableCollection<PlayerToTournamentMainPlayer_ViewModel>();

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

                objTournMain = new TournamentMain();
                objTournMain = conn.GetWithChildren<TournamentMain>(intTournID, true);
                Title = objTournMain.Name;

                UpdatePlayerList(conn);

                //Get full list of players
                List<Player> lstPlayers = conn.Query<Player>("SELECT * FROM Player WHERE (Active = 1 AND DateDeleted IS NULL) OR Id IN (" + objTournMain.ActivePlayersList() + ")");

                //Get list of currently active players in tournament
                string[] arrActivePlayers = objTournMain.ActivePlayersList().Split(',');


                //Set using the ViewModel version.  This allows being able to manipulate back and forth across the class properties, while displaying as intended on the GUI
                //while also ensuring none of the goings ons of the properties touching each other don't occur without this specific view model (such as the SQL table updates)
                int intRow = 0;
                lstViewPlayers = new ObservableCollection<PlayerToTournamentMainPlayer_ViewModel>();
                TournamentMainPlayer tmpTournamentMainPlayer = null;
                foreach (Player player in lstPlayers)
                {
                    player.Active = false;
                    if (arrActivePlayers.Contains<string>(player.Id.ToString())) player.Active = true;
                    intRow++;

                    //Set the tournament player equivalent
                    tmpTournamentMainPlayer = null;
                    foreach (TournamentMainPlayer tournamentPlayer in objTournMain.Players)
                    {
                        if (tournamentPlayer.PlayerId == player.Id)
                        {
                            tmpTournamentMainPlayer = tournamentPlayer;
                            break;
                        }
                    }

                    lstViewPlayers.Add(new PlayerToTournamentMainPlayer_ViewModel(player, intTournID, intRow, tmpTournamentMainPlayer));
                }
                playersListView.ItemsSource = lstViewPlayers;
            }


            //Remove all the "Round" tabs and then repopulate them
            for (int index = this.Children.Count - 1; index > 0; index--)
            {
                if (index > 0)
                {
                    this.Children.RemoveAt(index);
                }
            }

            //Add each round as tabs
            foreach (TournamentMainRound round in objTournMain.Rounds)
            {
                Children.Add(new Tournaments_RoundInfo("Rd " + round.Number, round.Id));
            }

            //Remove option to delete round if there are no rounds
            if (objTournMain.Rounds.Count == 0)
            {
                ToolbarItems.Remove(deleteRoundBtn);
            }

        }

        //Open add player popup
        void addPlayers()
        {
            playersListView_SearchBar.Text = "";
            addPlayersPopup.IsVisible = true;        
        }

        //Close add player popup
        void closePlayers()
        {
            addPlayersPopup.IsVisible = false;
            OnAppearing();  //Refresh page so that the popup has all the correct players selected and set
        }


        //Hide add player popup when hitting the back button
        protected override bool OnBackButtonPressed()
        {
            if (addPlayersPopup.IsVisible)
            {
                closePlayers();
                return true;    //Prevent back button from continuing
            }
            else
            {
                return base.OnBackButtonPressed();
            }
        }


        private void UpdatePlayerList(SQLite.SQLiteConnection conn)
        {
            List<Player> lstActivePlayers = conn.Query<Player>("SELECT * FROM Player WHERE Id IN (" + objTournMain.ActivePlayersList() + ")");
            activePlayersListView.ItemsSource = lstActivePlayers;
        }



        //Secondary toolbar items
        private void editTournmentBtn_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Tournaments_AddEdit(intTournID));
        }


        //Start next round
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
                List<TournamentMainRoundPlayer> lstActiveTournamentPlayers = new List<TournamentMainRoundPlayer>();
                foreach(TournamentMainPlayer player in objTournMain.Players)
                {
                    if (player.Active)
                    {
                        TournamentMainRoundPlayer roundPlayer = new TournamentMainRoundPlayer();
                        roundPlayer.PlayerId = player.PlayerId;
                        lstActiveTournamentPlayers.Add(roundPlayer);
                    }
                }

                //Create a new round
                TournamentMainRound round = new TournamentMainRound();
                round.TournmentId = intTournID;
                round.Number = objTournMain.Rounds.Count + 1;
                round.Players.AddRange(lstActiveTournamentPlayers);

                if (objTournMain.Rounds.Count == 0)
                {
                    //First round, completely random player pairings
                    lstActiveTournamentPlayers.Shuffle();
                }
                else
                {

                }

                //Create each table, pair 'em up
                TournamentMainRoundTable roundTable = new TournamentMainRoundTable();
                foreach (TournamentMainRoundPlayer player in lstActiveTournamentPlayers)
                {     
                    if (roundTable.Player1Id != 0 && roundTable.Player2Id != 0)
                    {
                        setRoundTableNames(ref roundTable);
                        round.Tables.Add(roundTable);
                        roundTable = new TournamentMainRoundTable();
                    }
                    
                    if (roundTable.Player1Id == 0)
                    {
                        roundTable.Number = round.Tables.Count + 1;
                        roundTable.Player1Id = player.PlayerId;
                    }
                    else if (roundTable.Player2Id == 0)
                    {
                        roundTable.Player2Id = player.PlayerId;
                    }
                }

                setRoundTableNames(ref roundTable);
                round.Tables.Add(roundTable);


                //Add/Save the round
                using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
                {
                    try
                    {
                        conn.InsertWithChildren(round, true);
                    }
                    catch(Exception ex)
                    {
                        DisplayAlert("Warning!", "Error adding round to tournament! " + ex.Message, "OK");
                    }

                    OnAppearing();
                }
            }
        }

        private void setRoundTableNames(ref TournamentMainRoundTable roundTable)
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {

                Player player;

                string strPlayer1Name = "N/A";
                string strPlayer2Name = "N/A";

                if (roundTable.Player1Id > 0)
                {
                    player = conn.Get<Player>(roundTable.Player1Id);
                    strPlayer1Name = player.Name;
                }

                if (roundTable.Player2Id > 0)
                {
                    player = conn.Get<Player>(roundTable.Player2Id);
                    strPlayer2Name = player.Name;
                }

                roundTable.Player1Name = strPlayer1Name;
                roundTable.Player2Name = strPlayer2Name;
                roundTable.TableName = string.Format("{0} vs {1}", strPlayer1Name, strPlayer2Name);
            }
        }

        //Delete the last round
        async private void deleteRoundBtn_Activated(object sender, EventArgs e)
        {

            var confirmed = await DisplayAlert("Confirm", "Do you want to delete Round " + objTournMain.Rounds.Count + "?  This cannot be undone!", "Yes", "No");
            if (confirmed)
            {
                using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
                {

                    try
                    {
                        //Grab the latest round and delete it
                        TournamentMainRound round = objTournMain.Rounds[objTournMain.Rounds.Count - 1];
                        conn.Delete(round, true);
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Warning!", "Error deleting round from tournament! " + ex.Message, "OK");
                    }

                    OnAppearing();
                }
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            playersListView.BeginRefresh();

            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                playersListView.ItemsSource = lstViewPlayers;
            }
            else
            {
                playersListView.ItemsSource = lstViewPlayers.Where(i => i.TournamentMainPlayer.PlayerName.Contains(e.NewTextValue));
            }

            playersListView.EndRefresh();

        }
    }
}