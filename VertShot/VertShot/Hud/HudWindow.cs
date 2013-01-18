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

        List<HudObject> hudObjList = new List<HudObject>();
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

        public void AddObject(HudObject hudObject)
        {
            hudObject.position.X += this.rect.X;
            hudObject.position.Y += this.rect.Y;
            hudObject.RefreshPosition(Vector2.Zero);
            hudObjList.Add(hudObject);
        }


        public void Reset()
        {
            foreach (HudObject hudObj in hudObjList)
                hudObj.Reset();
        }


        public void RefreshPosition(Vector2 position)
        {
            foreach (HudObject hudObj in hudObjList)
                hudObj.RefreshPosition(position - new Vector2(rect.X, rect.Y));

            rect.X = (int)position.X;
            rect.Y = (int)position.Y;
        }


        public void Update(GameTime gameTime)
        {
            foreach (HudObject hudObj in hudObjList)
            {
                hudObj.Update(gameTime);
                if (hudObj.GetType() == typeof(HudButton) && ((HudButton)hudObj).buttonPressed)
                {
                    HudButton hudButton = (HudButton)hudObj;
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
                                byte[] i = (byte[])hudButton.value;
                                string[] res = hudObjList[i[0]].GetText.Split(new char[] { 'x' });
                                Game1.game.SetResolution(Int32.Parse(res[0]), Int32.Parse(res[1]), (bool)hudObjList[i[1]].value);
                                Hud.ShowMessageBox(HudMessageBoxTypes.GraphicChange);
                                break;
                            }
                        case HudButtonAction.ApplySound:
                            {
                                byte[] i = (byte[])hudButton.value;
                                Game1.Config.soundVol = byte.Parse(hudObjList[i[0]].GetText);
                                Game1.Config.musicVol = byte.Parse(hudObjList[i[1]].GetText);
                                LoadSave.SaveConfig(Game1.Config);
                                Hud.ShowWindow(HudWindowTypes.Options);
                                break;
                            }
                        case HudButtonAction.ApplyShipColor:
                            {
                                byte[] i = (byte[])hudButton.value;
                                Game1.Config.shipColorR = byte.Parse(hudObjList[i[0]].GetText);
                                Game1.Config.shipColorG = byte.Parse(hudObjList[i[1]].GetText);
                                Game1.Config.shipColorB = byte.Parse(hudObjList[i[2]].GetText);
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

            foreach (HudObject hudObj in hudObjList)
                hudObj.Draw(spriteBatch);
        }
    }
}
