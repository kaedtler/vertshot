using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VertShot
{

    public class HudWindow
    {
        static Texture2D background;
        static Texture2D cornerTop;
        static Texture2D cornerBottom;
        static Texture2D borderTop;
        static Texture2D borderLeft;
        public bool IsActive { get { return active; } set { active = value; if (value) foreach (HudButton b in buttonList)b.Reset(); } }
        bool active;
        List<HudButton> buttonList = new List<HudButton>();
        List<HudLabel> labelList = new List<HudLabel>();
        Rectangle rect;


        static public void Initialize(Texture2D background, Texture2D cornerTop, Texture2D cornerBottom, Texture2D borderTop, Texture2D borderLeft)
        {
            HudWindow.background = background;
            HudWindow.cornerTop = cornerTop;
            HudWindow.cornerBottom = cornerBottom;
            HudWindow.borderTop = borderTop;
            HudWindow.borderLeft = borderLeft;
        }

        public HudWindow(Rectangle rect)
        {
            this.rect = rect;
            active = false;
        }

        public void AddButton(Rectangle rect, String text, hudButtonAction buttonAction, object value = null)
        {
            rect.X += this.rect.X;
            rect.Y += this.rect.Y;
            buttonList.Add(new HudButton(rect, text, buttonAction, value));
        }

        public void AddLabel(Vector2 position, String text)
        {
            position.X += this.rect.X;
            position.Y += this.rect.Y;
            labelList.Add(new HudLabel(position, text));
        }


        public void Update(GameTime gameTime)
        {
            foreach (HudButton hButton in buttonList)
                hButton.Update(gameTime);

            HudButton hudButton = buttonList.Find(h => h.buttonPressed);
            if (hudButton != null)
            {
                switch (hudButton.buttonAction)
                {
                    case hudButtonAction.MainMenu:
                        Game1.SetGameState(GameState.MainMenu);
                        break;
                    case hudButtonAction.NewGame:
                        Game1.SetGameState(GameState.NewGame);
                        break;
                    case hudButtonAction.Continue:
                        Game1.SetGameState(GameState.Game);
                        break;
                    case hudButtonAction.OpenWindow:
                        Hud.ShowWindow((HudWindows)hudButton.value);
                        break;
                    case hudButtonAction.Quit:
                        Game1.SetGameState(GameState.Quit);
                        break;
                    default: break;
                }
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null);
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
            spriteBatch.Begin();
            spriteBatch.Draw(cornerTop, new Vector2(rect.X, rect.Y), Color.White);
            spriteBatch.Draw(cornerTop, new Vector2(rect.X + rect.Width - cornerTop.Width, rect.Y), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(cornerBottom, new Vector2(rect.X, rect.Y + rect.Height - cornerBottom.Height), Color.White);
            spriteBatch.Draw(cornerBottom, new Vector2(rect.X + rect.Width - cornerTop.Width, rect.Y + rect.Height - cornerBottom.Height), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);

            foreach (HudButton hudButton in buttonList)
                hudButton.Draw(spriteBatch);
            foreach (HudLabel hudLabel in labelList)
                hudLabel.Draw(spriteBatch);
        }
    }
}
