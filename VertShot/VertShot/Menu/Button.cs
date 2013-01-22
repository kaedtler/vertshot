using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VertShot.Menu
{
    public class Button : MenuObject
    {
        public ButtonAction buttonAction { get; private set; }
        public bool buttonPressed;
        bool mouseOver;
        bool mouseDown;
        bool secondRun;

        public Button(Rectangle rect, String text, ButtonAction buttonAction, object value = null, bool replaceText = false)
        {
            this.position = new Vector2(rect.Location.X, rect.Location.Y);
            this.size = new Vector2(rect.Width, rect.Height);
            this.text = text;
            this.buttonAction = buttonAction;
            this.value = value;
            this.replaceText = replaceText;
        }

        public override void Update(GameTime gameTime)
        {
            if (secondRun)
            {
                mouseOver = rect.Contains(Input.MouseScaledPoint);
                mouseDown = mouseOver && Input.IsMouseKeyDown(MouseKeys.Left);
                buttonPressed = mouseOver && Input.IsMouseKeyReleased(MouseKeys.Left);
                if (buttonPressed) Input.RefreshMouseInput();
            }
            else
                secondRun = true;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.oneTexture, rect, mouseDown ? new Color(0, 40, 0, 160) : mouseOver ? new Color(20, 80, 0, 160) : new Color(0, 60, 0, 160));
            spriteBatch.DrawString(Game1.buttonFont, GetText, new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2) - Game1.buttonFont.MeasureString(GetText) / 2, new Color(20, 120, 0, 160));
        }

        public override void Reset()
        {
            mouseOver = mouseDown = buttonPressed = secondRun = false;
        }
    }
}
