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
        public bool active;
        List<HudButton> buttonList = new List<HudButton>();
        List<HudLabel> labelList = new List<HudLabel>();
        List<HudList> listList = new List<HudList>();
        List<HudCheckBox> checkBoxList = new List<HudCheckBox>();
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

        public void AddButton(Rectangle rect, String text, HudButtonAction buttonAction, object value = null, bool replaceText = false)
        {
            rect.X += this.rect.X;
            rect.Y += this.rect.Y;
            buttonList.Add(new HudButton(rect, text, buttonAction, value, replaceText));
        }

        public void AddList(Rectangle rect, List<string> stringList, int defaultValue = 0)
        {
            rect.X += this.rect.X;
            rect.Y += this.rect.Y;
            listList.Add(new HudList(rect, "", stringList, defaultValue));
        }

        public void AddList(Rectangle rect, string preString, List<string> stringList, int defaultValue = 0)
        {
            rect.X += this.rect.X;
            rect.Y += this.rect.Y;
            listList.Add(new HudList(rect, preString, stringList, defaultValue));
        }

        public void AddCheckBox(Rectangle rect, string text, bool defaultValue = false)
        {
            rect.X += this.rect.X;
            rect.Y += this.rect.Y;
            checkBoxList.Add(new HudCheckBox(rect, text, defaultValue));
        }

        public void AddLabel(Vector2 position, String text)
        {
            position.X += this.rect.X;
            position.Y += this.rect.Y;
            labelList.Add(new HudLabel(position, text));
        }


        public void Reset()
        {
            foreach (HudButton hudButton in buttonList)
                hudButton.Reset();
            foreach (HudList hList in listList)
                hList.Reset();
            foreach (HudCheckBox hudCheck in checkBoxList)
                hudCheck.Reset();
        }


        public void RefreshPosition(Vector2 position)
        {
            foreach (HudButton hudButton in buttonList)
                hudButton.RefreshPosition(hudButton.GetPosition - new Vector2(rect.X, rect.Y) + position);
            foreach (HudList hList in listList)
                hList.RefreshPosition(hList.GetPosition - new Vector2(rect.X, rect.Y) + position);
            foreach (HudLabel hudLabel in labelList)
                hudLabel.RefreshPosition(hudLabel.GetPosition - new Vector2(rect.X, rect.Y) + position);
            foreach (HudCheckBox hudCheck in checkBoxList)
                hudCheck.RefreshPosition(hudCheck.GetPosition - new Vector2(rect.X, rect.Y) + position);

            rect.X = (int)position.X;
            rect.Y = (int)position.Y;
        }


        public void Update(GameTime gameTime)
        {
            foreach (HudList hList in listList)
                hList.Update(gameTime);

            foreach (HudCheckBox hudCheck in checkBoxList)
                hudCheck.Update(gameTime);

            foreach (HudButton hButton in buttonList)
                hButton.Update(gameTime);

            HudButton hudButton = buttonList.Find(h => h.buttonPressed);
            if (hudButton != null)
            {
                switch (hudButton.buttonAction)
                {
                    case HudButtonAction.MainMenu:
                        Game1.SetGameState(GameState.MainMenu);
                        break;
                    case HudButtonAction.NewGame:
                        Game1.SetGameState(GameState.NewGame);
                        break;
                    case HudButtonAction.Continue:
                        Game1.SetGameState(GameState.Game);
                        break;
                    case HudButtonAction.ApplyGraphic:
                        {
                            int[] i = (int[])hudButton.value;
                            string[] res = listList[i[0]].StringValue.Split(new char[] { 'x' });
                            Game1.game.SetResolution(Int32.Parse(res[0]), Int32.Parse(res[1]), checkBoxList[i[1]].value);
                            Hud.ShowMessageBox(HudMessageBoxTypes.GraphicChange);
                            break;
                        }
                    case HudButtonAction.ApplySound:
                        {
                            int[] i = (int[])hudButton.value;
                            Game1.Config.soundVol = byte.Parse(listList[i[0]].StringValue);
                            Game1.Config.musicVol = byte.Parse(listList[i[1]].StringValue);
                            LoadSave.SaveConfig(Game1.Config);
                            Hud.ShowWindow(HudWindowTypes.Options);
                            break;
                        }
                    case HudButtonAction.ApplyShipColor:
                        {
                            int[] i = (int[])hudButton.value;
                            Game1.Config.shipColorR = byte.Parse(listList[i[0]].StringValue);
                            Game1.Config.shipColorG = byte.Parse(listList[i[1]].StringValue);
                            Game1.Config.shipColorB = byte.Parse(listList[i[2]].StringValue);
                            LoadSave.SaveConfig(Game1.Config);
                            Hud.ShowWindow(HudWindowTypes.Options);
                            break;
                        }
                    case HudButtonAction.OpenWindow:
                        Hud.ShowWindow((HudWindowTypes)hudButton.value);
                        break;
                    case HudButtonAction.OpenMessageBox:
                        if (hudButton.value.GetType() == typeof(GameKeys))
                        {
                            Hud.SetMessageBoxValue(HudMessageBoxTypes.GameKeyChange, (GameKeys)hudButton.value);
                            Hud.ShowMessageBox(HudMessageBoxTypes.GameKeyChange);
                        }
                        else
                            Hud.ShowMessageBox((HudMessageBoxTypes)hudButton.value);
                        break;
                    case HudButtonAction.Quit:
                        Game1.SetGameState(GameState.Quit);
                        break;
                    default: break;
                }
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
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

            foreach (HudButton hudButton in buttonList)
                hudButton.Draw(spriteBatch);
            foreach (HudList hList in listList)
                hList.Draw(spriteBatch);
            foreach (HudCheckBox hudCheck in checkBoxList)
                hudCheck.Draw(spriteBatch);
            foreach (HudLabel hudLabel in labelList)
                hudLabel.Draw(spriteBatch);
        }
    }
}
