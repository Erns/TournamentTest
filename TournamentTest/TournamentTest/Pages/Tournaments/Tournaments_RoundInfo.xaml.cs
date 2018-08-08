using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentTest.Classes;
using TournamentTest.MVVM;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TournamentTest.Pages.Tournaments
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Tournaments_RoundInfo : ContentPage
	{

        private int intRoundId = 0;
        //List<TournamentMainRoundTable> bvm;

        public Tournaments_RoundInfo (string strTitle, int intRoundId)
		{
			InitializeComponent ();
            Title = strTitle;
            this.intRoundId = intRoundId;



            //bvm = new List<TournamentMainRoundTable>();
            //BindingContext = bvm;

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                Utilities.InitializeTournamentMain(conn);

                TournamentMainRound round = new TournamentMainRound();
                round = conn.GetWithChildren<TournamentMainRound>(intRoundId);

                tournamentTableListView.ItemsSource = round.Tables;

                //ObservableCollection<TournamentMainRoundTable_ViewModel> lstTables = new ObservableCollection<TournamentMainRoundTable_ViewModel>();
                //foreach (TournamentMainRoundTable table in round.Tables)
                //{
                //    lstTables.Add(new TournamentMainRoundTable_ViewModel(table));
                //}
                //tournamentTableListView.ItemsSource = lstTables;

            }
        }
    }
}