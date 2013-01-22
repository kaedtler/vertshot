using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VertShot.Menu
{
    public enum WindowTypes
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
    public enum MessageBoxTypes
    {
        GraphicChange,
        GameKeyChange
    }
    public enum ButtonAction
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

    static public class Menu
    {
        static public Dictionary<WindowTypes, Window> windowList = new Dictionary<WindowTypes, Window>();
        static public Dictionary<MessageBoxTypes, MessageBox> messageBoxList = new Dictionary<MessageBoxTypes, MessageBox>();

        static public void Initialize(Texture2D background, Texture2D cornerTop, Texture2D cornerBottom, Texture2D borderTop, Texture2D borderLeft)
        {
            Window.Initialize(background, cornerTop, cornerBottom, borderTop, borderLeft);
            MessageBox.Initialize(background, cornerTop, cornerBottom, borderTop, borderLeft);


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
            windowList[WindowTypes.MainMenu] = new Window(new Point(348, 348));
            windowList[WindowTypes.MainMenu].AddObject(new Button(new Rectangle(40, 40, 268, 40), "Neues Spiel", ButtonAction.NewGame));
            windowList[WindowTypes.MainMenu].AddObject(new Button(new Rectangle(40, 100, 268, 40), "Optionen", ButtonAction.OpenWindow, WindowTypes.Options));
            windowList[WindowTypes.MainMenu].AddObject(new Button(new Rectangle(40, 160, 268, 40), "Credits", ButtonAction.OpenWindow, WindowTypes.Credits));
            windowList[WindowTypes.MainMenu].AddObject(new Button(new Rectangle(40, 280, 268, 40), "Beenden", ButtonAction.Quit));
            // Optionen
            windowList[WindowTypes.Options] = new Window(new Point(348, 348));
            windowList[WindowTypes.Options].AddObject(new Button(new Rectangle(40, 40, 268, 40), "Grafikoptionen", ButtonAction.OpenWindow, WindowTypes.Graphics));
            windowList[WindowTypes.Options].AddObject(new Button(new Rectangle(40, 100, 268, 40), "Soundoptionen", ButtonAction.OpenWindow, WindowTypes.Sound));
            windowList[WindowTypes.Options].AddObject(new Button(new Rectangle(40, 160, 268, 40), "Tastenbelegung", ButtonAction.OpenWindow, WindowTypes.GameKeys));
            windowList[WindowTypes.Options].AddObject(new Button(new Rectangle(40, 220, 268, 40), "Schiffsfarbe", ButtonAction.OpenWindow, WindowTypes.ShipColor));
            windowList[WindowTypes.Options].AddObject(new Button(new Rectangle(40, 280, 268, 40), "Zurück", ButtonAction.OpenWindow, WindowTypes.MainMenu));
            // Grafikoptionen
            windowList[WindowTypes.Graphics] = new Window(new Point(348, 348));
            windowList[WindowTypes.Graphics].AddObject(new List(new Rectangle(40, 40, 268, 40), "", resList, resDefault));
            windowList[WindowTypes.Graphics].AddObject(new CheckBox(new Rectangle(40, 100, 268, 40), "Vollbild", Game1.game.IsFullScreen));
            windowList[WindowTypes.Graphics].AddObject(new Button(new Rectangle(40, 280, 160, 40), "Übernehmen", ButtonAction.ApplyGraphic, new byte[] { 0, 1 }));
            windowList[WindowTypes.Graphics].AddObject(new Button(new Rectangle(208, 280, 110, 40), "Zurück", ButtonAction.OpenWindow, WindowTypes.Options));
            // Soundoptionen
            windowList[WindowTypes.Sound] = new Window(new Point(348, 348));
            windowList[WindowTypes.Sound].AddObject(new List(new Rectangle(40, 40, 268, 40), "Sound: ", new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" }, Game1.Config.soundVol));
            windowList[WindowTypes.Sound].AddObject(new List(new Rectangle(40, 100, 268, 40), "Musik: ", new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" }, Game1.Config.musicVol));
            windowList[WindowTypes.Sound].AddObject(new CheckBox(new Rectangle(40, 160, 268, 40), "3D Sound (Beta)", Game1.Config.sound3d));
            windowList[WindowTypes.Sound].AddObject(new Label(new Vector2(20, 220), "[NOAUDIO]", true));
            windowList[WindowTypes.Sound].AddObject(new Button(new Rectangle(40, 280, 160, 40), "Übernehmen", ButtonAction.ApplySound, new byte[] { 0, 1, 2 }));
            windowList[WindowTypes.Sound].AddObject(new Button(new Rectangle(208, 280, 110, 40), "Zurück", ButtonAction.OpenWindow, WindowTypes.Options));
            // Schiffoptionen
            windowList[WindowTypes.ShipColor] = new Window(new Point(348, 348));
            windowList[WindowTypes.ShipColor].AddObject(new List(new Rectangle(40, 40, 268, 40), "Rot: ", colorList, Game1.Config.shipColorR / 51));
            windowList[WindowTypes.ShipColor].AddObject(new List(new Rectangle(40, 100, 268, 40), "Grün: ", colorList, Game1.Config.shipColorG / 51));
            windowList[WindowTypes.ShipColor].AddObject(new List(new Rectangle(40, 160, 268, 40), "Blau: ", colorList, Game1.Config.shipColorB / 51));
            windowList[WindowTypes.ShipColor].AddObject(new Player(new Vector2(144, 210)));
            windowList[WindowTypes.ShipColor].AddObject(new Button(new Rectangle(40, 280, 160, 40), "Übernehmen", ButtonAction.ApplyShipColor, new byte[] { 0, 1, 2 }));
            windowList[WindowTypes.ShipColor].AddObject(new Button(new Rectangle(208, 280, 110, 40), "Zurück", ButtonAction.OpenWindow, WindowTypes.Options));
            // Tastenbelegung
            windowList[WindowTypes.GameKeys] = new Window(new Point(348, 400));
            windowList[WindowTypes.GameKeys].AddObject(new Button(new Rectangle(40, 20, 268, 40), "[GAMEKEY]: [LEFT]", ButtonAction.OpenMessageBox, GameKeys.Left, true));
            windowList[WindowTypes.GameKeys].AddObject(new Button(new Rectangle(40, 70, 268, 40), "[GAMEKEY]: [RIGHT]", ButtonAction.OpenMessageBox, GameKeys.Right, true));
            windowList[WindowTypes.GameKeys].AddObject(new Button(new Rectangle(40, 120, 268, 40), "[GAMEKEY]: [UP]", ButtonAction.OpenMessageBox, GameKeys.Up, true));
            windowList[WindowTypes.GameKeys].AddObject(new Button(new Rectangle(40, 170, 268, 40), "[GAMEKEY]: [DOWN]", ButtonAction.OpenMessageBox, GameKeys.Down, true));
            windowList[WindowTypes.GameKeys].AddObject(new Button(new Rectangle(40, 220, 268, 40), "[GAMEKEY]: [FIRE1]", ButtonAction.OpenMessageBox, GameKeys.Fire1, true));
            windowList[WindowTypes.GameKeys].AddObject(new Button(new Rectangle(40, 270, 268, 40), "[GAMEKEY]: [FIRE2]", ButtonAction.OpenMessageBox, GameKeys.Fire2, true));
            windowList[WindowTypes.GameKeys].AddObject(new Button(new Rectangle(40, 320, 268, 40), "Zurück", ButtonAction.OpenWindow, WindowTypes.Options));
            // Credits
            windowList[WindowTypes.Credits] = new Window(new Point(600,600));
            windowList[WindowTypes.Credits].AddObject(new Label(new Vector2(20, 40), "Entwicklung, Programmierung,\nGrafik, Design, Sonstiges:\n\nOliver Kädtler\n\n\nExplosionsgrafik:\nhttp://www.nuvorm.nl/?p=1\n\nSounds:\nhttp://freesound.org/\n\nMusik:\nhttp://pacdv.com/"));
            windowList[WindowTypes.Credits].AddObject(new Button(new Rectangle(166, 540, 268, 40), "Zurück", ButtonAction.OpenWindow, WindowTypes.MainMenu));
            // Pause
            windowList[WindowTypes.Pause] = new Window(new Point(348, 348));
            windowList[WindowTypes.Pause].AddObject(new Button(new Rectangle(40, 40, 268, 40), "Fortsetzen", ButtonAction.Continue));
            windowList[WindowTypes.Pause].AddObject(new Button(new Rectangle(40, 220, 268, 40), "Hauptmenü", ButtonAction.MainMenu));
            windowList[WindowTypes.Pause].AddObject(new Button(new Rectangle(40, 280, 268, 40), "Beenden", ButtonAction.Quit));
            // GameOver
            windowList[WindowTypes.GameOver] = new Window(new Point(348, 348));
            windowList[WindowTypes.GameOver].AddObject(new Label(new Vector2(20, 40), "GAME OVER!!!\n\nDein Schiff\nist schrott!\n\nZeit: [GAMETIME]\nAbschüsse: [SCORE]", true));
            windowList[WindowTypes.GameOver].AddObject(new Button(new Rectangle(40, 280, 268, 40), "Hauptmenü", ButtonAction.MainMenu));

            ////Message Box
            // Grafik geändert
            messageBoxList[MessageBoxTypes.GraphicChange] = new MessageBox(new Rectangle(Game1.Width / 2 - 400, Game1.Height / 2 - 74, 800, 148),
                "Grafikeinstellungen übernehmen? Restliche Zeit: [TIMER]", true, true, 15, -1);
            // Tastenbelegung ändern
            messageBoxList[MessageBoxTypes.GameKeyChange] = new MessageBox(new Rectangle(Game1.Width / 2 - 300, Game1.Height / 2 - 74, 600, 148),
                "Drücke eine Taste für [GAMEKEY]", false, true, -1, -1);
        }


        static public void RefreshWindowPositions()
        {
            foreach (Window window in windowList.Values)
                window.RefreshPosition();
        }


        static public void Update(GameTime gameTime)
        {
            bool messageBoxActive = false;
            foreach (MessageBoxTypes hMessageBoxType in messageBoxList.Keys)
                if (messageBoxList[hMessageBoxType].active)
                {
                    messageBoxList[hMessageBoxType].Update(gameTime);

                    if (hMessageBoxType == MessageBoxTypes.GraphicChange)
                    {
                        if (messageBoxList[hMessageBoxType].Return == -1)
                        {
                            Game1.game.SetLastResolution();
                            messageBoxList[hMessageBoxType].active = false;
                            messageBoxActive = false;
                        }
                        else if (messageBoxList[hMessageBoxType].Return == 1)
                        {
                            messageBoxList[hMessageBoxType].active = false;
                            messageBoxActive = false;
                        }
                    }
                    else if (hMessageBoxType == MessageBoxTypes.GameKeyChange)
                    {
                        Microsoft.Xna.Framework.Input.Keys key = messageBoxList[hMessageBoxType].pressedKeyCode;
                        if (messageBoxList[hMessageBoxType].Return == -1)
                        {
                            messageBoxList[hMessageBoxType].active = false;
                            messageBoxActive = false;
                        }
                        else if (key != Microsoft.Xna.Framework.Input.Keys.None && key != Microsoft.Xna.Framework.Input.Keys.Escape)
                        {
                            Input.AssignKeyboard[(GameKeys)messageBoxList[hMessageBoxType].value] = key;
                            messageBoxList[hMessageBoxType].active = false;
                            messageBoxActive = false;
                            LoadSave.SaveConfig(Game1.Config);
                        }
                    }
                    else
                        messageBoxActive = true;
                }
            if (!messageBoxActive)
                foreach (Window hWindow in windowList.Values)
                    if (hWindow.active) hWindow.Update(gameTime);
        }

        static public void SetMessageBoxValue(MessageBoxTypes hudMessageBox, object value)
        {
            messageBoxList[hudMessageBox].value = value;
        }

        static public void ShowMessageBox(MessageBoxTypes hudMessageBox)
        {
            foreach (Window hWindow in windowList.Values)
                hWindow.Reset();
            foreach (MessageBox hMessageBox in messageBoxList.Values)
            {
                hMessageBox.Reset();
                hMessageBox.active = false;
            }
            messageBoxList[hudMessageBox].active = true;
        }

        static public void ShowWindow(WindowTypes hudWindow)
        {
            foreach (Window hWindow in windowList.Values)
            {
                hWindow.Reset();
                hWindow.active = false;
            }
            windowList[hudWindow].active = true;
        }

        static public void CloseAllWindows()
        {
            foreach (Window hWindow in windowList.Values)
            {
                hWindow.active = false;
            }
        }


        static public void Draw(SpriteBatch spriteBatch)
        {
            bool messageBoxActive = false;
            foreach (MessageBox hMessageBox in messageBoxList.Values)
                if (hMessageBox.active) { hMessageBox.Draw(spriteBatch); messageBoxActive = true; }
            if (!messageBoxActive)
                foreach (Window hWindow in windowList.Values)
                    if (hWindow.active) hWindow.Draw(spriteBatch);
        }
    }
}
