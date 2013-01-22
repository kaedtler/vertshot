using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VertShot.Menu
{
    public class Label : MenuObject
    {
        public Label(Vector2 position, String text, bool replaceText = false)
        {
            this.position = position;
            this.text = text;
            this.replaceText = replaceText;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game1.buttonFont, GetText, position, new Color(20, 120, 0, 160));
        }
    }
}
