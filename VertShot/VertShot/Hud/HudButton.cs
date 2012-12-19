using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VertShot
{
    public class HudButton
    {
        Rectangle rect;
        String text;
        public hudButtonAction buttonAction { get; private set; }
        public object value { get; private set; }
        public bool buttonPressed { get; private set; }
        bool mouseOver;
        bool mouseDown;
        bool secondRun;

        public HudButton(Rectangle rect, String text, hudButtonAction buttonAction, object value = null)
        {
            this.rect = rect;
            this.text = text;
            this.buttonAction = buttonAction;
            this.value = value;
        }

        public void Update(GameTime gameTime)
        {
            if (secondRun)
            {
                mouseOver = rect.Contains(Input.MousePoint);
                mouseDown = mouseOver && Input.IsMouseKeyDown(MouseKeys.Left);
                buttonPressed = mouseOver && Input.IsMouseKeyReleased(MouseKeys.Left);
            }
            secondRun = true;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if(!mouseOver && !mouseDown)
                spriteBatch.Draw(Game1.oneTexture, rect, new Color(0, 60, 0, 160));
            else if(mouseOver && !mouseDown)
                spriteBatch.Draw(Game1.oneTexture, rect, new Color(20, 80, 0, 160));
            else if(mouseOver && mouseDown)
                spriteBatch.Draw(Game1.oneTexture, rect, new Color(0, 40, 0, 160));
            spriteBatch.DrawString(Game1.buttonFont, text, new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2) - Game1.buttonFont.MeasureString(text) / 2, new Color(20, 120, 0, 160));
        }

        internal void Reset()
        {
            mouseOver = mouseDown = buttonPressed = secondRun = false;
        }
    }
}
