using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VertShot
{
    class LaserShot : Shot
    {
        //protected Texture2D texture;
        //protected Vector2 position;
        //protected Vector2 size;
        //protected Rectangle rect { get { return new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), Convert.ToInt32(size.X), Convert.ToInt32(size.Y)); } }
        //protected Vector2 direction;
        //protected float speed;
        //protected ShotType shotType = ShotType.None;
        //protected float damage;
        //public bool IsAlive = true;
        float angle;

        public LaserShot(Texture2D texture, Vector2 position, Vector2 size, Vector2 direction, float angle, float speed, int damage, bool fromPlayer)
            : base(texture, true, ShotType.Laser)
        {
            this.position = position;
            this.size = size;
            this.direction = direction;
            this.angle = angle;
            this.speed = speed;
            this.damage = damage;
            this.fromPlayer = fromPlayer;
            IsAlive = true;
        }

        public override void Update(GameTime gameTime)
        {
            position += direction * (float)gameTime.ElapsedGameTime.TotalMilliseconds * speed;
            if (!Game1.GameRect.Intersects(rect))
                IsAlive = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rect, null, Color.White, (float)(angle * Math.PI/180), Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
