//Author: Joon Song
//Project Name: A1_Review
//File Name: Graphic.cs
//Creation Date: 09/11/2018
//Modified Date: 09/11/2018
//Description: Class to hold graphic object; parent to sprite and background objects

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
    public abstract class Graphic
    {
        /// <summary>
        /// The image of the graphic
        /// </summary>
        public Texture2D Image { get; set; }

        /// <summary>
        /// The rectangle representing the rectangular dimensions of the graphic
        /// </summary>
        public Rectangle Rect { get; set; }

        /// <summary>
        /// Constructor for parent graphic object
        /// </summary>
        /// <param name="image">The image of the graphic</param>
        protected Graphic(Texture2D image)
        {
            //Setting image parameter to object property
            Image = image;
        }

        /// <summary>
        /// Draw subprogram for graphic object
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw graphic</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            //Drawing graphic
            spriteBatch.Draw(Image, Rect, Color.White);
        }
    }
}