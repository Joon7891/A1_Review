//Author: Joon Song
//Project Name: A1_Review
//File Name: SharedData.cs
//Creation Date: 09/11/2018
//Modified Date: 09/11/2018
//Description: Class to hold a variety of shared data

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
    public static class SharedData
    {
        /// <summary>
        /// The height of the screen
        /// </summary>
        public const int SCREEN_HEIGHT = 600;

        /// <summary>
        /// The width of the screen
        /// </summary>
        public const int SCREEN_WIDTH = 800;

        /// <summary>
        /// Back-button
        /// </summary>
        public static Button BackButton { get; private set; }

        /// <summary>
        /// Subprogram to load various shared data
        /// </summary>
        /// <param name="content">ContentManager to help load content</param>
        public static void Load(ContentManager content)
        {
            //Loading and setting up back-button
            BackButton = new Button(content.Load<Texture2D>("Images/Sprites/Buttons/backButton"), new Rectangle(20, 20, 50, 50), 
                () => 
                {
                    Main.CurrentScreen = ScreenMode.MainMenu;
                    MediaPlayer.Stop();
                });
        }
    }
}