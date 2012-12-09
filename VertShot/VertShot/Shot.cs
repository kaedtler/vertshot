using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VertShot
{
    public class Shot
    {
        Texture2D texture;
        Vector2 position;
        Vector2 direction;
        Vector2 size;
        float angle;
        int damage;
        float speed;
        public bool active;

        public Shot(Texture2D texture, Vector2 position, Vector2 size, Vector2 direction, float angle, float speed, int damage, bool playerIsTarget)
        {
            this.texture = texture;
            this.position = position;
            this.size = size;
            this.direction = direction;
            this.angle = angle;
            this.speed = speed;
            this.damage = damage;
            active = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), null, Color.White, (float)(angle * Math.PI/180), Vector2.Zero, SpriteEffects.None, 0);
        }


        internal void Update(GameTime gameTime)
        {
            position += direction * (float)gameTime.ElapsedGameTime.TotalMilliseconds * speed;
            if (position.Y < 0)
                active = false;
        }
    }
}
