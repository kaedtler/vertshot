using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VertShot
{
    public class AnimatedSprite
    {
        Texture2D texture;
        Vector2 position;
        Point spriteSize;
        int frameTime;
        float time;
        int animationMaxSteps;
        int animationStep = 0;
        bool repeat;
        Point spriteRowsColumns;
        float rotation;
        float scale;
        float speed;
        Vector2 direction;
        public bool IsAlive = true;
        public Rectangle rect { get { return new Rectangle(Convert.ToInt32(position.X - spriteSize.X), Convert.ToInt32(position.Y - spriteSize.Y), Convert.ToInt32(spriteSize.X), Convert.ToInt32(spriteSize.Y)); } }

        public AnimatedSprite(Texture2D texture, Vector2 position, Point spriteSize, int frameTime, int animationMaxSteps,
            bool repeat, float rotation, float scale, bool centerPos, float speed, Vector2 direction)
        {
            this.texture = texture;
            if(centerPos)
                this.position = position + new Vector2(spriteSize.X / 2, spriteSize.Y / 2);
            else
                this.position = position;
            this.spriteSize = spriteSize;
            this.frameTime = frameTime;
            this.repeat = repeat;
            this.rotation = rotation;
            this.scale = scale;
            this.speed = speed;
            this.direction = direction;
            spriteRowsColumns = new Point(texture.Width / spriteSize.X, texture.Height / spriteSize.Y);
            if (animationMaxSteps <= 0)
                this.animationMaxSteps = spriteRowsColumns.X * spriteRowsColumns.Y;
            else
                this.animationMaxSteps = animationMaxSteps;
        }

        public void Update(GameTime gameTime)
        {
            if (IsAlive)
            {
                time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (time >= frameTime)
                {
                    time -= frameTime;
                    animationStep++;
                }
                if (animationStep > animationMaxSteps)
                    if (repeat)
                        animationStep = 0;
                    else
                        IsAlive = false;
                if (speed != 0)
                    position += direction * speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsAlive)
                spriteBatch.Draw(texture,
                    position - new Vector2(spriteSize.X / 2, spriteSize.Y / 2),
                    new Rectangle(animationStep % spriteRowsColumns.X * spriteSize.X, animationStep / spriteRowsColumns.X * spriteSize.Y, spriteSize.X, spriteSize.Y),
                    Color.White,
                    (float)(rotation * Math.PI / 180),
                    new Vector2(spriteSize.X / 2, spriteSize.Y / 2),
                    scale,
                    SpriteEffects.None,
                    0);
        }
    }
}
