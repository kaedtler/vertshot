using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VertShot
{
    public class Enemy
    {
        public bool IsAlive = true;
        protected Texture2D texture;
        protected Vector2 position;
        protected Vector2 size;
        public float speed { get; protected set; }
        public Vector2 direction { get; protected set; }
        public float collisionDamage { get; protected set; }
        public Rectangle rect { get { return new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), Convert.ToInt32(size.X), Convert.ToInt32(size.Y)); } }
        public float energy { get; protected set; }


        public Enemy(Texture2D texture)
        {
            this.texture = texture;
            size = new Vector2(this.texture.Width, this.texture.Height);
            position = Vector2.Zero;
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void AddDamage(float damage, ShotType shotType)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rect, Color.White);
        }
    }
}
