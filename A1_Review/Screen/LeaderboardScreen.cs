//Author: Joon Song
//Project Name: A1_Review
//File Name: LeaderboardScreen.cs
//Creation Date: 09/10/2018
//Modified Date: 09/20/2018
//Description: Class to hold various leaderboard screen components

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace A1_Review
{
    public static class LeaderboardScreen
    {
        //Object to hold on-screen background image
        public static Background background;

        //Song object to hold background music song
        private static Song backgroundMusic;

        //Array of leaderboard entries act as a leaderboard
        private static LeaderboardEntry[] leaderboard = new LeaderboardEntry[5];

        //SpriteFont variables to hold various fonts used in game
        private static SpriteFont headerFont;
        private static SpriteFont infoFont;

        //Variables to hold necessary data to draw leaderboard text
        private static Vector2 headerLoc = new Vector2(151, 25);
        private static string[] subHeaderTxts = { "Rank", "Name", "Score", "Time" };
        private static Vector2[] subHeaderLocs = { new Vector2(41, 110), new Vector2(237, 110), new Vector2(426, 110), new Vector2(646, 110) };
        private static Vector2[,] infoLocs = new Vector2[5, 4]; 
        private static Color[] dataColors = { Color.Gold, Color.Silver, Color.RosyBrown, Color.Black, Color.Black };

        /// <summary>
        /// Subprogram to load various leaderboard screen components
        /// </summary>
        /// <param name="content">ContentManager required to load content</param>
        public static void Load(ContentManager content)
        {
            //Setting up background
            background = new Background(content.Load<Texture2D>("Images/Backgrounds/LeaderboardBackground"));

            //Importing music and fonts
            backgroundMusic = content.Load<Song>("Audio/Music/leaderboardBackMsc");
            headerFont = content.Load<SpriteFont>("Fonts/LeaderboardHeaderFont");
            infoFont = content.Load<SpriteFont>("Fonts/LeaderboardInfoFont");

            //Setting up various leaderboard data
            leaderboard = IO.LoadedLeaderboard;
            for (byte i = 0; i < leaderboard.Length; i++)
            {
                infoLocs[i, 0] = new Vector2(100 - infoFont.MeasureString($"{i + 1}").X / 2, 165 + 50 * i);
                infoLocs[i, 1] = new Vector2(300 - infoFont.MeasureString(leaderboard[i].Name).X / 2, 165 + 50 * i);
                infoLocs[i, 2] = new Vector2(500 - infoFont.MeasureString($"{leaderboard[i].Score}").X / 2, 165 + 50 * i);
                infoLocs[i, 3] = new Vector2(700 - infoFont.MeasureString($"{leaderboard[i].Time}").X / 2, 165 + 50 * i);
            }
        }

        /// <summary>
        /// Subprogram to update various leaderboard screen components
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        public static void Update(GameTime gameTime)
        {
            //Playing background music if music is not already playing
            if (MediaPlayer.State != MediaState.Playing)
            {
                MediaPlayer.Play(backgroundMusic);
            }

            //Updating back-button
            SharedData.BackButton.Update(gameTime);
        }

        /// <summary>
        /// Subprogram to update leaderboard data
        /// </summary>
        /// <param name="newEntry">The entry to be added</param>
        public static void UpdateLeaderboardData(LeaderboardEntry newEntry)
        {
            //Determining position of entry and making adjustments as needed
            for (byte i = 0; i < leaderboard.Length; i++)
            {
                if (newEntry.Score > leaderboard[i].Score || (newEntry.Score == leaderboard[i].Score && newEntry.Time <= leaderboard[i].Time))
                {
                    for (byte adjustNo = (byte)(leaderboard.Length - 1); adjustNo > i; adjustNo--)
                    {
                        leaderboard[adjustNo] = leaderboard[adjustNo - 1];
                        infoLocs[adjustNo, 1].X = infoLocs[adjustNo - 1, 1].X;
                        infoLocs[adjustNo, 2].X = infoLocs[adjustNo - 1, 2].X;
                        infoLocs[adjustNo, 3].X = infoLocs[adjustNo - 1, 3].X;
                    }

                    //Adding entry to leaderboard
                    leaderboard[i] = newEntry;
                    infoLocs[i, 1] = new Vector2(300 - infoFont.MeasureString(leaderboard[i].Name).X / 2, 165 + 50 * i);
                    infoLocs[i, 2] = new Vector2(500 - infoFont.MeasureString($"{leaderboard[i].Score}").X / 2, 165 + 50 * i);
                    infoLocs[i, 3] = new Vector2(700 - infoFont.MeasureString($"{leaderboard[i].Time}").X / 2, 165 + 50 * i);

                    //Breaking out of check loop
                    break;
                }
            }

            //Uploading data to file
            IO.UploadLeaderboardData(leaderboard);
        }

        /// <summary>
        /// Subprogram to draw various leaderboard screen components
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw sprites</param>
        public static void Draw(SpriteBatch spriteBatch)
        {
            //Drawing background
            background.Draw(spriteBatch);

            //Drawing leaderboard header and subheaders
            spriteBatch.DrawString(headerFont, "Leaderboard", headerLoc, Color.Red);
            for (byte i = 0; i < subHeaderTxts.Length; i++)
            {
                spriteBatch.DrawString(infoFont, subHeaderTxts[i], subHeaderLocs[i], Color.Blue);
            }

            //Drawing leaderboard data
            for (byte i = 0; i < leaderboard.Length; i++)
            {
                spriteBatch.DrawString(infoFont, $"{i + 1}", infoLocs[i, 0], dataColors[i]);
                spriteBatch.DrawString(infoFont, leaderboard[i].Name, infoLocs[i, 1], dataColors[i]);
                spriteBatch.DrawString(infoFont, $"{leaderboard[i].Score}", infoLocs[i, 2], dataColors[i]);
                spriteBatch.DrawString(infoFont, $"{leaderboard[i].Time}", infoLocs[i, 3], dataColors[i]);
            }

            //Drawing back-button
            SharedData.BackButton.Draw(spriteBatch);
        }
    }
}
