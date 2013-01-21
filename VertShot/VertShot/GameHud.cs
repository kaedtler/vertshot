using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VertShot
{
    static public class GameHud
    {
        static Texture2D hudTex;
        static Texture2D hudTexRight;
        static SpriteFont hudFont;
        static Color hudColor;
        static float hudEnergy;
        static float hudShield;

        static Rectangle energyRect { get { return new Rectangle(Game1.Width - hudTex.Width + 14, 7, Convert.ToInt32(Math.Ceiling(hudEnergy * 2)), 20); } }
        static Rectangle shieldRect { get { return new Rectangle(Game1.Width - hudTex.Width + 14, 35, Convert.ToInt32(Math.Ceiling(hudShield * 2)), 20); } }

        static public void Initialize(Texture2D hudTex, Texture2D hudTexRight, SpriteFont hudFont)
        {
            Initialize(hudTex, hudTexRight, hudFont, Color.White);
        }
        static public void Initialize(Texture2D hudTex, Texture2D hudTexRight, SpriteFont hudFont, Color hudColor)
        {
            GameHud.hudTex = hudTex;
            GameHud.hudTexRight = hudTexRight;
            GameHud.hudFont = hudFont;
            GameHud.hudColor = hudColor;
        }

        static public void Update(GameTime gameTime)
        {
            if (hudEnergy < Game1.player.energy)
                hudEnergy = Math.Min(hudEnergy + 0.1f * (float)gameTime.ElapsedGameTime.TotalMilliseconds, Game1.player.energy);
            else if (hudEnergy > Game1.player.energy)
                hudEnergy = Math.Max(hudEnergy - 0.1f * (float)gameTime.ElapsedGameTime.TotalMilliseconds, Game1.player.energy);

            if (hudShield < Game1.player.shield)
                hudShield = Math.Min(hudShield + 0.1f * (float)gameTime.ElapsedGameTime.TotalMilliseconds, Game1.player.shield);
            else if (hudShield > Game1.player.shield)
                hudShield = Math.Max(hudShield - 0.1f * (float)gameTime.ElapsedGameTime.TotalMilliseconds, Game1.player.shield);
        }

        static public void Reset()
        {
            hudEnergy = 0;
            hudShield = 0;
        }

        static public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(hudTexRight, new Vector2(Game1.Width - hudTex.Width, 0), hudColor);
            spritebatch.Draw(hudTex, new Vector2(0, 0), null, hudColor, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
            spritebatch.Draw(Game1.oneTexture, energyRect, (hudEnergy <= 20f ? new Color(160, 0, 0, 160) : new Color(48, 160, 0, 160)));
            spritebatch.Draw(Game1.oneTexture, shieldRect, new Color(0, 93, 160, 160));
            spritebatch.DrawString(hudFont, "█████ ███", new Vector2(20, 20), new Color(0, 60, 0, 180));
            spritebatch.DrawString(hudFont,
                (Game1.gametimeCounter < 6000 ? String.Format("{0,2:d}:{1,2:d2}", (byte)(Game1.gametimeCounter / 60), (byte)(Game1.gametimeCounter % 60)) : "██:██") +
                (Game1.enemyCounter < 1000 ? String.Format(" {0,3:d}", Game1.enemyCounter) : " ███"),
                new Vector2(20, 20), new Color(48, 160, 0, 200));
        }
    }
}
