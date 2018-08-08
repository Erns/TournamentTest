using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using TournamentTest.Classes;
using Xamarin.Forms;

namespace TournamentTest.MVVM
{
    class TournamentMainRoundTable_ViewModel : INotifyPropertyChanged
    {

        private TournamentMainRoundTable _roundTable;
        public TournamentMainRoundTable TournamentMainRoundTable
        {
            get
            {
                return _roundTable;
            }
            set
            {
                _roundTable = value;
                OnPropertyChanged();
            }
        }

        public TournamentMainRoundTable_ViewModel(TournamentMainRoundTable table)
        {
            TournamentMainRoundTable = new TournamentMainRoundTable();
            TournamentMainRoundTable = table;
        }

        public Command SaveCommand
        {
            get
            {
                return new Command(() => {
                    using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
                    {
                        Utilities.InitializeTournamentMain(conn);

                        TournamentMainRoundTable roundTable = conn.Get<TournamentMainRoundTable>(TournamentMainRoundTable.Id);

                        roundTable.Player1Score = TournamentMainRoundTable.Player1Score;
                        roundTable.Player1Winner = TournamentMainRoundTable.Player1Winner;
                        roundTable.Player2Score = TournamentMainRoundTable.Player2Score;
                        roundTable.Player2Winner = TournamentMainRoundTable.Player2Winner;

                        conn.Update(roundTable);
                    }
                });
            }
        }

        public void UpdateRoundTable()
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                Utilities.InitializeTournamentMain(conn);

                TournamentMainRoundTable roundTable = conn.Get<TournamentMainRoundTable>(TournamentMainRoundTable.Id);

                roundTable.Player1Score = TournamentMainRoundTable.Player1Score;
                roundTable.Player1Winner = TournamentMainRoundTable.Player1Winner;
                roundTable.Player2Score = TournamentMainRoundTable.Player2Score;
                roundTable.Player2Winner = TournamentMainRoundTable.Player2Winner;

                conn.Update(roundTable);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
