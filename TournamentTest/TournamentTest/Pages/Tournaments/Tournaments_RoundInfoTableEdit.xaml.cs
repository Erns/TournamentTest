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
	public partial class Tournaments_RoundInfoTableEdit : ContentPage
	{
        List<clsPlayerInfo> lstPlayers = new List<clsPlayerInfo>();


        public Tournaments_RoundInfoTableEdit (int intRoundId, int intTableId)
		{
			InitializeComponent ();
            int intPlayer1Id = 0;
            int intPlayer2Id = 0;

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                TournamentMainRound round = new TournamentMainRound();
                round = conn.GetWithChildren<TournamentMainRound>(intRoundId);

                lstPlayers = new List<clsPlayerInfo>();

                foreach(TournamentMainRoundTable table in round.Tables)
                {
                    if (table.Player1Id > 0)
                        lstPlayers.Add(new clsPlayerInfo(table.Player1Id, table.Player1Name));

                    if (table.Player2Id > 0)
                        lstPlayers.Add(new clsPlayerInfo(table.Player2Id, table.Player2Name));

                    if (table.Id == intTableId)
                    {
                        Title = table.TableName;
                        lblPlayer1.Text = table.Player1Name;
                        lblPlayer2.Text = table.Player2Name;

                        intPlayer1Id = table.Player1Id;
                        intPlayer2Id = table.Player2Id;                       
                    }
                        
                }

                pckPlayer1.ItemsSource = lstPlayers.OrderBy(obj => obj.PlayerName).AsQueryable().Where(o => o.PlayerId != intPlayer1Id).ToList();
                pckPlayer2.ItemsSource = lstPlayers.OrderBy(obj => obj.PlayerName).AsQueryable().Where(o => o.PlayerId != intPlayer2Id).ToList();

                pckPlayer1.SelectedItem = new clsPlayerInfo(intPlayer1Id, lblPlayer1.Text);
                pckPlayer2.SelectedItem = new clsPlayerInfo(intPlayer2Id, lblPlayer2.Text);
            }
        }

        private class clsPlayerInfo
        {
            public int PlayerId { get; set; }
            public string PlayerName { get; set; }

            public clsPlayerInfo(int Id, string Name)
            {
                this.PlayerId = Id;
                this.PlayerName = Name;
            }

        }

        private void pckPlayer1_SelectedIndexChanged(Picker sender, EventArgs e)
        {
            if (sender.SelectedIndex < 0) return;

            //string strName = sender.Items[sender.SelectedIndex];

            //clsPlayerInfo tmp = (clsPlayerInfo)sender.SelectedItem;

            //sender.ItemsSource = lstPlayers.OrderBy(obj => obj.PlayerName).AsQueryable().Where(o => o.PlayerName != strName).ToList();
        }

        private void pckPlayer2_SelectedIndexChanged(Picker sender, EventArgs e)
        {

        }

    }
}