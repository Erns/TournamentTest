using System;
using System.Collections.Generic;
using System.Text;

namespace TournamentTest.Classes
{
    public static class Utilities
    {
        public static void CreateTournamentMain(SQLite.SQLiteConnection conn)
        {
            //Need to create all tables for SQLite
            conn.CreateTable<TournamentMain>();
            conn.CreateTable<TournamentMainPlayer>();
            conn.CreateTable<TournamentMainRound>();
            conn.CreateTable<TournamentMainRoundTable>();
            conn.CreateTable<TournamentMainRoundTableResult>();
        }
    }

   

}
