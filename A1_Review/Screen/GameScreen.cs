//Author: Joon Song
//Project Name: A1_Review
//File Name: GameScreen.cs
//Creation Date: 09/10/2018
//Modified Date: 09/20/2018
//Description: Class to hold various game screen components

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace A1_Review
{
    public static class GameScreen
    {
        //Object to hold on-screen background images and buttons
        private static Background selectionBackground;
        private static Background gameBackground;

        //Song object to hold background music
        private static Song backgroundMusic;

        //Enum and enum instance to hold gameState
        private enum GameState { InitialState, CardsMoving, Playing, Win, Loss };
        private static GameState gameState = GameState.InitialState;

        //Variables to help user load or create a new game
        private static Button[] gameSetupButtons = new Button[2];

        //Variables to hold the card deck and the 2 x 6 card piles + pile counts
        private static List<Card> cardDeck = new List<Card>();
        private static Stack<Card>[,] cardPiles = new Stack<Card>[2, 6];
        private static Card[,] cardPilesBottom = new Card[2, 6];
        private static Sprite[,] cardPilesCounter = new Sprite[2, 6];
        private static Vector2[,] cardPilesCounterLoc = new Vector2[2, 6];

        //Array of 2D vectors to hold the indexes of various cards that are selected; (-1, -1) represents null value
        private static Vector2[] selectedCardIndex = new Vector2[] { new Vector2(-1, -1), new Vector2(-1, -1) };

        //SpriteFont variables to hold various fonts used in game screen
        private static SpriteFont cardPileCountFont;
        private static SpriteFont userFeedbackFont;
        private static SpriteFont gameEndedFont;

        //Button to be clicked for user to 'make a move'
        private static Button makeMoveButton;
        private static Button[] makeMoveButtons = new Button[3];

        //Variables to output various game information
        private static double playTime;
        private static string[] userFeedbackTxt = { string.Empty, string.Empty };
        private static Vector2[] msgLocs = { new Vector2(80, 20), new Vector2(80, 50), new Vector2(80, 80), new Vector2(80, 110), new Vector2(80, 130) };
        private static Vector2[] gameEndMsgLocs = { new Vector2(270, 175), new Vector2(290, 175), new Vector2(237, 265), new Vector2(0, 335), new Vector2(62, 465) };
        private static string finalMessage;
        private static string playerName = string.Empty;

        /// <summary>
        /// Subprogram to load various game screen components
        /// </summary>
        /// <param name="content">ContentManager to load content</param>
        public static void Load(ContentManager content)
        {
            //Loading in game setup buttons
            gameSetupButtons[0] = new Button(content.Load<Texture2D>("Images/Sprites/Buttons/newGameButton"), new Rectangle(630, 540, 150, 40), () => NewGame());
            if (IO.GameDataExists)
            {
                gameSetupButtons[1] = new Button(content.Load<Texture2D>("Images/Sprites/Buttons/loadGameButton"), new Rectangle(630, 490, 150, 40), () => LoadGame());
            }
            else
            {
                gameSetupButtons[1] = null;
            }

            //Importing and setting up pile stack components
            Texture2D pileCounterImg = content.Load<Texture2D>("Images/Sprites/stackCounter");
            for (byte row = 0; row < cardPilesCounter.GetLength(0); row++)
            {
                for (byte column = 0; column < cardPilesCounter.GetLength(1); column++)
                {
                    cardPiles[row, column] = new Stack<Card>();
                    cardPilesCounter[row, column] = new Sprite(pileCounterImg, new Rectangle(150 + 114 * column, 170 + 154 * row, 20, 20));
                    cardPilesCounterLoc[row, column] = new Vector2(155 + 114 * column, 170 + 154 * row);
                }
            }

            //Loading in background music, image, and fonts
            selectionBackground = new Background(content.Load<Texture2D>("Images/Backgrounds/howToPlay"));
            gameBackground = new Background(content.Load<Texture2D>("Images/Backgrounds/gameScreenBack"));
            backgroundMusic = content.Load<Song>("Audio/Music/gameScreenMusic");
            cardPileCountFont = content.Load<SpriteFont>("Fonts/CardPileCountFont");
            userFeedbackFont = content.Load<SpriteFont>("Fonts/UserFeedbackFont");
            gameEndedFont = content.Load<SpriteFont>("Fonts/GameEndFont");

            //Setting up "make-move" button
            makeMoveButtons[0] = new Button(content.Load<Texture2D>("Images/Sprites/Buttons/makeMoveButton0"), new Rectangle(300, 500, 200, 40), () => { });
            makeMoveButtons[1] = new Button(content.Load<Texture2D>("Images/Sprites/Buttons/makeMoveButton1"), new Rectangle(300, 500, 200, 40), ReplaceFaceCard);
            makeMoveButtons[2] = new Button(content.Load<Texture2D>("Images/Sprites/Buttons/makeMoveButton2"), new Rectangle(300, 500, 200, 40), ReplaceElevenPair);
            makeMoveButton = makeMoveButtons[0];
        }

        /// <summary>
        /// Subprogram to set up a new game
        /// </summary>
        private static void NewGame()
        {
            //Setting up and shuffling deck of cards
            cardDeck = new List<Card>();
            for (byte suit = 0; suit < 4; suit++)
            {
                for (byte rank = 1; rank < 14; rank++)
                {
                    cardDeck.Add(new Card((SuitType)suit, (RankType)rank, CardHelper.CardDeckRect));
                }
            }
            cardDeck = ListHelper.Shuffle(cardDeck);

            //Setting up 2 x 6 card piles
            cardPilesBottom = new Card[2, 6];
            for (byte row = 0; row < cardPiles.GetLength(0); row++)
            {
                for (byte column = 0; column < cardPiles.GetLength(1); column++)
                {
                    cardPiles[row, column].Clear();
                    cardDeck[0].SetUpMovement(CardHelper.CardPileRects[row, column], 0.2f, (row * 6 + column) * 0.2f);
                    cardDeck[0].IsFaceUp = true;
                    cardPiles[row, column].Push(cardDeck[0]);
                    cardDeck.RemoveAt(0);
                }
            }

            //Resetting game play time and feedback message
            playTime = 0;
            userFeedbackTxt[0] = string.Empty;
            userFeedbackTxt[1] = string.Empty;

            //Indicating that game has been set
            gameState = GameState.CardsMoving;

            //Uploading new game data to file
            IO.UploadGameData(playTime, cardDeck, cardPiles);
        }

        /// <summary>
        /// Subprogram to load a game
        /// </summary>
        private static void LoadGame()
        {
            //Loading in game time, piles, and deck
            playTime = IO.LoadedPlayTime;
            cardPiles = IO.LoadedCardPiles;
            cardDeck = IO.LoadedCardDeck;

            //Indicating that game has been setup
            gameState = GameState.CardsMoving;
        }

        /// <summary>
        /// Subprogram to update various game screen components
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        public static void Update(GameTime gameTime)
        {
            //Playing background music if music is not already playing
            if (MediaPlayer.State != MediaState.Playing)
            {
                MediaPlayer.Play(backgroundMusic);
            }

            //Updating game-state, if game is running
            if (gameState != GameState.InitialState && gameState != GameState.Loss && gameState != GameState.Win)
            {
                gameState = UpdateGameState();
            }

            //Updating game-time, if game is running
            if (gameState == GameState.CardsMoving || gameState == GameState.Playing)
            {
                playTime += gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0;
            }

            //Switch statement to call appropriate subprogram for the given game state
            switch (gameState)
            {
                case GameState.InitialState:
                    UpdateSetup(gameTime);
                    break;

                case GameState.CardsMoving:
                    UpdateCards(gameTime);
                    break;

                case GameState.Playing:
                    UpdateGame(gameTime);
                    break;

                case GameState.Win:
                    UpdateGameEnd(gameTime);
                    break;
                
                case GameState.Loss:
                    UpdateGameEnd(gameTime);
                    break;
            }

            //Updating back-button
            SharedData.BackButton.Update(gameTime);
        }

        /// <summary>
        /// Subprogram to update the setup of the game
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timming values</param>
        private static void UpdateSetup(GameTime gameTime)
        {
            //Updating the game setup buttons
            gameSetupButtons[0].Update(gameTime);
            if (gameSetupButtons[1] != null)
            {
                gameSetupButtons[1].Update(gameTime);
            }
        }

        /// <summary>
        /// Subprogram to hold playing logic for 11s game
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        private static void UpdateGame(GameTime gameTime)
        {
            //Calling subprogram to update cards
            UpdateCards(gameTime);

            //Looping through each card pile and checking if top card is selected
            for (byte row = 0; row < cardPiles.GetLength(0); row++)
            {
                for (byte column = 0; column < cardPiles.GetLength(1); column++)
                {
                    if (cardPiles[row, column].Peek().IsClicked)
                    {
                        //Adding card index to array of selected card indices; may clear previous card selections if necessary
                        if (selectedCardIndex[1].X == -1 && selectedCardIndex[1].Y == -1)
                        {
                            selectedCardIndex[1] = selectedCardIndex[0];
                            selectedCardIndex[0].X = column;
                            selectedCardIndex[0].Y = row;
                        }
                        else
                        {
                            selectedCardIndex[0].X = column;
                            selectedCardIndex[0].Y = row;
                            selectedCardIndex[1].X = -1;
                            selectedCardIndex[1].Y = -1;
                        }

                        //Adjusting "make-move" button to appropriate button given number of cards selected
                        makeMoveButton = makeMoveButtons[2 - Convert.ToByte(selectedCardIndex[0].X == -1 && selectedCardIndex[0].Y == -1) -
                            Convert.ToByte(selectedCardIndex[1].X == -1 && selectedCardIndex[1].Y == -1)];
                    }
                }
            }

            //Updating "make-move" button, if a move can be made
            if (makeMoveButton != makeMoveButtons[0])
            {
                makeMoveButton.Update(gameTime);
            }

            //Updating new game button
            gameSetupButtons[0].Update(gameTime);

            //If mouse is clicked and it did not click a card pile, reset selections and "make-move" button
            if (MouseHelper.NewClick() && DidNotSelectCard())
            {
                ResetSelectedPiles();
                makeMoveButton = makeMoveButtons[0];
            }
        }

        /// <summary>
        /// Subprogram to update cards
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        private static void UpdateCards(GameTime gameTime)
        {
            //Updating cards in the deck
            for (byte i = 0; i < cardDeck.Count; i++)
            {
                cardDeck[i].Update(gameTime);
            }

            //Updating cards on top of each pile
            for (byte row = 0; row < cardPiles.GetLength(0); row++)
            {
                for (byte column = 0; column < cardPiles.GetLength(1); column++)
                {
                    cardPiles[row, column].Peek().Update(gameTime);
                }
            }
        }

        /// <summary>
        /// Subprogram to handle logic after game as ended
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        private static void UpdateGameEnd(GameTime gameTime)
        {
            //Updating playaer name
            KeyboardHelper.BuildString(ref playerName, 8);
            gameEndMsgLocs[3].X = 400 - gameEndedFont.MeasureString(playerName).X / 2;

            //If enter is pressed, submit player data to leaderboard and reset game
            if (KeyboardHelper.NewKeyStroke(Keys.Enter))
            {
                LeaderboardScreen.UpdateLeaderboardData(new LeaderboardEntry(playerName, (byte)(52 - cardDeck.Count), Math.Round(playTime, 3)));
                playerName = string.Empty;
                Main.CurrentScreen = ScreenMode.Leaderboard;
                gameState = GameState.InitialState;
                IO.UploadBlankGameData();
                MediaPlayer.Stop();
            }
        }

        /// <summary>
        /// Subprogram to draw various game screen components
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw sprites</param>
        public static void Draw(SpriteBatch spriteBatch)
        {
            //Calling appropriate subprograms depending on whether the game is setup
            if (gameState == GameState.InitialState)
            {
                DrawSetup(spriteBatch);
            }
            else if (gameState == GameState.Win || gameState == GameState.Loss)
            {
                DrawGameEnd(spriteBatch);
            }
            else
            {
                DrawGame(spriteBatch);
            }

            //Drawing back button
            SharedData.BackButton.Draw(spriteBatch);
        }

        /// <summary>
        /// Subprogram to draw the game setup screen
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw sprites</param>
        private static void DrawSetup(SpriteBatch spriteBatch)
        {
            //Drawing how to play and setup buttons
            selectionBackground.Draw(spriteBatch);
            gameSetupButtons[0].Draw(spriteBatch);
            if (gameSetupButtons[1] != null)
            {
                gameSetupButtons[1].Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Subprogram to draw the game
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw sprites</param>
        private static void DrawGame(SpriteBatch spriteBatch)
        {
            //Drawing background
            gameBackground.Draw(spriteBatch);

            //Drawing 2 x 6 card piles and their counts
            for (byte row = 0; row < cardPiles.GetLength(0); row++)
            {
                for (byte column = 0; column < cardPiles.GetLength(1); column++)
                {
                    //Drawing card below if top card is in movement
                    if (cardPiles[row, column].Peek().IsMoving && cardPilesBottom[row, column] != null)
                    {
                        cardPilesBottom[row, column].Draw(spriteBatch, Color.White);
                    }

                    //Drawing card in yellow if it is a selected card, otherwise draw in white
                    if ((selectedCardIndex[0].X == column && selectedCardIndex[0].Y == row) ||
                        (selectedCardIndex[1].X == column && selectedCardIndex[1].Y == row))
                    {
                        cardPiles[row, column].Peek().Draw(spriteBatch, Color.Yellow);
                    }
                    else
                    {
                        cardPiles[row, column].Peek().Draw(spriteBatch, Color.White);
                    }

                    //Drawing card pile counter
                    cardPilesCounter[row, column].Draw(spriteBatch);
                    spriteBatch.DrawString(cardPileCountFont, "" + cardPiles[row, column].Count, cardPilesCounterLoc[row, column], Color.White);
                }
            }

            //Calling subprogram to draw various feedback
            DrawGameFeedback(spriteBatch);

            //Drawing "make-move" button; if a move can be made
            if (makeMoveButton != makeMoveButtons[0])
            {
                makeMoveButton.Draw(spriteBatch);
            }

            //Drawing card deck
            for (byte i = 0; i < cardDeck.Count; i++)
            {
                cardDeck[i].Draw(spriteBatch, Color.White);
            }

            //Drawing new game button
            gameSetupButtons[0].Draw(spriteBatch);
        }

        /// <summary>
        /// Subprogram to draw various end of game components
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw sprites</param>
        private static void DrawGameEnd(SpriteBatch spriteBatch)
        {
            //Drawing game background
            gameBackground.Draw(spriteBatch);
            
            //Calling subprogram to draw various game feedback
            DrawGameFeedback(spriteBatch);

            //Drawing game text
            if (finalMessage == "You Win!")
            {
                spriteBatch.DrawString(gameEndedFont, finalMessage, gameEndMsgLocs[0], Color.Red);
            }
            else
            {
                spriteBatch.DrawString(gameEndedFont, finalMessage, gameEndMsgLocs[1], Color.Red);
            }
            spriteBatch.DrawString(gameEndedFont, "Enter Name:", gameEndMsgLocs[2], Color.Black);
            spriteBatch.DrawString(gameEndedFont, "" + playerName, gameEndMsgLocs[3], Color.Black);
            spriteBatch.DrawString(gameEndedFont, "Press <Enter> to Continue", gameEndMsgLocs[4], Color.Red);
        }

        /// <summary>
        /// Subprogram to draw game feedback
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw sprites</param>
        private static void DrawGameFeedback(SpriteBatch spriteBatch)
        {
            //Drawing feedback for user
            spriteBatch.DrawString(userFeedbackFont, $"Time: {Math.Round(playTime, 3)}", msgLocs[0], Color.Red);
            spriteBatch.DrawString(userFeedbackFont, $"Cards Left: {cardDeck.Count}", msgLocs[1], Color.SaddleBrown);
            spriteBatch.DrawString(userFeedbackFont, "Feedback:", msgLocs[2], Color.Gold);
            for (byte i = 0; i < userFeedbackTxt.Length; i++)
            {
                spriteBatch.DrawString(userFeedbackFont, userFeedbackTxt[i], msgLocs[i + 3], Color.Silver);
            }
        }

        /// <summary>
        /// Subprogram to determined the current game state of the game
        /// </summary>
        /// <returns>The current game state</returns>
        private static GameState UpdateGameState()
        {
            //Returning thet game state is cards are moving if any of the cards are moving
            for (byte row = 0; row < cardPiles.GetLength(0); row++)
            {
                for (byte column = 0; column < cardPiles.GetLength(1); column++)
                {
                    if (cardPiles[row, column].Peek().IsMoving)
                    {
                        return GameState.CardsMoving;
                    }
                }
            }

            //Returning that game is a win or loss, informing player and uploading blank data
            if (GameWinState())
            {
                userFeedbackTxt[0] = "You Win!";
                finalMessage = "You Win!";
                userFeedbackTxt[1] = string.Empty;
                IO.UploadBlankGameData();
                return GameState.Win;
            }
            else if (GameLossState())
            {
                userFeedbackTxt[0] = "You Lose";
                finalMessage = "You Lose";
                userFeedbackTxt[1] = string.Empty;
                IO.UploadBlankGameData();
                return GameState.Loss;
            }

            //Otherwise returning that game state is playing
            return GameState.Playing;
        }

        /// <summary>
        /// Subprogram to check if game has been won
        /// </summary>
        /// <returns>A bool representing if game has been won</returns>
        private static bool GameWinState()
        {
            //If any of the cards is not a face card, return false
            for (byte row = 0; row < cardPiles.GetLength(0); row++)
            {
                for (byte column = 0; column < cardPiles.GetLength(1); column++)
                {
                    if ((int)cardPiles[row, column].Peek().Rank < 11)
                    {
                        return false;
                    }
                }
            }

            //Since all of the cards is a face card, return true
            return true;
        }

        /// <summary>
        /// Subprogram to check if game has been loss
        /// </summary>
        /// <returns>A bool representing if game has been loss</returns>
        private static bool GameLossState()
        {
            //If there are any face cards by itself in a pile, return false
            for (byte row = 0; row < cardPiles.GetLength(0); row++)
            {
                for (byte column = 0; column < cardPiles.GetLength(1); column++)
                {
                    if ((int)cardPiles[row, column].Peek().Rank > 10 && cardPiles[row, column].Count == 1)
                    {
                        return false;
                    }
                }
            }

            //If there exists any 11 pairs, return false
            for (byte row1 = 0; row1 < cardPiles.GetLength(0); row1++)
            {
                for (byte column1 = 0; column1 < cardPiles.GetLength(1); column1++)
                {
                    for (byte row2 = 0; row2 < cardPiles.GetLength(0); row2++)
                    {
                        for (byte column2 = 0; column2 < cardPiles.GetLength(1); column2++)
                        {
                            if ((int)cardPiles[row1, column1].Peek().Rank + (int)cardPiles[row2, column2].Peek().Rank == 11)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            //As there are no possible moves, return true
            return true;
        }

        /// <summary>
        /// Subprogram to replace face-card
        /// </summary>
        private static void ReplaceFaceCard()
        {
            //Assinging variables to indexes for convenience
            byte column = (byte)selectedCardIndex[0].X;
            byte row = (byte)selectedCardIndex[0].Y;

            //Checking if face-card is selected
            if ((int)cardPiles[row, column].Peek().Rank > 10)
            {
                //Checking if face-card is only card in pile
                if (cardPiles[row, column].Count == 1)
                {
                    //Replacing face-card with a card in the deck
                    cardPiles[row, column].Peek().SetUpMovement(CardHelper.CardDeckRect, 0.2f);
                    cardDeck[0].SetUpMovement(CardHelper.CardPileRects[row, column], 0.2f);
                    cardPiles[row, column].Peek().IsFaceUp = false;
                    cardDeck[0].IsFaceUp = true;
                    cardDeck.Add(cardPiles[row, column].Pop());
                    cardPiles[row, column].Push(cardDeck[0]);
                    cardDeck.RemoveAt(0);

                    //Informing user that move was successfully made
                    userFeedbackTxt[0] = "Face-Card successfully";
                    userFeedbackTxt[1] = "replaced.";
                }
                else
                {
                    //Informing user why move cannot be made
                    userFeedbackTxt[0] = "Error: Face-Card must be";
                    userFeedbackTxt[1] = "only card in pile.";
                }
            }
            else
            {
                //Informing user why move cannot be made
                userFeedbackTxt[0] = "Error: Face-Card was not";
                userFeedbackTxt[1] = "selected.";
            }

            //Resetting selected piles
            ResetSelectedPiles();

            //Resetting "make-move" button
            makeMoveButton = makeMoveButtons[0];

            //Uploading new game data to file
            IO.UploadGameData(playTime, cardDeck, cardPiles);
        }

        /// <summary>
        /// Subprogram to replace "11-pairs"
        /// </summary>
        private static void ReplaceElevenPair()
        {
            //Caching indexes for convenience
            byte[] columns = new byte[] { (byte)selectedCardIndex[0].X, (byte)selectedCardIndex[1].X };
            byte[] rows = new byte[] { (byte)selectedCardIndex[0].Y, (byte)selectedCardIndex[1].Y };

            //If pair adds up to 11, add cards to top of each deck the pair cards are in
            if ((int)cardPiles[rows[0], columns[0]].Peek().Rank + (int)cardPiles[rows[1], columns[1]].Peek().Rank == 11)
            {
                for (byte i = 0; i < 2; i++)
                {
                    cardDeck[0].SetUpMovement(CardHelper.CardPileRects[rows[i], columns[i]], 0.2f);
                    cardDeck[0].IsFaceUp = true;
                    cardPilesBottom[rows[i], columns[i]] = cardPiles[rows[i], columns[i]].Peek();
                    cardPiles[rows[i], columns[i]].Push(cardDeck[0]);
                    cardDeck.RemoveAt(0);

                    //Informing user that move was successfully made
                    userFeedbackTxt[0] = "11-Pair successfully";
                    userFeedbackTxt[1] = "replaced";
                }
            }
            else
            {
                //Informing user why move cannot be made
                userFeedbackTxt[0] = "Error: 11-Pair does not add";
                userFeedbackTxt[1] = "up to 11.";
            }

            //Resetting selected piles
            ResetSelectedPiles();

            //Resetting "make-move" button
            makeMoveButton = makeMoveButtons[0];

            //Uploading new game data to file
            IO.UploadGameData(playTime, cardDeck, cardPiles);
        }

        /// <summary>
        /// Subprogram to check if any of the cards have been clicked
        /// </summary>
        /// <returns>If card was not selected or not</returns>
        private static bool DidNotSelectCard()
        {
            //If the card selected any of the cards, return false
            for (byte row = 0; row < CardHelper.CardPileRects.GetLength(0); row++)
            {
                for (byte column = 0; column < CardHelper.CardPileRects.GetLength(1); column++)
                {
                    if (cardPiles[row, column].Peek().IsClicked)
                    {
                        return false;
                    }
                }
            }

            //Otherwise, returning true
            return true;
        }

        /// <summary>
        /// Subprogram to replace the selected piles of cards
        /// </summary>
        private static void ResetSelectedPiles()
        {
            //Resetting all of the selected piles
            for (byte i = 0; i < selectedCardIndex.Count(); i++)
            {
                selectedCardIndex[i].X = -1;
                selectedCardIndex[i].Y = -1;
            }
        }

        /// <summary>
        /// Subprogram to check '11-Pair' is valid
        /// </summary>
        /// <returns>Returns a bool reflecting the pair validity</returns>
        private static bool IsPairValid()
        {
            //If pair adds up to a value of 11, return true
            if ((int)cardPiles[(int)selectedCardIndex[0].Y, (int)selectedCardIndex[0].X].Peek().Rank +
                (int)cardPiles[(int)selectedCardIndex[1].Y, (int)selectedCardIndex[1].X].Peek().Rank == 11)
            {
                return true;
            }

            //Otherwise return false
            return false;
        }
    }
}
