//Author: Joon Song
//Project Name: A1_Review
//File Name: ListHelper.cs
//Creation Date: 09/17/2018
//Modified Date: 09/17/2018
//Desription: Class to hold subprograms that help with a List's functionality

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A1_Review
{
    public static class ListHelper
    {
        //Instance of random number generator
        private static Random rng = new Random();

        /// <summary>
        /// Subprogram to shuffle a given list
        /// </summary>
        /// <typeparam name="T">The type of the list</typeparam>
        /// <param name="list">The list to be shuffled</param>
        /// <returns>The shuffled list</returns>
        public static List<T> Shuffle<T>(List<T> list)
        {
            //Shuffled list which is to be returned from subprogram
            List<T> shuffledList = new List<T>();

            //Byte variable to keep track of which index is being added
            byte addIndex;

            //Randomly adding elements in the initial list to the shuffled list
            while (list.Count > 0)
            {
                addIndex = (byte) rng.Next(0, list.Count);
                shuffledList.Add(list[addIndex]);
                list.RemoveAt(addIndex);
            }

            //Returning shuffled list
            return shuffledList;
        }
    }
}
