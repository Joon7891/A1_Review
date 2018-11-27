//Author: Joon Song
//Project Name: A1_Review
//File Name: CardHelper.cs
//Creation Date: 09/10/2018
//Modified Date: 09/18/2018
//Desription: Class to hold resources and subprograms that help with card object's functionality

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace A1_Review
{
    public static class CardHelper
    {
        /// <summary>
        /// Dictionary to hold card face images
        /// </summary>
        public static Dictionary<string, Texture2D> CardImageDictionary { get; private set; }

        /// <summary>
        /// Image of card back
        /// </summary>
        public static Texture2D CardBack;

        /// <summary>
        /// Rectangle representing rectangular dimensions of cards in card deck
        /// </summary>
        public static Rectangle CardDeckRect { get; private set; }

        /// <summary>
        /// 2D array of rectangles representing the rectangular dimensions of the cards in each card pile stack
        /// </summary>
        public static Rectangle[,] CardPileRects { get; private set; }

        /// <summary>
        /// Loads various CardHelper components 
        /// </summary>
        /// <param name="content"></param>
        public static void Load(ContentManager content)
        {
            //Setting up card deck and pile rectangles
            CardDeckRect = new Rectangle(355, 15, 110, 150);
            CardPileRects = new Rectangle[2, 6];
            for (byte i = 0; i < CardPileRects.GetLength(0); i++)
            {
                for (byte j = 0; j < CardPileRects.GetLength(1); j++)
                {
                    CardPileRects[i, j] = new Rectangle(60 + 114 * j, 170 + 154 * i, 110, 150);
                }
            }

            //Loading in card back
            CardBack = content.Load<Texture2D>("Images/Sprites/Cards/CardBack");

            //Constructing and setting up card image dictionary
            CardImageDictionary = new Dictionary<string, Texture2D>();
            for (byte suit = 0; suit < Enum.GetNames(typeof(SuitType)).Length; suit++)
            {
                for (byte rank = 0; rank < Enum.GetNames(typeof(RankType)).Length; rank++)
                {
                    CardImageDictionary.Add($"{(SuitType)suit}{(RankType)(rank + 1)}", content.Load<Texture2D>($"Images/Sprites/Cards/cardFace{(SuitType)suit}{(RankType)(rank + 1)}"));
                }
            }
        }
    }
}
