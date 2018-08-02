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

        private List<string> lstNames = new List<string>();

        public Tournaments_AllInfo (string strTitle, int tournamentId)
        {
            InitializeComponent();
            Title = strTitle;
            intTournID = tournamentId;


            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Player>();

                var lstPlayers = conn.Query<Player>("SELECT * FROM Player WHERE Active = 1 AND DateDeleted IS NULL");
                playersListView.ItemsSource = lstPlayers;
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<TournamentPlayers>();

                List<TournamentPlayers> lstTournamentPlayers = new List<TournamentPlayers>();

                lstTournamentPlayers = conn.Query<TournamentPlayers>("SELECT * FROM TournamentPlayers WHERE Active = 'True' AND TournmentId = ?", intTournID);
                activePlayersListView.ItemsSource = lstTournamentPlayers;

                lstTournamentPlayers = conn.Query<TournamentPlayers>("SELECT * FROM TournamentPlayers WHERE Active = 'False' AND TournmentId = ?", intTournID);
                droppedPlayersListView.ItemsSource = lstTournamentPlayers;
                
            }
        }

        void addPlayers()
        {
            //addPlayersPopup.IsVisible = true;
            //activityIndicator.IsRunning = true;

            //Navigation.PushModalAsync(new Players_Main());

            addPlayersPopup.IsVisible = true;
            
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("testing", "test this", "OK");

            string strNames = "";


            foreach (string name in lstNames)
            {
                strNames += name + ' ';
            }

            DisplayAlert("Selected", strNames, "Sweet");

            addPlayersPopup.IsVisible = false;
        }



        private void SwitchCell_OnChanged(SwitchCell sender, ToggledEventArgs e)
        {
            if (sender.On)
            {
                lstNames.Add(sender.Text);
            }
            else
            {
                lstNames.Remove(sender.Text);
            }
        }
    }
}