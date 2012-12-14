using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VertShot
{
    public class Meteor : Enemy
    {
        float rotateSpeed;
        float rotation;
        public Meteor(Texture2D texture, Vector2 position, float speed, Vector2 direction)
            : base(texture)
        {
            this.position = position;
            this.speed = speed;
            this.direction = direction;

            rotateSpeed = ((float)Game1.rand.Next(90, 131) / 1000 * (Game1.rand.Next(0,2) == 1 ? -1 : 1));
            energy = 10;
            collisionDamage = 10f;
        }

        public override void AddDamage(float damage, ShotType shotType)
        {
            switch (shotType)
            {
                case ShotType.Laser: energy -= damage; break;
                case ShotType.Explosive: energy -= damage * 1.25f; break;
                case ShotType.Collision: energy -= damage; break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            position += direction * (float)gameTime.ElapsedGameTime.TotalMilliseconds * speed;

            rotation += (float)gameTime.ElapsedGameTime.TotalMilliseconds * rotateSpeed;
            if (rotation >= 360) rotation -= 360; else if (rotation < 0) rotation += 360;

            if (energy <= 0)
            {
                EffectCollector.AddExplosion1(new Vector2(rect.Center.X, rect.Center.Y), true, speed, direction);
                IsAlive = false;
            }
            if(!Game1.GameRect.Intersects(rect))
            {
                IsAlive = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position + size / 2, null, Color.White, (float)(rotation * Math.PI / 180), size / 2, 1, SpriteEffects.None, 0);
        }
    }
}
