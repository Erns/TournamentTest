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

        public Tournaments_AllInfo (string strTitle, int tournamentId)
        {
            InitializeComponent();
            Title = strTitle;
            intTournID = tournamentId;

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Player>();

                List<Player> lstPlayers = conn.Query<Player>("SELECT * FROM Player WHERE Active = 1 AND DateDeleted IS NULL");
                playersListView.ItemsSource = lstPlayers;
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                Utilities.CreateTournamentMain(conn);
                conn.CreateTable<Player>();

                objTournMain = new TournamentMain();
                objTournMain = conn.GetWithChildren<TournamentMain>(intTournID);

                UpdatePlayerList(conn);

            }
        }

        void addPlayers()
        {
            addPlayersPopup.IsVisible = true;        
        }

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
            string strPlayerList = objTournMain.PlayersList();
            List<Player> lstPlayers = conn.Query<Player>("SELECT * FROM Player WHERE Active = 1 AND DateDeleted IS NULL AND Id IN (" + strPlayerList + ")");
            activePlayersListView.ItemsSource = lstPlayers;
        }

        private void startRoundBtn_ToolbarItem_Activated(object sender, EventArgs e)
        {

        }

        private void editTournmentBtn_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Tournaments_AddEdit(intTournID));
        }
    }
}