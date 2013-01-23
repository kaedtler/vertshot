using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VertShot
{
    static public class DynamicBackground
    {
        class ObjectStr
        {
            public List<Sprite> list;
            public float timer;

            public Texture2D texture;
            public Vector2 size;
            public float speed;
            public int minTime;
            public int maxTime;
            public ObjectStr(Texture2D texture, Vector2 size, float speed, int minTime, int maxTime)
            {
                this.texture = texture;
                this.size = size;
                this.speed = speed;
                this.minTime = minTime;
                this.maxTime = maxTime;
                list = new List<Sprite>();
                timer = 0;
            }
        }

        static List<ObjectStr> objectList = new List<ObjectStr>();

        static public void AddObjects(Texture2D texture, float speed, int minTime, int maxTime)
        {
            AddObjects(texture, new Vector2(texture.Width, texture.Height), speed, minTime, maxTime);
        }
        static public void AddObjects(Texture2D texture, Vector2 size, float speed, int minTime, int maxTime)
        {
            objectList.Add(new ObjectStr(texture, size, speed, minTime, maxTime));
            for (int i = Game1.rand.Next(minTime, maxTime); i < 1 / speed * Game1.Height; )
            {
                objectList.Last().list.Add(new Sprite(texture, new Vector2(Game1.rand.Next(0, Game1.Width - (int)size.X), Game1.rand.Next(0, Game1.Height - (int)size.Y)), size));
                i += Game1.rand.Next(minTime, maxTime);
            }
            objectList.Last().timer = Game1.rand.Next(minTime, maxTime);
        }

        static public void Update(GameTime gameTime)
        {
            foreach (ObjectStr obj in objectList)
            {
                obj.timer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (obj.timer <= 0)
                {
                    obj.list.Add(new Sprite(obj.texture, new Vector2(Game1.rand.Next(0, Game1.Width - (int)obj.size.X), -obj.size.Y + 1), obj.size));
                    obj.timer = Game1.rand.Next(obj.minTime, obj.maxTime);
                }
                for (int i = obj.list.Count - 1; i >= 0; i--)
                {
                    obj.list[i].AddPosition(new Vector2(0, 1) * obj.speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
                    if(!Game1.GameRect.Intersects(obj.list[i].rect))
                        obj.list.RemoveAt(i);
                }

            }
        }

        static public void Draw(SpriteBatch spriteBatch)
        {
            foreach (ObjectStr obj in objectList)
                foreach (Sprite sprite in obj.list)
                    sprite.Draw(spriteBatch);
        }
    }
}
