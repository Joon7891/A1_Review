//Author: Joon Song
//Project Name: A1_Review
//File Name: Main.cs
//Creation Date: 09/10/2018
//Modified Date: 09/20/2018
//Description: Program to emulate clasic "Game of Elevens" card game

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace A1_Review
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        //Instances of graphics device manager and sprite batch to allow for graphics drawing
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        /// <summary>
        /// The current screen that the game is in
        /// </summary>
        public static ScreenMode CurrentScreen { get; set; }

        //Dictionary to map screen mode to screen method pair
        private static Dictionary<ScreenMode, ScreenMethodPair> screenMethodDictionary = new Dictionary<ScreenMode, ScreenMethodPair>();

        /// <summary>
        /// The mouse state of the mouse 1 frame back
        /// </summary>
        public static MouseState OldMouse { get; private set; }

        /// <summary>
        /// The mouse state of the mouse currently
        /// </summary>
        public static MouseState NewMouse { get; private set; }

        /// <summary>
        /// The keyboard state of the keyboard 1 frame back
        /// </summary>
        public static KeyboardState OldKeyboard { get; private set; }

        /// <summary>
        /// The keyboard state of the keyboard currently
        /// </summary>
        public static KeyboardState NewKeyboard { get; private set; }

        /// <summary>
        /// Variable to hold if game should be quit; used so other classes can exit game
        /// </summary>
        public static bool QuitGame { private get; set; }

        /// <summary>
        /// Vector to hold current location of mouse
        /// </summary>
        public static Vector2 mouseLoc;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //Setting up game screen dimensions and making mouse visible
            graphics.PreferredBackBufferHeight = SharedData.SCREEN_HEIGHT;
            graphics.PreferredBackBufferWidth = SharedData.SCREEN_WIDTH;
            graphics.ApplyChanges();
            IsMouseVisible = true;

            //Initializing game
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Setting default screen as main menu
            CurrentScreen = ScreenMode.MainMenu;

            //Loading various static classes
            CardHelper.Load(Content);
            SharedData.Load(Content);
            IO.Load();
            MainMenuScreen.Load(Content);
            GameScreen.Load(Content);
            LeaderboardScreen.Load(Content);

            //Setting up screen method dictionary
            screenMethodDictionary.Add(ScreenMode.MainMenu, new ScreenMethodPair(MainMenuScreen.Update, MainMenuScreen.Draw));
            screenMethodDictionary.Add(ScreenMode.Game, new ScreenMethodPair(GameScreen.Update, GameScreen.Draw));
            screenMethodDictionary.Add(ScreenMode.Leaderboard, new ScreenMethodPair(LeaderboardScreen.Update, LeaderboardScreen.Draw));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //Exit game of escape key is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) || QuitGame)
            {
                Exit();
            }

            //Updating old and new mouse as well as its location
            OldMouse = NewMouse;
            NewMouse = Mouse.GetState();
            mouseLoc.X = NewMouse.X;
            mouseLoc.Y = NewMouse.Y;

            //Updating old and new keyboard
            OldKeyboard = NewKeyboard;
            NewKeyboard = Keyboard.GetState();

            //Calling appropriate update subprogram given current screen
            screenMethodDictionary[CurrentScreen].Update(gameTime);

            //Updating game
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //Beginning spriteBatch
            spriteBatch.Begin();

            //Calling appropriate draw subprogram given current screen
            screenMethodDictionary[CurrentScreen].Draw(spriteBatch);

            //Ending spriteBatch
            spriteBatch.End();

            //Drawing game
            base.Draw(gameTime);
        }
    }
}
