using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VertShot.Menu
{
    public class Player : MenuObject
    {
        public Player(Vector2 position)
        {
            this.position = position;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Game1.player.Draw(spriteBatch, position);
        }
    }
}
