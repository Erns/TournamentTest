using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using TournamentTest.Classes;

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


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
