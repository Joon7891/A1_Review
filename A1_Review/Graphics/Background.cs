//Author: Joon Song
//Project Name: A1_Review
//File Name: Background.cs
//Creation Date: 09/11/2018
//Modified Date: 09/11/2018
//Description: Class to hold background object; child to graphic object

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
    public sealed class Background : Graphic
    {
        /// <summary>
        /// Constructor for background object 
        /// </summary>
        /// <param name="image">The image of the background</param>
        public Background(Texture2D image) : base(image)
        {
            //Constructing background rectagle
            Rect = new Rectangle(0, 0, SharedData.SCREEN_WIDTH, SharedData.SCREEN_HEIGHT);
        }
    }
}
