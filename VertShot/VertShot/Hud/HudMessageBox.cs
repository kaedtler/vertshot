using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VertShot
{
    public class HudMessageBox
    {
        static Texture2D background;
        static Texture2D cornerTop;
        static Texture2D cornerBottom;
        static Texture2D borderTop;
        static Texture2D borderLeft;
        public bool active;
        Rectangle rect;
        float timerSeconds;
        HudButton ok;
        HudButton cancel;
        int defaultValue;
        string text;
        string startText;
        float startTimerSeconds;
        public object value;
        public int Return { get { return ok != null && ok.buttonPressed || defaultValue == 1 && timerSeconds == 0 ? 1 : cancel != null && cancel.buttonPressed || defaultValue == -1 && timerSeconds == 0 ? -1 : 0; } }
        public Microsoft.Xna.Framework.Input.Keys pressedKeyCode { get { return Input.GetPressedKeyCode(); } }



        static public void Initialize(Texture2D background, Texture2D cornerTop, Texture2D cornerBottom, Texture2D borderTop, Texture2D borderLeft)
        {
            HudMessageBox.background = background;
            HudMessageBox.cornerTop = cornerTop;
            HudMessageBox.cornerBottom = cornerBottom;
            HudMessageBox.borderTop = borderTop;
            HudMessageBox.borderLeft = borderLeft;
        }

        public HudMessageBox(Rectangle rect, string text, bool okButton, bool cancelButton, int timerSeconds, int defaultValue, object value = null)
        {
            this.rect = rect;
            active = false;
            startText = text;
            if (okButton && cancelButton)
            {
                ok = new HudButton(new Rectangle(rect.X + rect.Width / 2 - 145, rect.Bottom - 55, 140, 40), "OK", HudButtonAction.OK);
                cancel = new HudButton(new Rectangle(rect.X + rect.Width / 2 + 5, rect.Bottom - 55, 140, 40), "Abbrechen", HudButtonAction.Cancel);
            }
            else if (okButton)
                ok = new HudButton(new Rectangle(rect.X + rect.Width / 2 - 70, rect.Bottom - 55, 140, 40), "OK", HudButtonAction.OK);
            else if (cancelButton)
                cancel = new HudButton(new Rectangle(rect.X + rect.Width / 2 - 70, rect.Bottom - 55, 140, 40), "Abbrechen", HudButtonAction.Cancel);
            else
                rect.Height -= 60;
            this.startTimerSeconds = timerSeconds;
            this.defaultValue = defaultValue;
            this.value = value;
            Reset();
        }

        internal void Update(GameTime gameTime)
        {
            if (ok != null) ok.Update(gameTime);
            if (cancel != null) cancel.Update(gameTime);

            if (timerSeconds > 0)
                timerSeconds = Math.Max(timerSeconds - (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
        }

        internal void Reset()
        {
            timerSeconds = startTimerSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            text = startText.Replace("[TIMER]", Convert.ToInt32(timerSeconds).ToString());
            if(value != null && value.GetType() == typeof(GameKeys)) text = text.Replace("[GAMEKEY]", Input.GetGameKeyString((GameKeys)value));

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null, null, Game1.scaleMatrix * Game1.transMatrix);
            spriteBatch.Draw(background, new Vector2(rect.X + cornerTop.Width, rect.Y), new Rectangle(0, 0, rect.Width - cornerTop.Width * 2, cornerTop.Height), Color.White);
            spriteBatch.Draw(background, new Vector2(rect.X + cornerTop.Width, rect.Y + rect.Height - cornerBottom.Height), new Rectangle(0, 0, rect.Width - cornerTop.Width * 2, cornerTop.Height), Color.White);
            spriteBatch.Draw(background, new Vector2(rect.X, rect.Y + cornerBottom.Height), new Rectangle(0, 0, rect.Width, rect.Height - cornerBottom.Height * 2), Color.White);
            spriteBatch.Draw(borderTop, new Vector2(rect.X + cornerTop.Width, rect.Y), new Rectangle(0, 0, rect.Width - cornerTop.Width * 2, borderTop.Height), Color.White);
            spriteBatch.Draw(borderTop, new Vector2(rect.X + cornerTop.Width, rect.Y + rect.Height - borderTop.Height),
                new Rectangle(0, 0, rect.Width - cornerTop.Width * 2, borderTop.Height), Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipVertically, 0);
            spriteBatch.Draw(borderLeft, new Vector2(rect.X, rect.Y + cornerTop.Height), new Rectangle(0, 0, borderLeft.Width, rect.Height - cornerTop.Height * 2), Color.White);
            spriteBatch.Draw(borderLeft, new Vector2(rect.X + rect.Width - borderLeft.Width, rect.Y + cornerTop.Height),
                new Rectangle(0, 0, borderLeft.Width, rect.Height - cornerTop.Height * 2), Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, null, null, Game1.scaleMatrix * Game1.transMatrix);
            spriteBatch.Draw(cornerTop, new Vector2(rect.X, rect.Y), Color.White);
            spriteBatch.Draw(cornerTop, new Vector2(rect.X + rect.Width - cornerTop.Width, rect.Y), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(cornerBottom, new Vector2(rect.X, rect.Y + rect.Height - cornerBottom.Height), Color.White);
            spriteBatch.Draw(cornerBottom, new Vector2(rect.X + rect.Width - cornerTop.Width, rect.Y + rect.Height - cornerBottom.Height), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);

            if (ok != null) ok.Draw(spriteBatch);
            if (cancel != null) cancel.Draw(spriteBatch);
            spriteBatch.DrawString(Game1.buttonFont, text, new Vector2(rect.X + rect.Width / 2, rect.Y + (rect.Height - (ok != null || cancel != null ? 60 : 0)) / 2) - Game1.buttonFont.MeasureString(text) / 2, new Color(20, 120, 0, 160));
        }
    }
}
