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
	public partial class Tournaments_RoundInfo : ContentPage
	{

        private int intRoundId = 0;

        public Tournaments_RoundInfo (string strTitle, int intRoundId)
		{
			InitializeComponent ();
            Title = strTitle;
            this.intRoundId = intRoundId;

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                TournamentMainRound round = new TournamentMainRound();
                round = conn.GetWithChildren<TournamentMainRound>(intRoundId);

                //Set using the ViewModel version.  This allows being able to manipulate back and forth across the class properties, while displaying as intended on the GUI
                //while also ensuring none of the goings ons of the properties touching each other don't occur without this specific view model (such as the SQL table updates)
                ObservableCollection<TournamentMainRoundTable_ViewModel> lstTables = new ObservableCollection<TournamentMainRoundTable_ViewModel>();
                foreach (TournamentMainRoundTable table in round.Tables)
                {
                    lstTables.Add(new TournamentMainRoundTable_ViewModel(table));
                }
                tournamentTableListView.ItemsSource = lstTables;
            }
        }
    }
}