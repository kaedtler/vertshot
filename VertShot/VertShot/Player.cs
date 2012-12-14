using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VertShot
{
    public class Player
    {
        const float MaxEnergy = 100f;
        Texture2D texture;
        Vector2 position;
        Vector2 size;
        public Rectangle rect { get { return new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), Convert.ToInt32(size.X), Convert.ToInt32(size.Y)); } }
        Color color;
        public float energy { get; private set; }
        public float collisionDamage { get; private set; }
        float speed;
        double lastShotTime = 0;
        const double shotDelay = 200;

        public Player(Texture2D texture, Color color, Vector2 position)
        {
            this.texture = texture;
            this.color = color;
            this.position = position;
            size = new Vector2(texture.Width, texture.Height);
            speed = 0.65f;
            energy = MaxEnergy;
            collisionDamage = 20f;
        }


        public void AddDamage(float damage, ShotType shotType)
        {
            switch (shotType)
            {
                case ShotType.Laser: energy -= damage; break;
                case ShotType.Explosive: energy -= damage; break;
                case ShotType.Collision: energy -= damage; break;
            }
        }

        public void AddEnergy(float addEnergy)
        {
            energy = Math.Min(energy + addEnergy, MaxEnergy);
        }

        public void Update(GameTime gameTime)
        {
            position += Input.InputVector * new Vector2((float)gameTime.ElapsedGameTime.TotalMilliseconds * speed, (float)gameTime.ElapsedGameTime.TotalMilliseconds * speed);
            position.X = MathHelper.Clamp(position.X, 0, Game1.Width - size.X);
            position.Y = MathHelper.Clamp(position.Y, 0, Game1.Height - size.Y);


            if (Input.IsGameKeyDown(GameKeys.Fire) && gameTime.TotalGameTime.TotalMilliseconds - lastShotTime > shotDelay)
            {
                ShotCollector.AddLaserShot(new Vector2(rect.Center.X, rect.Center.Y), new Vector2(5, 20), new Vector2(0, -1), 0, 1);
                lastShotTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, color);
        }
    }
}
