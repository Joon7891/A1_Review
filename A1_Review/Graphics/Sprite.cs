//Author: Joon Song
//Project Name: A1_Review
//File Name: Sprite.cs
//Creation Date: 09/11/2018
//Modified Date: 09/11/2018
//Description: Class to hold sprite object; child of graphic object

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
    public sealed class Sprite : Graphic
    {
        /// <summary>
        /// Constructor for sprite object 
        /// </summary>
        /// <param name="image">The image of the sprite</param>
        /// <param name="rect">The rectangle representing the rectangular dimensions of the sprite</param>
        public Sprite(Texture2D image, Rectangle rect) : base(image)
        {
            //Setting rectangle parameter to object property
            Rect = rect;
        }
    }
}
