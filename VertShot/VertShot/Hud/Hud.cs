using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VertShot
{
    public enum HudWindows
    {
        MainMenu,
        Options,
        Credits,
        GameKeys,
        GameOver,
        Pause
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
        Quit
    }

    static public class Hud
    {
        static public Dictionary<HudWindows, HudWindow> hudWindowList = new Dictionary<HudWindows, HudWindow>();

        static public void Initialize(Texture2D background, Texture2D cornerTop, Texture2D cornerBottom, Texture2D borderTop, Texture2D borderLeft)
        {
            HudWindow.Initialize(background, cornerTop, cornerBottom, borderTop, borderLeft);


            // Auflösungsliste holen
            List<string> resList = new List<string>();
            int resDefault = 0;
            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                if (!resList.Contains(mode.Width + "x" + mode.Height) && mode.Width >= 800 && mode.Height >= 600)
                {
                    resList.Add(mode.Width + "x" + mode.Height);
                    if (mode.Width == Game1.GraphicWidth && mode.Height == Game1.GraphicHeight)
                        resDefault = resList.Count - 1;
                }
            }


            // Hauptmenü
            hudWindowList[HudWindows.MainMenu] = new HudWindow(new Rectangle(Game1.GraphicWidth / 2 - 174, Game1.GraphicHeight / 2 - 174, 348, 348));
            hudWindowList[HudWindows.MainMenu].AddButton(new Rectangle(40, 40, 268, 40), "Neues Spiel", hudButtonAction.NewGame);
            hudWindowList[HudWindows.MainMenu].AddButton(new Rectangle(40, 100, 268, 40), "Optionen", hudButtonAction.OpenWindow, HudWindows.Options);
            hudWindowList[HudWindows.MainMenu].AddButton(new Rectangle(40, 160, 268, 40), "Credits", hudButtonAction.OpenWindow, HudWindows.Credits);
            hudWindowList[HudWindows.MainMenu].AddButton(new Rectangle(40, 280, 268, 40), "Beenden", hudButtonAction.Quit);
            // Optionen
            hudWindowList[HudWindows.Options] = new HudWindow(new Rectangle(Game1.GraphicWidth / 2 - 174, Game1.GraphicHeight / 2 - 174, 348, 348));
            hudWindowList[HudWindows.Options].AddList(new Rectangle(40, 40, 268, 40), resList, resDefault);
            hudWindowList[HudWindows.Options].AddCheckBox(new Rectangle(40, 100, 268, 40), "Vollbild", Game1.game.IsFullScreen);
            hudWindowList[HudWindows.Options].AddButton(new Rectangle(40, 160, 268, 40), "Tastenbelegung", hudButtonAction.OpenWindow, HudWindows.GameKeys);
            hudWindowList[HudWindows.Options].AddButton(new Rectangle(40, 220, 268, 40), "Übernehmen", hudButtonAction.ApplyGraphic, new int[]{0,0});
            hudWindowList[HudWindows.Options].AddButton(new Rectangle(40, 280, 268, 40), "Zurück", hudButtonAction.OpenWindow, HudWindows.MainMenu);
            // Tastenbelegung TEMP
            hudWindowList[HudWindows.GameKeys] = new HudWindow(new Rectangle(Game1.GraphicWidth / 2 - 174, Game1.GraphicHeight / 2 - 174, 348, 348));
            hudWindowList[HudWindows.GameKeys].AddLabel(new Vector2(20, 40), "Pfeiltasten: bewegen\nLeertaste:   Feuer 1\nStrg Rechts: Feuer 2");
            hudWindowList[HudWindows.GameKeys].AddButton(new Rectangle(40, 280, 268, 40), "Zurück", hudButtonAction.OpenWindow, HudWindows.Options);
            // Credits
            hudWindowList[HudWindows.Credits] = new HudWindow(new Rectangle(Game1.GraphicWidth / 2 - 174, Game1.GraphicHeight / 2 - 174, 348, 348));
            hudWindowList[HudWindows.Credits].AddLabel(new Vector2(20, 40), "Hier stehen die\nMacher drin.");
            hudWindowList[HudWindows.Credits].AddButton(new Rectangle(40, 280, 268, 40), "Zurück", hudButtonAction.OpenWindow, HudWindows.Options);
            // Pause
            hudWindowList[HudWindows.Pause] = new HudWindow(new Rectangle(Game1.GraphicWidth / 2 - 174, Game1.GraphicHeight / 2 - 174, 348, 348));
            hudWindowList[HudWindows.Pause].AddButton(new Rectangle(40, 40, 268, 40), "Fortsetzen", hudButtonAction.Continue);
            hudWindowList[HudWindows.Pause].AddButton(new Rectangle(40, 220, 268, 40), "Hauptmenü", hudButtonAction.MainMenu);
            hudWindowList[HudWindows.Pause].AddButton(new Rectangle(40, 280, 268, 40), "Beenden", hudButtonAction.Quit);
            // GameOver
            hudWindowList[HudWindows.GameOver] = new HudWindow(new Rectangle(Game1.GraphicWidth / 2 - 174, Game1.GraphicHeight / 2 - 174, 348, 348));
            hudWindowList[HudWindows.GameOver].AddLabel(new Vector2(20, 40), "GAME OVER!!!\n\nDein Schiff\nist schrott!\n\nAbschüsse: $SCORE$");
            hudWindowList[HudWindows.GameOver].AddButton(new Rectangle(40, 280, 268, 40), "Hauptmenü", hudButtonAction.MainMenu);
        }


        static public void RefreshWindowPositions()
        {
            if (hudWindowList.Count > 0)
            {
                hudWindowList[HudWindows.MainMenu].RefreshPosition(new Vector2(Game1.GraphicWidth / 2 - 174, Game1.GraphicHeight / 2 - 174));
                hudWindowList[HudWindows.Options].RefreshPosition(new Vector2(Game1.GraphicWidth / 2 - 174, Game1.GraphicHeight / 2 - 174));
                hudWindowList[HudWindows.GameKeys].RefreshPosition(new Vector2(Game1.GraphicWidth / 2 - 174, Game1.GraphicHeight / 2 - 174));
                hudWindowList[HudWindows.Credits].RefreshPosition(new Vector2(Game1.GraphicWidth / 2 - 174, Game1.GraphicHeight / 2 - 174));
                hudWindowList[HudWindows.Pause].RefreshPosition(new Vector2(Game1.GraphicWidth / 2 - 174, Game1.GraphicHeight / 2 - 174));
                hudWindowList[HudWindows.GameOver].RefreshPosition(new Vector2(Game1.GraphicWidth / 2 - 174, Game1.GraphicHeight / 2 - 174));
            }
        }


        static public void Update(GameTime gameTime)
        {
            foreach (HudWindow hWindow in hudWindowList.Values)
                if (hWindow.IsActive) hWindow.Update(gameTime);
        }

        static public void ShowWindow(HudWindows hudWindow)
        {
            foreach (HudWindow hWindow in hudWindowList.Values)
            {
                hWindow.IsActive = false;
            }
            hudWindowList[hudWindow].IsActive = true;
        }

        static public void CloseAllWindows()
        {
            foreach (HudWindow hWindow in hudWindowList.Values)
            {
                hWindow.IsActive = false;
            }
        }


        static public void Draw(SpriteBatch spriteBatch)
        {
            foreach (HudWindow hWindow in hudWindowList.Values)
                if (hWindow.IsActive) hWindow.Draw(spriteBatch);
        }
    }
}
