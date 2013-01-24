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
        static Texture2D laserTex;
        static List<Shot> shotList = new List<Shot>();

        static public List<Shot> GetList { get { return shotList; } }

        static public void Initialize(Texture2D texture)
        {
            ShotCollector.laserTex = texture;
        }

        static public void AddLaserShot(Vector2 position, Vector2 size, Vector2 direction, float angle, float speed, int damage = 5, bool fromPlayer = true)
        {
            Shot shot = new LaserShot(laserTex, position, size, direction, angle, speed, damage, fromPlayer);
            shotList.Add(shot);
            Sound.PlaySound(Sound.Sounds.Laser, position);
        }


        static public void Update(GameTime gameTime)
        {
            for (int i = shotList.Count - 1; i >= 0; i--)
            {

                foreach (Enemy enemy in EnemyCollector.GetList)
                    if (shotList[i].fromPlayer && shotList[i].rect.Intersects(enemy.rect))
                    {
                        EffectCollector.AddExplosion2(new Vector2(shotList[i].rect.X - shotList[i].rect.Width / 2, shotList[i].rect.Y), true, enemy.speed, enemy.direction);
                        enemy.AddDamage(shotList[i].damage, shotList[i].shotType);
                        if (shotList[i].singleHit)
                            shotList[i].IsAlive = false;
                        if (enemy.energy <= 0)
                        {
                            Game1.enemyCounter++;
                            if (Game1.rand.Next(0, 100) < 3)
                                Items.AddItem(Items.ItemTypes.Health25, new Vector2(shotList[i].rect.X, shotList[i].rect.Y));
                        }
                    }

                shotList[i].Update(gameTime);

                if (!shotList[i].IsAlive)
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
