//Author: Joon Song
//Project Name: A1_Review
//File Name: MainMenuScreen.cs
//Creation Date: 09/10/2018
//Modified Date: 09/20/2018
//Description: Class to hold various main menu screen components

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace A1_Review
{
    public static class MainMenuScreen
    {
        //Variables to hold on-screen graphics and buttons
        private static Background background;
        private static Button[] buttons = new Button[3];

        //Song object to hold background music song
        private static Song backgroundMusic;

        //Variables required for drawing screen header
        private static SpriteFont headerFont;
        private static string[] headerTexts = { "GAME OF", "ELEVENS" };
        private static Vector2[] headerLocs = { new Vector2(130, 40), new Vector2(120, 160) };

        /// <summary>
        /// Subprogram to load various main menu components
        /// </summary>
        /// <param name="content">ContentManager required to load content</param>
        public static void Load(ContentManager content)
        {
            //Importing and setting up on-screen graphics and buttons
            background = new Background(content.Load<Texture2D>("Images/Backgrounds/mainMenuBack"));
            for (byte i = 0; i < buttons.Length - 1; i++)
            {
                byte iCache = i;
                buttons[i] = new Button(content.Load<Texture2D>($"Images/Sprites/Buttons/mainMenuButton{i}"), new Rectangle(300, 300 + 65 * i, 200, 60),
                    () =>
                    {
                        Main.CurrentScreen = (ScreenMode)(iCache + 1);
                        MediaPlayer.Stop();
                    });
            }
            buttons[2] = new Button(content.Load<Texture2D>("Images/Sprites/Buttons/mainMenuQuitButton"), new Rectangle(300, 430, 200, 60), () => Main.QuitGame = true);

            //Importing music and fonts
            backgroundMusic = content.Load<Song>("Audio/Music/mainMenuBackMsc");
            headerFont = content.Load<SpriteFont>("Fonts/MainMenuHeadFont");
        }

        /// <summary>
        /// Subprogram to update various main menu components
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        public static void Update(GameTime gameTime)
        {
            //Playing background music if music is not already playing
            if (MediaPlayer.State != MediaState.Playing)
            {
                MediaPlayer.Play(backgroundMusic);
            }

            //Updating on-screen buttons
            for (byte i = 0; i < buttons.Length; i++)
            {
                buttons[i].Update(gameTime);
            }
        }

        /// <summary>
        /// Subprogram to draw various main menu components
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw sprites</param>
        public static void Draw(SpriteBatch spriteBatch)
        {
            //Drawing background
            background.Draw(spriteBatch);

            //Drawing screen header
            for (byte i = 0; i < headerTexts.Length; i++)
            {
                spriteBatch.DrawString(headerFont, headerTexts[i], headerLocs[i], Color.Goldenrod);
            }

            //Drawing on-screen buttons
            for (byte i = 0; i < buttons.Length; i++)
            {
                buttons[i].Draw(spriteBatch);
            }
        }
    }
}
