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
	public partial class Tournaments_AddEdit : ContentPage
	{

        private TournamentMain openTournament;

		public Tournaments_AddEdit ()
		{
			InitializeComponent ();
            openTournament = new TournamentMain();
		}

        private void saveButton_Clicked(object sender, EventArgs e)
        {
            TournamentMain tournament = new TournamentMain()
            {
                Name = nameEntry.Text,
                StartDate = dateEntry.Date
            };

            //Check Name
            if (tournament.Name == null || tournament.Name.ToString().Trim() == "")
            {
                DisplayAlert("Warning!", "Please enter a tournament name!", "OK");
                return;
            }

            //Check Date
            try
            {
                DateTime.Parse(tournament.StartDate.ToString());
            }
            catch
            {
                DisplayAlert("Warning!", "Please enter a valid tournament date!", "OK");
                return;
            }

            //Update database
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<TournamentMain>();

                int numberOfRows = 0;

                //Create
                numberOfRows = conn.Insert(tournament);
                if (numberOfRows > 0)
                {
                    DisplayAlert("Success", "Tournament successfully created", "OK");
                }
                else
                {
                    DisplayAlert("Failure", "An error occurred when creating this tournament", "OK");
                }

                Navigation.PopAsync();
            }
            

        }
    }
}