using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VertShot
{
    public class Sprite
    {
        Texture2D texture;
        Vector2 position;
        Vector2 size;
        public Rectangle rect { get { return new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), Convert.ToInt32(size.X), Convert.ToInt32(size.Y)); } }

        

        public Sprite(Texture2D texture, Vector2 position, Vector2 size)
        {
            this.texture = texture;
            this.size = size;
            this.position = position;
        }

        public void AddPosition(Vector2 position)
        {
            this.position += position;
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rect, Color.White);
        }
    }
}
