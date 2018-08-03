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
	public partial class Tournaments_AddEdit : ContentPage
	{

        private TournamentMain openTournament;

		public Tournaments_AddEdit ()
		{
			InitializeComponent ();
            openTournament = new TournamentMain();
		}


        public Tournaments_AddEdit(int intTournID)
        {
            InitializeComponent();
            openTournament = new TournamentMain();

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                Utilities.CreateTournamentMain(conn);
                conn.CreateTable<Player>();

                openTournament = conn.GetWithChildren<TournamentMain>(intTournID);

                nameEntry.Text = openTournament.Name;
                dateEntry.Date = openTournament.StartDate;
            }
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

                Utilities.CreateTournamentMain(conn);

                //Create
                try
                {
                    
                    if (openTournament.Id == 0)
                    {
                        conn.InsertWithChildren(tournament);
                        DisplayAlert("Success", "Tournament successfully created", "OK");

                    }
                    else
                    {
                        conn.UpdateWithChildren(tournament);
                        DisplayAlert("Success", "Tournament successfully updated", "OK");

                    }
                }
                catch
                {
                    DisplayAlert("Failure", "An error occurred when creating this tournament", "OK");

                }

                Navigation.PopAsync();
            }
            

        }
    }
}