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
        Texture2D texture;
        Vector2 position;
        Rectangle rect;
        Color color;
        int energy;
        float speed;
        double lastShotTime = 0;
        const double shotDelay = 200;

        public Player(Texture2D texture, Color color, Vector2 position)
        {
            this.texture = texture;
            this.color = color;
            this.position = position;
            rect = new Rectangle((int)position.X, (int)position.Y, (int)texture.Width, (int)texture.Height);
            speed = 1f;
            energy = 100;
        }

        public void Update(GameTime gameTime)
        {
            position += Input.InputVector * new Vector2((float)gameTime.ElapsedGameTime.TotalMilliseconds * speed, (float)gameTime.ElapsedGameTime.TotalMilliseconds * speed);
            rect.X = (int)position.X;
            rect.Y = (int)position.Y;

            if (Input.keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space) && gameTime.TotalGameTime.TotalMilliseconds - lastShotTime > shotDelay)
            {
                ShotCollector.AddShot(new Vector2(rect.Center.X, rect.Center.Y), new Vector2(5, 20), new Vector2(0, -1), 0, 1);
                lastShotTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, color);
        }
    }
}
