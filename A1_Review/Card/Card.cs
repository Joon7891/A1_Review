//Author: Joon Song
//Project Name: A1_Review
//File Name: Card.cs
//Creation Date: 09/10/2018
//Modified Date: 09/18/2018
//Desription: Class to hold Card, an object that represents a classic poker card

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace A1_Review
{
    public sealed class Card
    {
        /// <summary>
        /// The suit of the card
        /// </summary>
        public SuitType Suit { get; }

        /// <summary>
        /// The rank of the card
        /// </summary>
        public RankType Rank { get; }

        /// <summary>
        /// Variable to hold information on whether card face is up or not
        /// </summary>
        public bool IsFaceUp { private get; set; }

        /// <summary>
        /// Variable to hold information on whether card is clicked or not
        /// </summary>
        public bool IsClicked { get => MouseHelper.NewClick() && CollisionDetection.PointToRect(Main.mouseLoc, rect); }

        //Variables to hold the image and rectangular dimensions of card
        private Rectangle rect;
        private Texture2D img;

        //Variables of data required for card movement
        private double nonRoundedX;
        private double nonRoundedY;
        private Rectangle destinationRect;
        private float timeMoved = 0.0f;       
        private Vector2 velocity = new Vector2();

        /// <summary>
        /// Variable containing information on if the card is moving 
        /// </summary>
        public bool IsMoving { get => !(rect.X == destinationRect.X && rect.Y == destinationRect.Y) && timeMoved >= 0; }

        /// <summary>
        /// Constructor for card object
        /// </summary>
        /// <param name="suit">The suit of the card</param>
        /// <param name="rank">The rank of the card</param>
        /// <param name="rect">The rectangle representing the rectangular dimensions of the card</param>
        public Card(SuitType suit, RankType rank, Rectangle rect)
        {
            //Setting values of 'Suit' and 'Rank' properties from parameters
            Suit = suit;
            Rank = rank;

            //Setting up card image and rectangular dimensions, given parameters
            this.rect = rect;
            destinationRect = rect;
            img = CardHelper.CardImageDictionary[$"{suit}{rank}"];

            //Setting card face to be down by default
            IsFaceUp = false;
        }

        /// <summary>
        /// Subprogram to return an empty card; used as filler
        /// </summary>
        /// <param name="row">The row of the card in the pile</param>
        /// <param name="column">The column of the card in the pile</param>
        /// <returns>An empty card</returns>
        public static Card GetEmptyCard(byte row, byte column)
        {
            //Returning an empty card
            return new Card(0, (RankType) 1, CardHelper.CardPileRects[row, column]);
        }

        /// <summary>
        /// Updates various card components
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        public void Update(GameTime gameTime)
        {
            //Updating time moved
            timeMoved += (float)(gameTime.ElapsedGameTime.Milliseconds / 1000.0);

            //Calling subprogram to move card if card is moving
            if (IsMoving)
            {
                Move(gameTime);
            }
        }

        /// <summary>
        /// Draws card
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw card</param>
        /// <param name="color">The color to draw the card in</param>
        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            //If card face is up, draw card face or else, draw card back
            if (IsFaceUp)
            {
                spriteBatch.Draw(img, rect, color);
            }
            else
            {
                spriteBatch.Draw(CardHelper.CardBack, rect, color);
            }
        }
        
        /// <summary>
        /// Subprogram to handle movement logic of moving card
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        private void Move(GameTime gameTime)
        {
            //Moving card rectangle
            nonRoundedX += velocity.X * gameTime.ElapsedGameTime.Milliseconds / 1000.0;
            nonRoundedY += velocity.Y * gameTime.ElapsedGameTime.Milliseconds / 1000.0;
            rect.X = (int)(nonRoundedX + 0.5);
            rect.Y = (int)(nonRoundedY + 0.5);

            //Adjusting x-velocity if card will pass destination
            if (Math.Abs(rect.X - destinationRect.X) * 60 < Math.Abs(velocity.X))
            {
                velocity.X = (destinationRect.X - rect.X) * 60;
            }

            //Adjusting y-velocity if card will pass destination
            if (Math.Abs(rect.Y - destinationRect.Y) * 60 < Math.Abs(velocity.Y))
            {
                velocity.Y = (destinationRect.Y - rect.Y) * 60;
            }
        }

        /// <summary>
        /// Subprogram to setup the movement of the card
        /// </summary>
        /// <param name="destinationRect">The rectangular desination of the card movement</param>
        /// <param name="timeToMove">The time alloted for card movement</param>
        /// <param name="timeToMoveIn">The time in which movement will start</param>
        public void SetUpMovement(Rectangle destinationRect, float timeToMove, float timeToMoveIn = 0.0f)
        {
            //Setting up velocity vector for movement
            velocity.X = (destinationRect.X - rect.X) / timeToMove;
            velocity.Y = (destinationRect.Y - rect.Y) / timeToMove;

            //Setting up variables to act as the card's location before rounding
            nonRoundedX = rect.X;
            nonRoundedY = rect.Y;
            
            //Setting up timing and location variables
            this.destinationRect = destinationRect;
            timeMoved = -timeToMoveIn;
        }
    }
}
