using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TournamentTest.Classes
{
    public class TournamentMain
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }

        //public List<Player> Players { get; set; }
        //public List<Round> Rounds { get; set; }

        //public class Round
        //{
        //    public int Number { get; set; }
        //    public List<Player> Players { get; set; }
        //    public List<Table> Tables { get; set; }
        //    public List<Result> Results { get; set; }

        //    public class Table
        //    {
        //        public int Number { get; set; }
        //        public int Player1Id { get; set; }
        //        public int Player2Id { get; set; }
        //    }

        //    public class Result
        //    {
        //        public int PlayerId { get; set; }
        //        public int OpponentPlayerId { get; set; }
        //        public int Score { get; set; }
        //        public bool Win { get; set; }
        //    }
        //}
    }
}
