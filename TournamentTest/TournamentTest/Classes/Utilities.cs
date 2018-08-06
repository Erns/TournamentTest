using System;
using System.Collections.Generic;
using System.Text;

namespace TournamentTest.Classes
{
    public static class Utilities
    {
        public static void InitializeTournamentMain(SQLite.SQLiteConnection conn)
        {
            //Need to create all tables for SQLite
            conn.CreateTable<TournamentMain>();
            conn.CreateTable<TournamentMainPlayer>();
            conn.CreateTable<TournamentMainRound>();
            conn.CreateTable<TournamentMainRoundPlayer>();
            conn.CreateTable<TournamentMainRoundTable>();
            //conn.CreateTable<TournamentMainRoundResult>();

        }

        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

    }

   

}
