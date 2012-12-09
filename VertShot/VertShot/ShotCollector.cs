using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VertShot
{
    static public class ShotCollector
    {
        static Texture2D texture;
        static List<Shot> shotList = new List<Shot>();

        static public void Initialize(Texture2D texture)
        {
            ShotCollector.texture = texture;
        }

        static public void AddShot(Vector2 position, Vector2 size, Vector2 direction, float angle, float speed, int damage = 0, bool playerIsTarget = false)
        {
            Shot shot = new Shot(texture, position, size, direction, angle, speed, 0, false);
            shotList.Add(shot);
        }


        static public void Update(GameTime gameTime)
        {
            for (int i = shotList.Count - 1; i >= 0; i--)
            {
                shotList[i].Update(gameTime);
                if (!shotList[i].active)
                    shotList.RemoveAt(i);
            }
        }

        static public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Shot shot in shotList)
                shot.Draw(spriteBatch);
        }
    }
}
