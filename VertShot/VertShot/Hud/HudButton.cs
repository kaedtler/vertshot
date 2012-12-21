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
        String startText;
        String text;
        public hudButtonAction buttonAction { get; private set; }
        public object value { get; private set; }
        public bool buttonPressed
        {
            get
            {
                if (bPressed)
                {
                    bPressed = false;
                    return true;
                }
                else
                    return false;
            }
        }
        bool bPressed;
        bool mouseOver;
        bool mouseDown;
        bool replaceText;
        bool secondRun;

        public Vector2 GetPosition { get { return new Vector2(rect.X, rect.Y); } }

        public HudButton(Rectangle rect, String text, hudButtonAction buttonAction, object value = null, bool replaceText = false)
        {
            this.rect = rect;
            this.startText = text;
            this.buttonAction = buttonAction;
            this.value = value;
            this.replaceText = replaceText;
            if (!replaceText)
                this.text = startText;
        }

        public void Update(GameTime gameTime)
        {
            if (secondRun)
            {
                mouseOver = rect.Contains(Input.MouseScaledPoint);
                mouseDown = mouseOver && Input.IsMouseKeyDown(MouseKeys.Left);
                bPressed = mouseOver && Input.IsMouseKeyReleased(MouseKeys.Left);
            }
            else
                secondRun = true;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (replaceText)
            {
                text = startText.Replace("[LEFT]", Input.GetGameKeyCodeString(GameKeys.Left)).Replace
                    ("[RIGHT]", Input.GetGameKeyCodeString(GameKeys.Right)).Replace
                    ("[UP]", Input.GetGameKeyCodeString(GameKeys.Up)).Replace
                    ("[DOWN]", Input.GetGameKeyCodeString(GameKeys.Down)).Replace
                    ("[FIRE1]", Input.GetGameKeyCodeString(GameKeys.Fire1)).Replace
                    ("[FIRE2]", Input.GetGameKeyCodeString(GameKeys.Fire2));
                if (value.GetType() == typeof(GameKeys)) text = text.Replace("[GAMEKEY]", Input.GetGameKeyString((GameKeys)value));
            }
            spriteBatch.Draw(Game1.oneTexture, rect, mouseDown ? new Color(0, 40, 0, 160) : mouseOver ? new Color(20, 80, 0, 160) : new Color(0, 60, 0, 160));
            spriteBatch.DrawString(Game1.buttonFont, text, new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2) - Game1.buttonFont.MeasureString(text) / 2, new Color(20, 120, 0, 160));
        }

        internal void RefreshPosition(Vector2 position)
        {
            rect.X = (int)position.X;
            rect.Y = (int)position.Y;
        }

        internal void Reset()
        {
            mouseOver = mouseDown = bPressed = secondRun = false;
        }
    }
}
