using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VertShot
{
    public class HudCheckBox
    {
        Rectangle rect;
        string text;
        public bool value { get; private set; }
        bool mouseOver;
        bool mouseDown;
        Rectangle rectButton;
        Rectangle rectLabel;
        bool secondRun;

        public Vector2 GetPosition { get { return new Vector2(rect.X, rect.Y); } }


        public HudCheckBox(Rectangle rect, String text, bool defaultValue = false)
        {
            this.rect = rect;
            this.text = text;
            rectButton = new Rectangle(rect.Right - rect.Height, rect.Y, rect.Height, rect.Height);
            rectLabel = new Rectangle(rect.X, rect.Y, rect.Width - rect.Height - rect.Height / 10, rect.Height);

            value = defaultValue;

        }

        public void Update(GameTime gameTime)
        {
            if (secondRun)
            {
                mouseOver = rectButton.Contains(Input.MouseScaledPoint);
                mouseDown = mouseOver && Input.IsMouseKeyDown(MouseKeys.Left);
                if (mouseOver && Input.IsMouseKeyReleased(MouseKeys.Left))
                    value = !value;
            }
            else
                secondRun = true;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.oneTexture, rectButton, mouseDown ? new Color(0, 40, 0, 160) : mouseOver ? new Color(20, 80, 0, 160) : new Color(0, 60, 0, 160));
            spriteBatch.DrawString(Game1.buttonFont, value ? "√" : "", new Vector2(rectButton.X + rectButton.Width / 2, rectButton.Y + rectButton.Height / 2) - Game1.buttonFont.MeasureString(value ? "√" : "") / 2, new Color(20, 120, 0, 160));
            spriteBatch.DrawString(Game1.buttonFont, text, new Vector2(rectLabel.X, rectLabel.Y + rectLabel.Height / 2 - Game1.buttonFont.MeasureString(text).Y / 2), new Color(20, 120, 0, 160));
            //spriteBatch.DrawString(Game1.buttonFont, text, new Vector2(rectLabel.X + rectLabel.Width / 2, rectLabel.Y + rectLabel.Height / 2) - Game1.buttonFont.MeasureString(text) / 2, new Color(20, 120, 0, 160));
        }

        internal void RefreshPosition(Vector2 position)
        {
            rect.X = (int)position.X;
            rect.Y = (int)position.Y;
            rectButton = new Rectangle(rect.Right - rect.Height, rect.Y, rect.Height, rect.Height);
            rectLabel = new Rectangle(rect.X, rect.Y, rect.Width - rect.Height - rect.Height / 10, rect.Height);
        }

        internal void Reset()
        {
            mouseOver = mouseDown = secondRun = false;
        }
    }
}
