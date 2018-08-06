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
	public partial class Tournaments_RoundInfo : ContentPage
	{

        private int intRoundId = 0;

        private Dictionary<int, int?> dctPlayerScores;

		public Tournaments_RoundInfo (string strTitle, int intRoundId)
		{
			InitializeComponent ();
            Title = strTitle;
            this.intRoundId = intRoundId;
		}


        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                Utilities.InitializeTournamentMain(conn);

                TournamentMainRound round = conn.GetWithChildren<TournamentMainRound>(intRoundId);

                List<TournamentMainRoundTable> lstTables = round.Tables;

                tournamentTableListView.ItemsSource = lstTables;


                dctPlayerScores = new Dictionary<int, int?>();

                foreach (TournamentMainRoundTable table in lstTables)
                {
                    if (table.Player1Id > 0)
                    {
                        if(!dctPlayerScores.ContainsKey(table.Player1Id)) dctPlayerScores.Add(table.Player1Id, table.Player1Score);
                    }

                    if (table.Player2Id > 0)
                    {
                        if (!dctPlayerScores.ContainsKey(table.Player2Id)) dctPlayerScores.Add(table.Player2Id, table.Player2Score);
                    }
                }


            }

        }

        void Handle_FabClicked(object sender, System.EventArgs e)
        {

        }

        private void Entry_PropertyChanged_Score1(Entry sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //UpdateTableScores(sender, 1);
        }


        private void Entry_PropertyChanged_Score2(Entry sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //UpdateTableScores(sender, 2);
        }

        private void UpdateTableScores(Entry sender, int playerNumber)
        {
            if (sender.ClassId is null) return;

            int intTableId = Convert.ToInt32(sender.ClassId);
            int intPlayerId = Convert.ToInt32(sender.StyleClass);

            int? intCurrentScore = null;
            int? intSavedScore = null;

            if (sender.Text != "") intCurrentScore = Convert.ToInt32(Math.Round(Convert.ToDecimal(sender.Text)));
            if (dctPlayerScores.ContainsKey(intPlayerId)) intSavedScore = dctPlayerScores[intPlayerId];

            if (intSavedScore != intCurrentScore)
            {
                using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
                {
                    Utilities.InitializeTournamentMain(conn);

                    TournamentMainRoundTable roundTable = conn.Get<TournamentMainRoundTable>(intTableId);
                    if (playerNumber == 1) roundTable.Player1Score = intCurrentScore;
                    else roundTable.Player2Score = intCurrentScore;

                    conn.Update(roundTable);
                }
            }
        }

        private void Entry_Completed(object sender, EventArgs e)
        {
            
        }

        private void Entry_TextChanged(Entry sender, TextChangedEventArgs e)
        {
         
        }
    }
}