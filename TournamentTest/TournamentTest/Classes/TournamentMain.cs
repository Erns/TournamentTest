using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TournamentTest.Classes
{
    public class TournamentMain : INotifyPropertyChanged
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

        public event PropertyChangedEventHandler PropertyChanged;

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

    public class TournamentMainRoundTable
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

        public Nullable<int> Player1Score { get; set; } = null;

        [ForeignKey(typeof(Player))]
        public int Player2Id { get; set; } = 0;

        public string Player2Name { get; set; } = "N/A";

        public Nullable<int> Player2Score { get; set; } = null;

        public Nullable<int> Winner { get; set; } = null;

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
