using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VertShot.Menu
{

    public class Window
    {
        static Texture2D background;
        static Texture2D cornerTop;
        static Texture2D cornerBottom;
        static Texture2D borderTop;
        static Texture2D borderLeft;
        public bool active;

        List<MenuObject> hudObjList = new List<MenuObject>();
        Rectangle rect;


        static public void Initialize(Texture2D background, Texture2D cornerTop, Texture2D cornerBottom, Texture2D borderTop, Texture2D borderLeft)
        {
            Window.background = background;
            Window.cornerTop = cornerTop;
            Window.cornerBottom = cornerBottom;
            Window.borderTop = borderTop;
            Window.borderLeft = borderLeft;
        }

        public Window(Point size)
        {
            this.rect = new Rectangle(Game1.Width / 2 - size.X / 2, Game1.Height / 2 - size.Y / 2, size.X, size.Y);
            active = false;
        }

        public Window(Rectangle rect)
        {
            this.rect = rect;
            active = false;
        }

        public void AddObject(MenuObject hudObject)
        {
            hudObject.position.X += this.rect.X;
            hudObject.position.Y += this.rect.Y;
            hudObject.RefreshPosition(Vector2.Zero);
            hudObjList.Add(hudObject);
        }


        public void Reset()
        {
            foreach (MenuObject hudObj in hudObjList)
                hudObj.Reset();
        }


        public void RefreshPosition()
        {
            RefreshPosition(new Vector2(Game1.Width / 2 - rect.Width / 2, Game1.Height / 2 - rect.Height / 2));
        }


        public void RefreshPosition(Vector2 position)
        {
            foreach (MenuObject hudObj in hudObjList)
                hudObj.RefreshPosition(position - new Vector2(rect.X, rect.Y));

            rect.X = (int)position.X;
            rect.Y = (int)position.Y;
        }


        public void Update(GameTime gameTime)
        {
            foreach (MenuObject hudObj in hudObjList)
            {
                hudObj.Update(gameTime);
                if (hudObj.GetType() == typeof(Button) && ((Button)hudObj).buttonPressed)
                {
                    Button hudButton = (Button)hudObj;
                    switch (hudButton.buttonAction)
                    {
                        case ButtonAction.MainMenu:
                            Game1.SetGameState(GameState.MainMenu);
                            break;
                        case ButtonAction.NewGame:
                            Game1.SetGameState(GameState.NewGame);
                            break;
                        case ButtonAction.Continue:
                            Game1.SetGameState(GameState.Game);
                            break;
                        case ButtonAction.ApplyGraphic:
                            {
                                byte[] i = (byte[])hudButton.value;
                                string[] res = hudObjList[i[0]].GetText.Split(new char[] { 'x' });
                                Game1.game.SetResolution(Int32.Parse(res[0]), Int32.Parse(res[1]), (bool)hudObjList[i[1]].value);
                                Menu.ShowMessageBox(MessageBoxTypes.GraphicChange);
                                break;
                            }
                        case ButtonAction.ApplySound:
                            {
                                byte[] i = (byte[])hudButton.value;
                                Game1.Config.soundVol = byte.Parse(hudObjList[i[0]].GetText);
                                Game1.Config.musicVol = byte.Parse(hudObjList[i[1]].GetText);
                                LoadSave.SaveConfig(Game1.Config);
                                Sound.SetSoundVolume();
                                Sound.SetMusicVolume();
                                break;
                            }
                        case ButtonAction.ApplyShipColor:
                            {
                                byte[] i = (byte[])hudButton.value;
                                Game1.Config.shipColorR = byte.Parse(hudObjList[i[0]].GetText);
                                Game1.Config.shipColorG = byte.Parse(hudObjList[i[1]].GetText);
                                Game1.Config.shipColorB = byte.Parse(hudObjList[i[2]].GetText);
                                LoadSave.SaveConfig(Game1.Config);
                                break;
                            }
                        case ButtonAction.OpenWindow:
                            Menu.ShowWindow((WindowTypes)hudButton.value);
                            break;
                        case ButtonAction.OpenMessageBox:
                            if (hudButton.value.GetType() == typeof(GameKeys))
                            {
                                Menu.SetMessageBoxValue(MessageBoxTypes.GameKeyChange, (GameKeys)hudButton.value);
                                Menu.ShowMessageBox(MessageBoxTypes.GameKeyChange);
                            }
                            else
                                Menu.ShowMessageBox((MessageBoxTypes)hudButton.value);
                            break;
                        case ButtonAction.Quit:
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

            foreach (MenuObject hudObj in hudObjList)
                hudObj.Draw(spriteBatch);
        }
    }
}
