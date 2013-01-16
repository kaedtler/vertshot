using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VertShot
{
    public class HudList
    {
        Rectangle rect;
        List<string> stringList;
        string preString;
        int value;
        bool mouseOverB1;
        bool mouseOverB2;
        bool mouseDownB1;
        bool mouseDownB2;
        bool secondRun;
        Rectangle rectButton1;
        Rectangle rectButton2;
        Rectangle rectLabel;

        public Vector2 GetPosition { get { return new Vector2(rect.X, rect.Y); } }


        public HudList(Rectangle rect, string preString, List<string> stringList, int defaultValue = 0)
        {
            this.rect = rect;
            rectButton1 = new Rectangle(rect.X, rect.Y, rect.Height, rect.Height);
            rectButton2 = new Rectangle(rect.Right - rect.Height, rect.Y, rect.Height, rect.Height);
            rectLabel = new Rectangle(rect.X + rect.Height + rect.Height / 10, rect.Y, rect.Width - rect.Height - rect.Height / 10, rect.Height);

            this.preString = preString;
            this.stringList = stringList;
            value = defaultValue;

        }

        public string StringValue { get { return stringList[value]; } }

        public void Update(GameTime gameTime)
        {
            if (secondRun)
            {
                mouseOverB1 = rectButton1.Contains(Input.MouseScaledPoint);
                mouseOverB2 = rectButton2.Contains(Input.MouseScaledPoint);
                mouseDownB1 = mouseOverB1 && Input.IsMouseKeyDown(MouseKeys.Left);
                mouseDownB2 = mouseOverB2 && Input.IsMouseKeyDown(MouseKeys.Left);
                if (mouseOverB1 && Input.IsMouseKeyReleased(MouseKeys.Left))
                    value = Math.Max(value - 1, 0);
                else if (mouseOverB2 && Input.IsMouseKeyReleased(MouseKeys.Left))
                    value = Math.Min(value + 1, stringList.Count - 1);
            }
            else
                secondRun = true;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.oneTexture, rectButton1, mouseDownB1 ? new Color(0, 40, 0, 160) : mouseOverB1 ? new Color(20, 80, 0, 160) : new Color(0, 60, 0, 160));
            spriteBatch.Draw(Game1.oneTexture, rectButton2, mouseDownB2 ? new Color(0, 40, 0, 160) : mouseOverB2 ? new Color(20, 80, 0, 160) : new Color(0, 60, 0, 160));
            spriteBatch.DrawString(Game1.buttonFont, "<", new Vector2(rectButton1.X + rectButton1.Width / 2, rectButton1.Y + rectButton1.Height / 2) - Game1.buttonFont.MeasureString("<") / 2, new Color(20, 120, 0, 160));
            spriteBatch.DrawString(Game1.buttonFont, ">", new Vector2(rectButton2.X + rectButton2.Width / 2, rectButton2.Y + rectButton2.Height / 2) - Game1.buttonFont.MeasureString(">") / 2, new Color(20, 120, 0, 160));
            spriteBatch.DrawString(Game1.buttonFont, preString + stringList[value], new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2) - Game1.buttonFont.MeasureString(preString + stringList[value]) / 2, new Color(20, 120, 0, 160));
        }

        internal void Reset()
        {
            mouseOverB1 = mouseOverB2 = mouseDownB1 = mouseDownB2 = secondRun = false;
        }

        internal void RefreshPosition(Vector2 position)
        {
            rect.X = (int)position.X;
            rect.Y = (int)position.Y;
            rectButton1 = new Rectangle(rect.X, rect.Y, rect.Height, rect.Height);
            rectButton2 = new Rectangle(rect.Right - rect.Height, rect.Y, rect.Height, rect.Height);
            rectLabel = new Rectangle(rect.X + rect.Height + rect.Height / 10, rect.Y, rect.Width - rect.Height - rect.Height / 10, rect.Height);
        }
    }
}
