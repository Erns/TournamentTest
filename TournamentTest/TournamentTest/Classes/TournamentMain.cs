using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public class TournamentMainRound : TournamentMain_BaseViewModel
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

    public class TournamentMainRoundTable : TournamentMain_BaseViewModel
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
        
        public bool _player1Winner { get; set; } = false;
        public bool Player1Winner
        {
            get { return _player1Winner; }
            set
            {
                _player1Winner = value;
                RaisePropertyChanged();
            }
        }

        public bool _player2Winner { get; set; } = false;
        public bool Player2Winner
        {
            get { return _player2Winner; }
            set
            {
                _player2Winner = value;
                RaisePropertyChanged();
            }
        }


        private Nullable<int> _player1Score { get; set; } = null;
        public Nullable<int> Player1Score
        {
            get { return _player1Score; }
            set
            {
                _player1Score = value;
                RaisePropertyChanged();
                UpdateScores(1);
            }
        }

        private Nullable<int> _player2Score { get; set; } = null;
        public Nullable<int> Player2Score
        {
            get { return _player2Score; }
            set
            {
                _player2Score = value;
                RaisePropertyChanged();
                UpdateScores(2);
            }
        }

        private void UpdateScores(int intPlayer)
        {
            if (Player1Score > Player2Score)
            {
                Player1Winner = true;
                Player2Winner = false;
                RaisePropertyChanged();
            }
            else if (Player2Score > Player1Score)
            {
                Player1Winner = false;
                Player2Winner = true;
                RaisePropertyChanged();
            }
        }

        //public Command SaveCommand
        //{
        //    get
        //    {
        //        return new Command(() => {
        //            Message = "I am " + EmployeeModel.Name + ", My qualification is " + EmployeeModel.Qualification + " and working as a " + EmployeeModel.Designation;
        //        });
        //    }
        //}
        //public event PropertyChangedEventHandler PropertyChanged;
        //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

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
