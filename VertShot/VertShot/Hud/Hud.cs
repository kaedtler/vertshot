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
        Pause,
        Sound,
        ShipColor
    }
    public enum HudMessageBoxTypes
    {
        GraphicChange,
        GameKeyChange
    }
    public enum HudButtonAction
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
        Quit,
        ApplySound,
        ApplyShipColor
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

            // Farbliste erstellen
            List<string> colorList = new List<string>();
            for (int i = 0; i < 6; i++) colorList.Add((i*51).ToString());

            ////Fenster
            // Hauptmenü
            hudWindowList[HudWindowTypes.MainMenu] = new HudWindow(new Rectangle(Game1.Width / 2 - 174, Game1.Height / 2 - 174, 348, 348));
            hudWindowList[HudWindowTypes.MainMenu].AddObject(new HudButton(new Rectangle(40, 40, 268, 40), "Neues Spiel", HudButtonAction.NewGame));
            hudWindowList[HudWindowTypes.MainMenu].AddObject(new HudButton(new Rectangle(40, 100, 268, 40), "Optionen", HudButtonAction.OpenWindow, HudWindowTypes.Options));
            hudWindowList[HudWindowTypes.MainMenu].AddObject(new HudButton(new Rectangle(40, 160, 268, 40), "Credits", HudButtonAction.OpenWindow, HudWindowTypes.Credits));
            hudWindowList[HudWindowTypes.MainMenu].AddObject(new HudButton(new Rectangle(40, 280, 268, 40), "Beenden", HudButtonAction.Quit));
            // Optionen
            hudWindowList[HudWindowTypes.Options] = new HudWindow(new Rectangle(Game1.Width / 2 - 174, Game1.Height / 2 - 174, 348, 348));
            hudWindowList[HudWindowTypes.Options].AddObject(new HudButton(new Rectangle(40, 40, 268, 40), "Grafikoptionen", HudButtonAction.OpenWindow, HudWindowTypes.Graphics));
            hudWindowList[HudWindowTypes.Options].AddObject(new HudButton(new Rectangle(40, 100, 268, 40), "Soundoptionen", HudButtonAction.OpenWindow, HudWindowTypes.Sound));
            hudWindowList[HudWindowTypes.Options].AddObject(new HudButton(new Rectangle(40, 160, 268, 40), "Tastenbelegung", HudButtonAction.OpenWindow, HudWindowTypes.GameKeys));
            hudWindowList[HudWindowTypes.Options].AddObject(new HudButton(new Rectangle(40, 220, 268, 40), "Schiffsfarbe", HudButtonAction.OpenWindow, HudWindowTypes.ShipColor));
            hudWindowList[HudWindowTypes.Options].AddObject(new HudButton(new Rectangle(40, 280, 268, 40), "Zurück", HudButtonAction.OpenWindow, HudWindowTypes.MainMenu));
            // Grafikoptionen
            hudWindowList[HudWindowTypes.Graphics] = new HudWindow(new Rectangle(Game1.Width / 2 - 174, Game1.Height / 2 - 174, 348, 348));
            hudWindowList[HudWindowTypes.Graphics].AddObject(new HudList(new Rectangle(40, 40, 268, 40), "", resList, resDefault));
            hudWindowList[HudWindowTypes.Graphics].AddObject(new HudCheckBox(new Rectangle(40, 100, 268, 40), "Vollbild", Game1.game.IsFullScreen));
            hudWindowList[HudWindowTypes.Graphics].AddObject(new HudButton(new Rectangle(40, 280, 160, 40), "Übernehmen", HudButtonAction.ApplyGraphic, new byte[] { 0, 1 }));
            hudWindowList[HudWindowTypes.Graphics].AddObject(new HudButton(new Rectangle(208, 280, 110, 40), "Zurück", HudButtonAction.OpenWindow, HudWindowTypes.Options));
            // Soundoptionen
            hudWindowList[HudWindowTypes.Sound] = new HudWindow(new Rectangle(Game1.Width / 2 - 174, Game1.Height / 2 - 174, 348, 348));
            hudWindowList[HudWindowTypes.Sound].AddObject(new HudList(new Rectangle(40, 40, 268, 40), "Sound: ", new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" }, Game1.Config.soundVol));
            hudWindowList[HudWindowTypes.Sound].AddObject(new HudList(new Rectangle(40, 100, 268, 40), "Musik: ", new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" }, Game1.Config.musicVol));
            hudWindowList[HudWindowTypes.Sound].AddObject(new HudButton(new Rectangle(40, 280, 160, 40), "Übernehmen", HudButtonAction.ApplySound, new byte[] { 0, 1 }));
            hudWindowList[HudWindowTypes.Sound].AddObject(new HudButton(new Rectangle(208, 280, 110, 40), "Zurück", HudButtonAction.OpenWindow, HudWindowTypes.Options));
            // Schiffoptionen
            hudWindowList[HudWindowTypes.ShipColor] = new HudWindow(new Rectangle(Game1.Width / 2 - 174, Game1.Height / 2 - 174, 348, 348));
            hudWindowList[HudWindowTypes.ShipColor].AddObject(new HudList(new Rectangle(40, 40, 268, 40), "Rot: ", colorList, Game1.Config.shipColorR / 51));
            hudWindowList[HudWindowTypes.ShipColor].AddObject(new HudList(new Rectangle(40, 100, 268, 40), "Grün: ", colorList, Game1.Config.shipColorG / 51));
            hudWindowList[HudWindowTypes.ShipColor].AddObject(new HudList(new Rectangle(40, 160, 268, 40), "Blau: ", colorList, Game1.Config.shipColorB / 51));
            hudWindowList[HudWindowTypes.ShipColor].AddObject(new HudPlayer(new Vector2(144, 210)));
            hudWindowList[HudWindowTypes.ShipColor].AddObject(new HudButton(new Rectangle(40, 280, 160, 40), "Übernehmen", HudButtonAction.ApplyShipColor, new byte[] { 0, 1, 2 }));
            hudWindowList[HudWindowTypes.ShipColor].AddObject(new HudButton(new Rectangle(208, 280, 110, 40), "Zurück", HudButtonAction.OpenWindow, HudWindowTypes.Options));
            // Tastenbelegung
            hudWindowList[HudWindowTypes.GameKeys] = new HudWindow(new Rectangle(Game1.Width / 2 - 174, Game1.Height / 2 - 200, 348, 400));
            hudWindowList[HudWindowTypes.GameKeys].AddObject(new HudButton(new Rectangle(40, 20, 268, 40), "[GAMEKEY]: [LEFT]", HudButtonAction.OpenMessageBox, GameKeys.Left, true));
            hudWindowList[HudWindowTypes.GameKeys].AddObject(new HudButton(new Rectangle(40, 70, 268, 40), "[GAMEKEY]: [RIGHT]", HudButtonAction.OpenMessageBox, GameKeys.Right, true));
            hudWindowList[HudWindowTypes.GameKeys].AddObject(new HudButton(new Rectangle(40, 120, 268, 40), "[GAMEKEY]: [UP]", HudButtonAction.OpenMessageBox, GameKeys.Up, true));
            hudWindowList[HudWindowTypes.GameKeys].AddObject(new HudButton(new Rectangle(40, 170, 268, 40), "[GAMEKEY]: [DOWN]", HudButtonAction.OpenMessageBox, GameKeys.Down, true));
            hudWindowList[HudWindowTypes.GameKeys].AddObject(new HudButton(new Rectangle(40, 220, 268, 40), "[GAMEKEY]: [FIRE1]", HudButtonAction.OpenMessageBox, GameKeys.Fire1, true));
            hudWindowList[HudWindowTypes.GameKeys].AddObject(new HudButton(new Rectangle(40, 270, 268, 40), "[GAMEKEY]: [FIRE2]", HudButtonAction.OpenMessageBox, GameKeys.Fire2, true));
            hudWindowList[HudWindowTypes.GameKeys].AddObject(new HudButton(new Rectangle(40, 320, 268, 40), "Zurück", HudButtonAction.OpenWindow, HudWindowTypes.Options));
            // Credits
            hudWindowList[HudWindowTypes.Credits] = new HudWindow(new Rectangle(Game1.Width / 2 - 174, Game1.Height / 2 - 174, 348, 348));
            hudWindowList[HudWindowTypes.Credits].AddObject(new HudLabel(new Vector2(20, 40), "Hier stehen die\nMacher drin."));
            hudWindowList[HudWindowTypes.Credits].AddObject(new HudButton(new Rectangle(40, 280, 268, 40), "Zurück", HudButtonAction.OpenWindow, HudWindowTypes.MainMenu));
            // Pause
            hudWindowList[HudWindowTypes.Pause] = new HudWindow(new Rectangle(Game1.Width / 2 - 174, Game1.Height / 2 - 174, 348, 348));
            hudWindowList[HudWindowTypes.Pause].AddObject(new HudButton(new Rectangle(40, 40, 268, 40), "Fortsetzen", HudButtonAction.Continue));
            hudWindowList[HudWindowTypes.Pause].AddObject(new HudButton(new Rectangle(40, 220, 268, 40), "Hauptmenü", HudButtonAction.MainMenu));
            hudWindowList[HudWindowTypes.Pause].AddObject(new HudButton(new Rectangle(40, 280, 268, 40), "Beenden", HudButtonAction.Quit));
            // GameOver
            hudWindowList[HudWindowTypes.GameOver] = new HudWindow(new Rectangle(Game1.Width / 2 - 174, Game1.Height / 2 - 174, 348, 348));
            hudWindowList[HudWindowTypes.GameOver].AddObject(new HudLabel(new Vector2(20, 40), "GAME OVER!!!\n\nDein Schiff\nist schrott!\n\nZeit: [GAMETIME]\nAbschüsse: [SCORE]", true));
            hudWindowList[HudWindowTypes.GameOver].AddObject(new HudButton(new Rectangle(40, 280, 268, 40), "Hauptmenü", HudButtonAction.MainMenu));

            ////Message Box
            // Grafik geändert
            hudMessageBoxList[HudMessageBoxTypes.GraphicChange] = new HudMessageBox(new Rectangle(Game1.Width / 2 - 400, Game1.Height / 2 - 74, 800, 148),
                "Grafikeinstellungen übernehmen? Restliche Zeit: [TIMER]", true, true, 15, -1);
            // Tastenbelegung ändern
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
                hudWindowList[HudWindowTypes.Graphics].RefreshPosition(new Vector2(Game1.Width / 2 - 174, Game1.Height / 2 - 174));
                hudWindowList[HudWindowTypes.Sound].RefreshPosition(new Vector2(Game1.Width / 2 - 174, Game1.Height / 2 - 174));
                hudWindowList[HudWindowTypes.ShipColor].RefreshPosition(new Vector2(Game1.Width / 2 - 174, Game1.Height / 2 - 174));
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
