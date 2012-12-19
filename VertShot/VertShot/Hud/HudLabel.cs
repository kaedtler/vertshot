using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VertShot
{
    public class HudLabel
    {
        Vector2 position;
        String text;

        public HudLabel(Vector2 position, String text)
        {
            this.position = position;
            this.text = text;
        }

        public void Update(GameTime gameTime)
        {
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game1.buttonFont, text, position, new Color(20, 120, 0, 160));
        }
    }
}
