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

        }

        //Open add player popup
        void addPlayers()
        {
            addPlayersPopup.IsVisible = true;        
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

        }

        
    }
}