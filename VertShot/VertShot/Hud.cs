using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VertShot
{
    static public class Hud
    {
        static Texture2D background;
        static Texture2D cornerTop;
        static Texture2D cornerBottom;
        static Texture2D borderTop;
        static Texture2D borderLeft;
        // Temp
        static Rectangle rect;

        static public void Initialize(Texture2D background, Texture2D cornerTop, Texture2D cornerBottom, Texture2D borderTop, Texture2D borderLeft)
        {
            Hud.background = background;
            Hud.cornerTop = cornerTop;
            Hud.cornerBottom = cornerBottom;
            Hud.borderTop = borderTop;
            Hud.borderLeft = borderLeft;

            rect = new Rectangle(500, 200, 348,348);
        }


        static public void Update(GameTime gameTime)
        {
        }


        static public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null);
            spriteBatch.Draw(background, new Vector2(rect.X + cornerTop.Width, rect.Y), new Rectangle(0, 0, rect.Width - cornerTop.Width * 2, cornerTop.Height), Color.White);
            spriteBatch.Draw(background, new Vector2(rect.X + cornerTop.Width, rect.Y + rect.Height - cornerBottom.Height), new Rectangle(0, 0, rect.Width - cornerTop.Width * 2, cornerTop.Height), Color.White);
            spriteBatch.Draw(background, new Vector2(rect.X, rect.Y + cornerBottom.Height), new Rectangle(0, 0, rect.Width, rect.Height - cornerBottom.Height * 2), Color.White);
            spriteBatch.Draw(borderTop, new Vector2(rect.X + cornerTop.Width, rect.Y), new Rectangle(0, 0, rect.Width - cornerTop.Width * 2, borderTop.Height), Color.White);
            spriteBatch.Draw(borderTop, new Vector2(rect.X + cornerTop.Width, rect.Y + rect.Height - borderTop.Height),
                new Rectangle(0, 0, rect.Width - cornerTop.Width * 2, borderTop.Height),Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipVertically, 0);
            spriteBatch.Draw(borderLeft, new Vector2(rect.X, rect.Y + cornerTop.Height), new Rectangle(0, 0, borderLeft.Width, rect.Height - cornerTop.Height * 2), Color.White);
            spriteBatch.Draw(borderLeft, new Vector2(rect.X + rect.Width - borderLeft.Width, rect.Y + cornerTop.Height),
                new Rectangle(0, 0, borderLeft.Width, rect.Height - cornerTop.Height * 2), Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
            spriteBatch.End();
            spriteBatch.Begin();
            spriteBatch.Draw(cornerTop, new Vector2(rect.X, rect.Y), Color.White);
            spriteBatch.Draw(cornerTop, new Vector2(rect.X + rect.Width - cornerTop.Width, rect.Y), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(cornerBottom, new Vector2(rect.X, rect.Y + rect.Height - cornerBottom.Height), Color.White);
            spriteBatch.Draw(cornerBottom, new Vector2(rect.X + rect.Width - cornerTop.Width, rect.Y + rect.Height - cornerBottom.Height), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);

        }
    }
}
