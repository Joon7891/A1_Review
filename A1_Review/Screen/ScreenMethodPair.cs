//Author: Joon Song
//Project Name: A1_Review
//File Name: ScreenMethodPair.cs
//Creation Date: 09/10/2018
//Modified Date: 09/18/2018
//Desription: Class to hold ScreenMethodPair, a struct which contains Update and Draw subprogram pairs for a given screen

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
    public struct ScreenMethodPair
    {
        //Delegates for update and draw subprograms
        public delegate void UpdateSubprogram(GameTime gameTime);
        public delegate void DrawSubprogram(SpriteBatch spriteBatch);

        /// <summary>
        /// Update subprogram for screen
        /// </summary>
        public UpdateSubprogram Update;

        /// <summary>
        /// Draw subprogram for screen
        /// </summary>
        public DrawSubprogram Draw;

        /// <summary>
        /// Constructor for ScreenMethodPair struct
        /// </summary>
        /// <param name="update">The update subprogram</param>
        /// <param name="draw">The draw subprogram</param>
        public ScreenMethodPair(UpdateSubprogram update, DrawSubprogram draw)
        {
            //Setting parameter delegates to object properties
            Update = update;
            Draw = draw;
        }
    }
}
