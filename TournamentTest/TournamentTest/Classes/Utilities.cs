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
            //conn.CreateTable<TournamentMainRoundPlayer>();
            conn.CreateTable<TournamentMainRoundTable>();

            conn.CreateTable<Player>();
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

        public static void CalculatePlayerScores(ref TournamentMain objTournMain)
        {

            int intScoreDiff = 0;
            bool blnWinner = false;

            //Reset each player's score
            foreach (TournamentMainPlayer player in objTournMain.Players)
            {
                player.MOV = 0;
                player.RoundsPlayed = 0;
                player.Score = 0;
                player.SOS = 0;

                //Go through each round, find their table and calculate the score
                foreach (TournamentMainRound round in objTournMain.Rounds)
                {
                    foreach(TournamentMainRoundTable table in round.Tables)
                    {

                        if (table.Player1Id == player.Id || table.Player2Id == player.Id)
                        {
                            player.RoundsPlayed++;
                            blnWinner = false;

                            //Set the table's score difference and if player is the winner
                            if (table.Player1Winner)
                            {
                                intScoreDiff = table.Player1Score - table.Player2Score;
                                if (player.Id == table.Player1Id) blnWinner = true;
                            }
                            else if (table.Player2Winner)
                            {
                                intScoreDiff = table.Player2Score - table.Player1Score;
                                if (player.Id == table.Player2Id) blnWinner = true;
                            }

                            //Set points if score tied for the table
                            if (table.ScoreTied) player.MOV += objTournMain.MaxPoints;

                            //Set score and MOV
                            if (blnWinner)
                            {
                                player.Score++;

                                if (!table.ScoreTied)
                                    player.MOV += (intScoreDiff + objTournMain.MaxPoints);
                            }
                            else
                            {
                                if (!table.ScoreTied)
                                    player.MOV += (objTournMain.MaxPoints - intScoreDiff);
                            }

                            break;
                        }
                    }
                }
            }



            
        }

    }

   

}
