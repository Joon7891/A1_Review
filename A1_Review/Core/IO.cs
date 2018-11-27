//Author: Joon Song
//Project File: A1_Review
//File Name: IO.cs
//Creation Date: 09/19/2018
//Modified Date: 09/20/2018
//Description: Static class to hold various Input/Output processes

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace A1_Review
{
    public static class IO
    {
        //Strings to hold various IO file paths
        private static string filePath;
        private static string leaderboardDataPath;
        private static string gameDataPath;

        //Temp variables to hold lines of file data
        private static string dataLine;
        private static string[] dataLineParts;

        /// <summary>
        /// Variable to hold if game data exists
        /// </summary>
        public static bool GameDataExists { get; private set; }

        /// <summary>
        /// The card deck of the loaded game
        /// </summary>
        public static List<Card> LoadedCardDeck { get; private set; }

        /// <summary>
        /// The card piles of the loaded game
        /// </summary>
        public static Stack<Card>[,] LoadedCardPiles { get; private set; }

        /// <summary>
        /// The leaderboard of the loaded game
        /// </summary>
        public static LeaderboardEntry[] LoadedLeaderboard { get; private set; }

        /// <summary>
        /// The loaded play time; how long the game has been played for
        /// </summary>
        public static double LoadedPlayTime { get; private set; }

        //Instance of stream reader and writer to read and write to files
        private static StreamReader inFile;
        private static StreamWriter outFile;

        /// <summary>
        /// Subprogram to load in various components of File Input/Output
        /// </summary>
        public static void Load()
        {
            //Setting up file paths
            filePath = Assembly.GetExecutingAssembly().CodeBase;
            filePath = Path.GetDirectoryName(filePath);
            filePath = filePath.Substring(6) + @"\IO";
            gameDataPath = filePath + @"\GameData.txt";
            leaderboardDataPath = filePath + @"\LeaderboardData.txt";

            //Calling subprograms to load game and leaderboard data
            LoadGameData();
            LoadLeaderboardData();
        }

        /// <summary>
        /// Subprogram to load in game data
        /// </summary>
        private static void LoadGameData()
        {
            //Temp various to help with file reading
            byte numDeckCards;
            byte cardPileSize;
            byte cardSuitByte;
            byte cardRankByte;
            
            //Try-Catch block to handle file reading exceptions
            try
            {
                //Opening file
                inFile = File.OpenText(gameDataPath);

                //Only loading in game data if it exists
                GameDataExists = inFile.ReadLine() == "DataExists";
                if (GameDataExists)
                {
                    //Calling subprogram to set up cards deck and piles
                    SetUpCardData();

                    //Getting game played time
                    LoadedPlayTime = Convert.ToDouble(inFile.ReadLine());

                    //Getting number of cards in deck from file
                    dataLine = inFile.ReadLine();
                    dataLineParts = dataLine.Split();
                    numDeckCards = Convert.ToByte(dataLineParts[1]);

                    //Setting up card deck from file
                    for (byte i = 0; i < numDeckCards; i++)
                    {
                        dataLine = inFile.ReadLine();
                        dataLineParts = dataLine.Split();
                        cardSuitByte = Convert.ToByte(dataLineParts[0]);
                        cardRankByte = Convert.ToByte(dataLineParts[1]);

                        LoadedCardDeck.Add(new Card((SuitType)cardSuitByte, (RankType)cardRankByte, CardHelper.CardDeckRect));
                    }

                    //Setting up card piles from file
                    dataLine = inFile.ReadLine();
                    for (byte row = 0; row < LoadedCardPiles.GetLength(0); row++)
                    {
                        for (byte column = 0; column < LoadedCardPiles.GetLength(1); column++)
                        {
                            dataLine = inFile.ReadLine();
                            dataLineParts = dataLine.Split();
                            cardPileSize = Convert.ToByte(dataLineParts[0]);
                            cardSuitByte = Convert.ToByte(dataLineParts[1]);
                            cardRankByte = Convert.ToByte(dataLineParts[2]);

                            for (byte i = 0; i < cardPileSize - 1; i++)
                            {
                                LoadedCardPiles[row, column].Push(Card.GetEmptyCard(row, column));
                            }
                            LoadedCardPiles[row, column].Push(new Card((SuitType)cardSuitByte, (RankType)cardRankByte, CardHelper.CardPileRects[row, column]));
                            LoadedCardPiles[row, column].Peek().IsFaceUp = true;
                        }
                    }
                }

                //Closing file
                inFile.Close();
            }
            catch (FileNotFoundException)
            {
                //Informing user that file was not found
                Console.WriteLine("Exception: File was not found");
            }
            catch (IndexOutOfRangeException)
            {
                //Informing user that index was out of range
                Console.WriteLine("Exception: Index was out of range");
            }
            catch (FormatException)
            {
                //Informing user that their data was read in via wrong format
                Console.WriteLine("Exception: Data was read in wrong format");
            }
            catch (EndOfStreamException)
            {
                //Informing user that file was attempted to be read past end of stream
                Console.WriteLine("Exception: Attempted to read past end of file");
            }
        }

        /// <summary>
        /// Subprogram to set up card data
        /// </summary>
        private static void SetUpCardData()
        {
            //Constructing list for card deck
            LoadedCardDeck = new List<Card>();

            //Constructing card piles
            LoadedCardPiles = new Stack<Card>[2, 6];
            for (byte row = 0; row < LoadedCardPiles.GetLength(0); row++)
            {
                for (byte column = 0; column < LoadedCardPiles.GetLength(1); column++)
                {
                    LoadedCardPiles[row, column] = new Stack<Card>();
                }
            }
        }

        /// <summary>
        /// Subprogram to load in leaderboard data
        /// </summary>
        private static void LoadLeaderboardData()
        {
            //Try-Catch block to handle file reading exceptions
            try
            {
                //Opening file
                inFile = File.OpenText(leaderboardDataPath);

                //Loading in leaderboard data
                LoadedLeaderboard = new LeaderboardEntry[5];
                for (int i = 0; i < LoadedLeaderboard.Length; i++)
                {
                    dataLine = inFile.ReadLine();
                    dataLineParts = dataLine.Split();
                    LoadedLeaderboard[i] = new LeaderboardEntry(dataLineParts[0], Convert.ToByte(dataLineParts[1]), Convert.ToDouble(dataLineParts[2]));
                }

                //Closing file
                inFile.Close();
            }
            catch (FileNotFoundException)
            {
                //Informing user that file was not found
                Console.WriteLine("Exception: File was not found");
            }
            catch (IndexOutOfRangeException)
            {
                //Informing user that index was out of range
                Console.WriteLine("Exception: Index was out of range");
            }
            catch (FormatException)
            {
                //Informing user that their data was read in via wrong format
                Console.WriteLine("Exception: Data was read in wrong format");
            }
            catch (EndOfStreamException)
            {
                //Informing user that file was attempted to be read past end of stream
                Console.WriteLine("Exception: Attempted to read past end of file");
            }
        }

        /// <summary>
        /// Subprogram to upload leaderboard data
        /// </summary>
        /// <param name="playTime">The time the game was played for</param>
        /// <param name="cardDeck">The card deck</param>
        /// <param name="cardPiles">The card piles</param>
        public static void UploadGameData(double playTime, List<Card> cardDeck, Stack<Card>[,] cardPiles)
        {
            //Creating file
            outFile = File.CreateText(gameDataPath);

            //Only uploading file if game has been played
            if (cardDeck.Count < 40)
            {
                //Indicating that a played game exists
                outFile.WriteLine("DataExists");

                //Writing play time and number of cards in deck
                outFile.WriteLine(playTime);
                outFile.WriteLine($"CardsInDeck {cardDeck.Count}");
                
                //Uploading the cards in the deck
                for (byte i = 0; i < cardDeck.Count; i++)
                {
                    outFile.WriteLine($"{(int)cardDeck[i].Suit} {(int)cardDeck[i].Rank}");
                }

                //Uploading card deck data
                outFile.WriteLine("--Card Piles Start Here--");
                for (byte row = 0; row < cardPiles.GetLength(0); row++)
                {
                    for (byte column = 0; column < cardPiles.GetLength(1); column++)
                    {
                        outFile.WriteLine($"{cardPiles[row, column].Count} {(int)cardPiles[row, column].Peek().Suit} {(int)cardPiles[row, column].Peek().Rank}");
                    }
                }
            }
            else
            {
                //Indicating that game has not been played
                outFile.WriteLine("DataDoesNotExist");
            }

            //Closing file
            outFile.Close();
        }

        /// <summary>
        /// Subprogram to upload blank game data; used when resetting game
        /// </summary>
        public static void UploadBlankGameData()
        {
            //Creating file
            outFile = File.CreateText(gameDataPath);

            //Indicating that game data does not exist
            outFile.WriteLine("DataDoesNotExist");

            //Closing file
            outFile.Close();
        }

        /// <summary>
        /// Subprogram to upload leaderboard data
        /// </summary>
        /// <param name="leaderboard">The leaderboard of which to upload data from</param>
        public static void UploadLeaderboardData(LeaderboardEntry[] leaderboard)
        {
            //Creating file
            outFile = File.CreateText(leaderboardDataPath);

            //Uploading data to file, one entry at a time
            for (byte i = 0; i < leaderboard.Length; i++)
            {
                outFile.WriteLine($"{leaderboard[i].Name} {leaderboard[i].Score} {leaderboard[i].Time}");
            }

            //Closing file
            outFile.Close();
        }
    }
}
