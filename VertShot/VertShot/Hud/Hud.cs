using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VertShot
{
    public enum HudWindowTypes
    {
        MainMenu,
        Options,
        Graphics,
        Credits,
        GameKeys,
        GameOver,
        Pause
    }
    public enum HudMessageBoxTypes
    {
        GraphicChange,
        GameKeyChange
    }
    public enum hudButtonAction
    {
        None,
        MainMenu,
        NewGame,
        Continue,
        OpenWindow,
        OpenMessageBox,
        SetGameKey,
        ApplyGraphic,
        OK,
        Cancel,
        Quit
    }

    static public class Hud
    {
        static public Dictionary<HudWindowTypes, HudWindow> hudWindowList = new Dictionary<HudWindowTypes, HudWindow>();
        static public Dictionary<HudMessageBoxTypes, HudMessageBox> hudMessageBoxList = new Dictionary<HudMessageBoxTypes, HudMessageBox>();

        static public void Initialize(Texture2D background, Texture2D cornerTop, Texture2D cornerBottom, Texture2D borderTop, Texture2D borderLeft)
        {
            HudWindow.Initialize(background, cornerTop, cornerBottom, borderTop, borderLeft);
            HudMessageBox.Initialize(background, cornerTop, cornerBottom, borderTop, borderLeft);


            // Auflösungsliste holen
            List<string> resList = new List<string>();
            int resDefault = 0;
            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                if (!resList.Contains(mode.Width + "x" + mode.Height) && mode.Width >= 800 && mode.Height >= 600)
                {
                    resList.Add(mode.Width + "x" + mode.Height);
                    if (mode.Width == Game1.Config.resWitdh && mode.Height == Game1.Config.resHeight)
                        resDefault = resList.Count - 1;
                }
            }

            ////Fenster
            // Hauptmenü
            hudWindowList[HudWindowTypes.MainMenu] = new HudWindow(new Rectangle(Game1.Width / 2 - 174, Game1.Height / 2 - 174, 348, 348));
            hudWindowList[HudWindowTypes.MainMenu].AddButton(new Rectangle(40, 40, 268, 40), "Neues Spiel", hudButtonAction.NewGame);
            hudWindowList[HudWindowTypes.MainMenu].AddButton(new Rectangle(40, 100, 268, 40), "Optionen", hudButtonAction.OpenWindow, HudWindowTypes.Options);
            hudWindowList[HudWindowTypes.MainMenu].AddButton(new Rectangle(40, 160, 268, 40), "Credits", hudButtonAction.OpenWindow, HudWindowTypes.Credits);
            hudWindowList[HudWindowTypes.MainMenu].AddButton(new Rectangle(40, 280, 268, 40), "Beenden", hudButtonAction.Quit);
            // Optionen
            hudWindowList[HudWindowTypes.Options] = new HudWindow(new Rectangle(Game1.Width / 2 - 174, Game1.Height / 2 - 174, 348, 348));
            hudWindowList[HudWindowTypes.Options].AddButton(new Rectangle(40, 40, 268, 40), "Grafikoptionen", hudButtonAction.OpenWindow, HudWindowTypes.Graphics);
            hudWindowList[HudWindowTypes.Options].AddButton(new Rectangle(40, 100, 268, 40), "Soundoptionen", hudButtonAction.None);
            hudWindowList[HudWindowTypes.Options].AddButton(new Rectangle(40, 160, 268, 40), "Tastenbelegung", hudButtonAction.OpenWindow, HudWindowTypes.GameKeys);
            hudWindowList[HudWindowTypes.Options].AddButton(new Rectangle(40, 280, 268, 40), "Zurück", hudButtonAction.OpenWindow, HudWindowTypes.MainMenu);
            // Grafikoptionen
            hudWindowList[HudWindowTypes.Graphics] = new HudWindow(new Rectangle(Game1.Width / 2 - 174, Game1.Height / 2 - 174, 348, 348));
            hudWindowList[HudWindowTypes.Graphics].AddList(new Rectangle(40, 40, 268, 40), resList, resDefault);
            hudWindowList[HudWindowTypes.Graphics].AddCheckBox(new Rectangle(40, 100, 268, 40), "Vollbild", Game1.game.IsFullScreen);
            hudWindowList[HudWindowTypes.Graphics].AddButton(new Rectangle(40, 280, 160, 40), "Übernehmen", hudButtonAction.ApplyGraphic, new int[] { 0, 0 });
            hudWindowList[HudWindowTypes.Graphics].AddButton(new Rectangle(208, 280, 110, 40), "Zurück", hudButtonAction.OpenWindow, HudWindowTypes.Options);
            // Tastenbelegung
            hudWindowList[HudWindowTypes.GameKeys] = new HudWindow(new Rectangle(Game1.Width / 2 - 174, Game1.Height / 2 - 200, 348, 400));
            hudWindowList[HudWindowTypes.GameKeys].AddButton(new Rectangle(40, 20, 268, 40), "[GAMEKEY]: [LEFT]", hudButtonAction.OpenMessageBox, GameKeys.Left, true);
            hudWindowList[HudWindowTypes.GameKeys].AddButton(new Rectangle(40, 70, 268, 40), "[GAMEKEY]: [RIGHT]", hudButtonAction.OpenMessageBox, GameKeys.Right, true);
            hudWindowList[HudWindowTypes.GameKeys].AddButton(new Rectangle(40, 120, 268, 40), "[GAMEKEY]: [UP]", hudButtonAction.OpenMessageBox, GameKeys.Up, true);
            hudWindowList[HudWindowTypes.GameKeys].AddButton(new Rectangle(40, 170, 268, 40), "[GAMEKEY]: [DOWN]", hudButtonAction.OpenMessageBox, GameKeys.Down, true);
            hudWindowList[HudWindowTypes.GameKeys].AddButton(new Rectangle(40, 220, 268, 40), "[GAMEKEY]: [FIRE1]", hudButtonAction.OpenMessageBox, GameKeys.Fire1, true);
            hudWindowList[HudWindowTypes.GameKeys].AddButton(new Rectangle(40, 270, 268, 40), "[GAMEKEY]: [FIRE2]", hudButtonAction.OpenMessageBox, GameKeys.Fire2, true);

            hudWindowList[HudWindowTypes.GameKeys].AddButton(new Rectangle(40, 320, 268, 40), "Zurück", hudButtonAction.OpenWindow, HudWindowTypes.Options);
            // Credits
            hudWindowList[HudWindowTypes.Credits] = new HudWindow(new Rectangle(Game1.Width / 2 - 174, Game1.Height / 2 - 174, 348, 348));
            hudWindowList[HudWindowTypes.Credits].AddLabel(new Vector2(20, 40), "Hier stehen die\nMacher drin.");
            hudWindowList[HudWindowTypes.Credits].AddButton(new Rectangle(40, 280, 268, 40), "Zurück", hudButtonAction.OpenWindow, HudWindowTypes.MainMenu);
            // Pause
            hudWindowList[HudWindowTypes.Pause] = new HudWindow(new Rectangle(Game1.Width / 2 - 174, Game1.Height / 2 - 174, 348, 348));
            hudWindowList[HudWindowTypes.Pause].AddButton(new Rectangle(40, 40, 268, 40), "Fortsetzen", hudButtonAction.Continue);
            hudWindowList[HudWindowTypes.Pause].AddButton(new Rectangle(40, 220, 268, 40), "Hauptmenü", hudButtonAction.MainMenu);
            hudWindowList[HudWindowTypes.Pause].AddButton(new Rectangle(40, 280, 268, 40), "Beenden", hudButtonAction.Quit);
            // GameOver
            hudWindowList[HudWindowTypes.GameOver] = new HudWindow(new Rectangle(Game1.Width / 2 - 174, Game1.Height / 2 - 174, 348, 348));
            hudWindowList[HudWindowTypes.GameOver].AddLabel(new Vector2(20, 40), "GAME OVER!!!\n\nDein Schiff\nist schrott!\n\nAbschüsse: [SCORE]");
            hudWindowList[HudWindowTypes.GameOver].AddButton(new Rectangle(40, 280, 268, 40), "Hauptmenü", hudButtonAction.MainMenu);

            ////Message Box
            // Grafik geändert
            hudMessageBoxList[HudMessageBoxTypes.GraphicChange] = new HudMessageBox(new Rectangle(Game1.Width / 2 - 400, Game1.Height / 2 - 74, 800, 148),
                "Grafikeinstellungen übernehmen? Restliche Zeit: [TIMER]", true, true, 15, -1);
            hudMessageBoxList[HudMessageBoxTypes.GameKeyChange] = new HudMessageBox(new Rectangle(Game1.Width / 2 - 300, Game1.Height / 2 - 74, 600, 148),
                "Drücke eine Taste für [GAMEKEY]", false, true, -1, -1);
        }


        static public void RefreshWindowPositions()
        {
            if (hudWindowList.Count > 0)
            {
                hudWindowList[HudWindowTypes.MainMenu].RefreshPosition(new Vector2(Game1.Width / 2 - 174, Game1.Height / 2 - 174));
                hudWindowList[HudWindowTypes.Options].RefreshPosition(new Vector2(Game1.Width / 2 - 174, Game1.Height / 2 - 174));
                hudWindowList[HudWindowTypes.GameKeys].RefreshPosition(new Vector2(Game1.Width / 2 - 174, Game1.Height / 2 - 174));
                hudWindowList[HudWindowTypes.Credits].RefreshPosition(new Vector2(Game1.Width / 2 - 174, Game1.Height / 2 - 174));
                hudWindowList[HudWindowTypes.Pause].RefreshPosition(new Vector2(Game1.Width / 2 - 174, Game1.Height / 2 - 174));
                hudWindowList[HudWindowTypes.GameOver].RefreshPosition(new Vector2(Game1.Width / 2 - 174, Game1.Height / 2 - 174));
            }
        }


        static public void Update(GameTime gameTime)
        {
            bool messageBoxActive = false;
            foreach (HudMessageBoxTypes hMessageBoxType in hudMessageBoxList.Keys)
                if (hudMessageBoxList[hMessageBoxType].active)
                {
                    hudMessageBoxList[hMessageBoxType].Update(gameTime);

                    if (hMessageBoxType == HudMessageBoxTypes.GraphicChange)
                    {
                        if (hudMessageBoxList[hMessageBoxType].Return == -1)
                        {
                            Game1.game.SetLastResolution();
                            hudMessageBoxList[hMessageBoxType].active = false;
                            messageBoxActive = false;
                        }
                        else if (hudMessageBoxList[hMessageBoxType].Return == 1)
                        {
                            hudMessageBoxList[hMessageBoxType].active = false;
                            messageBoxActive = false;
                        }
                    }
                    else if (hMessageBoxType == HudMessageBoxTypes.GameKeyChange)
                    {
                        Microsoft.Xna.Framework.Input.Keys key = hudMessageBoxList[hMessageBoxType].pressedKeyCode;
                        if (hudMessageBoxList[hMessageBoxType].Return == -1)
                        {
                            hudMessageBoxList[hMessageBoxType].active = false;
                            messageBoxActive = false;
                        }
                        else if (key != Microsoft.Xna.Framework.Input.Keys.None && key != Microsoft.Xna.Framework.Input.Keys.Escape)
                        {
                            Input.AssignKeyboard[(GameKeys)hudMessageBoxList[hMessageBoxType].value] = key;
                            hudMessageBoxList[hMessageBoxType].active = false;
                            messageBoxActive = false;
                            LoadSave.SaveConfig(Game1.Config);
                        }
                    }
                    else
                        messageBoxActive = true;
                }
            if (!messageBoxActive)
                foreach (HudWindow hWindow in hudWindowList.Values)
                    if (hWindow.active) hWindow.Update(gameTime);
        }

        static public void SetMessageBoxValue(HudMessageBoxTypes hudMessageBox, object value)
        {
            hudMessageBoxList[hudMessageBox].value = value;
        }

        static public void ShowMessageBox(HudMessageBoxTypes hudMessageBox)
        {
            foreach (HudWindow hWindow in hudWindowList.Values)
                hWindow.Reset();
            foreach (HudMessageBox hMessageBox in hudMessageBoxList.Values)
            {
                hMessageBox.Reset();
                hMessageBox.active = false;
            }
            hudMessageBoxList[hudMessageBox].active = true;
        }

        static public void ShowWindow(HudWindowTypes hudWindow)
        {
            foreach (HudWindow hWindow in hudWindowList.Values)
            {
                hWindow.Reset();
                hWindow.active = false;
            }
            hudWindowList[hudWindow].active = true;
        }

        static public void CloseAllWindows()
        {
            foreach (HudWindow hWindow in hudWindowList.Values)
            {
                hWindow.active = false;
            }
        }


        static public void Draw(SpriteBatch spriteBatch)
        {
            bool messageBoxActive = false;
            foreach (HudMessageBox hMessageBox in hudMessageBoxList.Values)
                if (hMessageBox.active) { hMessageBox.Draw(spriteBatch); messageBoxActive = true; }
            if (!messageBoxActive)
                foreach (HudWindow hWindow in hudWindowList.Values)
                    if (hWindow.active) hWindow.Draw(spriteBatch);
        }
    }
}
