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
	public partial class Tournaments_List : ContentPage
	{
        private bool blnActive;
		public Tournaments_List (string strTitle, bool blnActive)
		{
			InitializeComponent ();
            Title = strTitle;
            this.blnActive = blnActive;
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<TournamentMain>();

                List<TournamentMain> lstTournaments = new List<TournamentMain>();

                if (blnActive)
                {
                    lstTournaments = conn.Query<TournamentMain>("SELECT * FROM TournamentMain WHERE StartDate >= ?", DateTime.Today);
                }
                else
                {
                    lstTournaments = conn.Query<TournamentMain>("SELECT * FROM TournamentMain WHERE StartDate < ?", DateTime.Today);
                }

                tournamentListView.ItemsSource = lstTournaments;
            }

        }

    }
}