using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using TournamentTest.MVVM;
using Xamarin.Forms;

namespace TournamentTest.Classes
{
    public class TournamentMain
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public int MaxPoints { get; set; }
        public Nullable<DateTime> DateDeleted { get; set; } = null;

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<TournamentMainPlayer> Players { get; set; } = new List<TournamentMainPlayer>();

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<TournamentMainRound> Rounds { get; set; } = new List<TournamentMainRound>();

        public string PlayersList()
        {
            List<string> lstIDs = new List<string>();
            foreach (TournamentMainPlayer item in Players)
            {
                lstIDs.Add(item.PlayerId.ToString());
            }

            return String.Join(",", lstIDs.ToArray());
        }
    }

    public class TournamentMainPlayer
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(TournamentMain))]
        public int TournmentId { get; set; }

        [ForeignKey(typeof(Player))]
        public int PlayerId { get; set; }

        public bool Active { get; set; } = true;

        public int Score { get; set; }

        public int MOV { get; set; }

    }

    public class TournamentMainRound 
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(TournamentMain))]
        public int TournmentId { get; set; }

        public int Number { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<TournamentMainRoundPlayer> Players { get; set; } = new List<TournamentMainRoundPlayer>();

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<TournamentMainRoundTable> Tables { get; set; } = new List<TournamentMainRoundTable>();

        //[OneToMany(CascadeOperations = CascadeOperation.All)]
        //public List<TournamentMainRoundResult> Results { get; set; } = new List<TournamentMainRoundResult>();

    }

    public class TournamentMainRoundPlayer
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(TournamentMainRound))]
        public int RoundId { get; set; }

        [ForeignKey(typeof(Player))]
        public int PlayerId { get; set; }

        public bool Active { get; set; } = true;
    }


    public class TournamentMainRoundTable : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(TournamentMainRound))]
        public int RoundId { get; set; }

        public int Number { get; set; }

        public string TableName { get; set; }

        [ForeignKey(typeof(Player))]
        public int Player1Id { get; set; } = 0;

        public string Player1Name { get; set; } = "N/A";


        [ForeignKey(typeof(Player))]
        public int Player2Id { get; set; } = 0;

        public string Player2Name { get; set; } = "N/A";

        private int _recursiveLvl = 0;  //Tracking how nested the recursion gets, trigger save on the last level to prevent unnecessary/excessive updates


        public bool _scoreTied { get; set; } = false;
        public bool ScoreTied
        {
            get { return _scoreTied; }
            set
            {
                if (_scoreTied != value)
                {
                    _scoreTied = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _player1Winner { get; set; } = false;
        public bool Player1Winner
        {
            get { return _player1Winner; }
            set
            {
                if (_player1Winner != value)
                {
                    _recursiveLvl += 1;
                    _player1Winner = value;
                    if (_player1Winner) Player2Winner = false;
                    OnPropertyChanged();
                    UpdateRoundTable();
                    _recursiveLvl -= 1;
                }
            }
        }

        private bool _player2Winner { get; set; } = false;
        public bool Player2Winner
        {
            get { return _player2Winner; }
            set
            {
                if (_player2Winner != value)
                {
                    _recursiveLvl += 1;
                    _player2Winner = value;
                    if (_player2Winner) Player1Winner = false;
                    OnPropertyChanged();
                    UpdateRoundTable();
                    _recursiveLvl -= 1;
                }
            }
        }


        private int _player1Score { get; set; }
        public int Player1Score
        {
            get { return _player1Score; }
            set
            {
                if (_player1Score != value)
                {
                    _recursiveLvl += 1;
                    _player1Score = value;
                    OnPropertyChanged();
                    UpdateScores();
                    UpdateRoundTable();
                    _recursiveLvl -= 1;
                }
            }
        }

        private int _player2Score { get; set; }
        public int Player2Score
        {
            get { return _player2Score; }
            set
            {
                if (_player2Score != value)
                {
                    _recursiveLvl += 1;
                    _player2Score = value;                    
                    OnPropertyChanged();
                    UpdateScores();
                    UpdateRoundTable();
                    _recursiveLvl -= 1;
                }
            }
        }

        private void UpdateScores()
        {
            ScoreTied = false;
            if (Player1Score > Player2Score)
            {
                Player1Winner = true;
            }
            else if (Player2Score > Player1Score)
            {
                Player2Winner = true;
            }
            else
            {
                ScoreTied = true;
            }
        }

        private void UpdateRoundTable()
        {
            if (Id == 0 || _recursiveLvl > 1) return;   

            using (SQLiteConnection conn = new SQLiteConnection(App.DB_PATH))
            {
                Utilities.InitializeTournamentMain(conn);

                TournamentMainRoundTable roundTable = this;
                try
                {
                    conn.Update(roundTable);
                }
                catch
                {
                    //Do nothing.  When initialized, if a connection is already open it'll be busy.  No need to update while already loading.
                    //Not currently able to find a way to check if a different connection to same tables are open
                }
                
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

    //public class TournamentMainRoundResult
    //{
    //    [PrimaryKey, AutoIncrement]
    //    public int Id { get; set; }

    //    [ForeignKey(typeof(TournamentMainRound))]
    //    public int RoundId { get; set; }

    //    [ForeignKey(typeof(Player))]
    //    public int PlayerId { get; set; }

    //    [ForeignKey(typeof(Player))]
    //    public int OpponentPlayerId { get; set; }

    //    public Nullable<int> Score { get; set; } = null;

    //    public bool Win { get; set; }
    //}



}
