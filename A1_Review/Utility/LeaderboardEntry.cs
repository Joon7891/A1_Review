//Author: Joon Song
//Project Name: A1_Review
//File Name: LeaderboardEntry.cs
//Creation Date: 09/19/2018
//Modified Date: 09/19/2018
//Description: Class to hold LeaderboardEntry, a struct which holds various data regarding a player's game

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A1_Review
{
    public struct LeaderboardEntry
    {
        /// <summary>
        /// The name of the player
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The score of the player
        /// </summary>
        public byte Score { get; private set; }

        /// <summary>
        /// The time of the player
        /// </summary>
        public double Time { get; private set; }

        /// <summary>
        /// Counstructor for LeaderboardEntry struct
        /// </summary>
        /// <param name="name">The name of the player</param>
        /// <param name="score">The score of the player</param>
        /// <param name="time">The time of the player</param>
        public LeaderboardEntry(string name, byte score, double time)
        {
            //Setting constructor parameters to object properties
            Name = name;
            Score = score;
            Time = time;
        }
    }
}
