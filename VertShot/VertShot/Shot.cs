using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VertShot
{
    public enum ShotType
    {
        None,
        Laser,
        Explosive
    }

    public class Shot
    {
        protected Texture2D texture;
        protected Vector2 position;
        protected Vector2 size;
        public Rectangle rect { get { return new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), Convert.ToInt32(size.X), Convert.ToInt32(size.Y)); } }
        protected Vector2 direction;
        protected float speed;
        public ShotType shotType { get; private set; }
        public bool fromPlayer { get; protected set; }
        public float damage { get; protected set; }
        public bool singleHit { get; protected set; }
        public bool IsAlive = true;

        public Shot(Texture2D texture, bool singleHit = true, ShotType shotType = ShotType.None)
        {
            this.texture = texture;
            this.singleHit = singleHit;
            this.shotType = shotType;
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rect, Color.White);
        }

    }
}
