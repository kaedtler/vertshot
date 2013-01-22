using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VertShot
{
    static public class EffectCollector
    {
        static List<AnimatedSprite> effectList = new List<AnimatedSprite>();
        static Texture2D explosionTex1;

        static public List<AnimatedSprite> GetList { get { return effectList; } }

        static public void Initialize(Texture2D explosionTex1)
        {
            EffectCollector.explosionTex1 = explosionTex1;
        }

        static public void AddExplosion1(Vector2 position, bool centerPos, float speed, Vector2 direction)
        {
            AnimatedSprite animatedSprite = new AnimatedSprite(explosionTex1, position, new Point(69, 64), 50, 0, false, Game1.rand.Next(0, 360), 1.7f, true, speed, direction);
            effectList.Add(animatedSprite);
            Sound.PlaySound(Sound.Sounds.BigExplosion, position + new Vector2(34.5f, 32));
            Input.Vibrate(0.3f, 0.15f, 0.1f);
        }

        static public void AddExplosion2(Vector2 position, bool centerPos, float speed, Vector2 direction)
        {
            AnimatedSprite animatedSprite = new AnimatedSprite(explosionTex1, position, new Point(69, 64), 30, 0, false, Game1.rand.Next(0, 360), 0.5f, true, speed, direction);
            effectList.Add(animatedSprite);
            Sound.PlaySound(Sound.Sounds.SmallExplosion, position + new Vector2(34.5f, 32));
            Input.Vibrate(0.2f, 0.15f, 0f);
        }

        static public void Reset()
        {
            effectList.Clear();
        }


        static public void Update(GameTime gameTime)
        {
            for (int i = effectList.Count - 1; i >= 0; i--)
            {
                effectList[i].Update(gameTime);
                if (!effectList[i].IsAlive)
                    effectList.RemoveAt(i);
            }
        }

        static public void Draw(SpriteBatch spriteBatch)
        {
            foreach (AnimatedSprite effect in effectList)
                effect.Draw(spriteBatch);
        }
    }
}
