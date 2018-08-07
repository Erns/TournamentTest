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

                TournamentMainRound round = conn.GetWithChildren<TournamentMainRound>(intRoundId);

                BindingContext = round;

                //ObservableCollection<TournamentMainRoundTable> lstTables = new ObservableCollection<TournamentMainRoundTable>();

                //foreach (TournamentMainRoundTable table in round.Tables)
                //{
                //    lstTables.Add(new TournamentMainRoundTable());
                //}
                //BindingContext = lstTables;

                //tournamentTableListView.ItemsSource = lstTables;
                
            }
        }

        void Handle_FabClicked(object sender, System.EventArgs e)
        {

        }


        private void Entry_TextChanged_Score1(Entry sender, TextChangedEventArgs e)
        {
            UpdateTableScores(sender, e.NewTextValue, 1);
        }

        private void Entry_TextChanged_Score2(Entry sender, TextChangedEventArgs e)
        {
            UpdateTableScores(sender, e.NewTextValue, 2);
        }


        private void UpdateTableScores(Entry sender, string strNewValue, int playerNumber)
        {

            int intTableId = Convert.ToInt32(sender.ReturnCommandParameter);
            if (intTableId == 0) return;

            Nullable<int> intNewValue = null;

            if (strNewValue != "") intNewValue = Convert.ToInt32(strNewValue);

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                //Utilities.InitializeTournamentMain(conn);

                //TournamentMainRoundTable roundTable = conn.Get<TournamentMainRoundTable>(intTableId);
                //if (playerNumber == 1) roundTable.Player1Score = intNewValue;
                //else roundTable.Player2Score = intNewValue;

                ////if (roundTable.Player1Score > roundTable.Player2Score)
                ////{
                ////    roundTable.Player1Winner = true;
                ////    roundTable.Player2Winner = false;
                ////}
                ////else if (roundTable.Player2Score > roundTable.Player1Score)
                ////{
                ////    roundTable.Player1Winner = false;
                ////    roundTable.Player2Winner = true;
                ////}
                ////else
                ////{
                ////    roundTable.Player1Winner = false;
                ////    roundTable.Player2Winner = false;
                ////}

                //conn.Update(roundTable);

                ////TournamentMainRound round = conn.GetWithChildren<TournamentMainRound>(intRoundId);
                ////tournamentTableListView.ItemsSource = round.Tables;

            }
        }

        private void Switch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {

        }

    }
}