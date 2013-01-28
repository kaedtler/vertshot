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
        String textOrg;
        String text;

        public Vector2 GetPosition { get { return position; } }

        public HudLabel(Vector2 position, String text)
        {
            this.position = position;
            this.textOrg = text;
        }

        public void Update(GameTime gameTime)
        {
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            text = textOrg.Replace("[SCORE]", Game1.enemyCounter.ToString());
            spriteBatch.DrawString(Game1.buttonFont, text, position, new Color(20, 120, 0, 160));
        }

        internal void RefreshPosition(Point position)
        {
            throw new NotImplementedException();
        }

        internal void RefreshPosition(Vector2 position)
        {
            this.position = position;
        }
    }
}
