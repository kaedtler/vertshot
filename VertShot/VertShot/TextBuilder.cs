using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VertShot
{
    static public class TextBuilder
    {
        static public string ReplacePlaceholder(string text, GameKeys gameKey = GameKeys.None, int timerSeconds = 0)
        {
            text = text.Replace("[LEFT]", Input.GetGameKeyCodeString(GameKeys.Left));
            text = text.Replace("[RIGHT]", Input.GetGameKeyCodeString(GameKeys.Right));
            text = text.Replace("[UP]", Input.GetGameKeyCodeString(GameKeys.Up));
            text = text.Replace("[DOWN]", Input.GetGameKeyCodeString(GameKeys.Down));
            text = text.Replace("[FIRE1]", Input.GetGameKeyCodeString(GameKeys.Fire1));
            text = text.Replace("[FIRE2]", Input.GetGameKeyCodeString(GameKeys.Fire2));
            text = text.Replace("[GAMEKEY]", Input.GetGameKeyString((GameKeys)gameKey));
            text = text.Replace("[TIMER]", timerSeconds.ToString());
            text = text.Replace("[SCORE]", Game1.enemyCounter.ToString());
            return text;
        }
    }
}
