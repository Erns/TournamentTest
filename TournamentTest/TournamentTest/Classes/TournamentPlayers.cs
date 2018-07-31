using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TournamentTest.Classes
{
    public class TournamentPlayers
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TournmentId { get; set; }
        public int PlayerId { get; set; }
    }
}
