using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TournamentTest.Classes
{
    public class TournamentRounds
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public int RoundNumber { get; set; }
        public int TableNumber { get; set; }
        public int Player1Id { get; set; }
        public int Player1Score { get; set; }
        public int Player2Id { get; set; }
        public int Player2Score { get; set; }
        public int Winner { get; set; }

    }
}
