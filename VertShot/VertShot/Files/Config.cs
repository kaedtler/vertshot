using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace VertShot.Files
{
    [Serializable]
    public class Config
    {
        public const byte FileVersion = 1;

        public byte fileVersion = FileVersion;
        public short resWitdh;
        public short resHeight;
        public bool fullscreen;

        public byte musicVol;
        public byte soundVol;

        public Dictionary<GameKeys, Keys> assignKeyboard = new Dictionary<GameKeys, Keys>();

        public Config()
        {
            // Config Standardwerte

            resWitdh = 800;
            resHeight = 600;
            fullscreen = false;

            musicVol = 10;
            soundVol = 10;

            assignKeyboard[GameKeys.Menu] = Keys.Escape;
            assignKeyboard[GameKeys.Left] = Keys.Left;
            assignKeyboard[GameKeys.Right] = Keys.Right;
            assignKeyboard[GameKeys.Up] = Keys.Up;
            assignKeyboard[GameKeys.Down] = Keys.Down;
            assignKeyboard[GameKeys.Fire1] = Keys.Space;
            assignKeyboard[GameKeys.Fire2] = Keys.LeftControl;
            assignKeyboard[GameKeys.Debug1] = Keys.F1;
            assignKeyboard[GameKeys.Debug2] = Keys.F2;
            assignKeyboard[GameKeys.Debug3] = Keys.F3;
            assignKeyboard[GameKeys.Debug4] = Keys.F4;
            assignKeyboard[GameKeys.Debug5] = Keys.F5;
            assignKeyboard[GameKeys.Debug6] = Keys.F6;
            assignKeyboard[GameKeys.Debug7] = Keys.F7;
            assignKeyboard[GameKeys.Debug8] = Keys.F8;
            assignKeyboard[GameKeys.Debug9] = Keys.F9;
            assignKeyboard[GameKeys.Debug10] = Keys.F10;
            assignKeyboard[GameKeys.Debug11] = Keys.F11;
            assignKeyboard[GameKeys.Debug12] = Keys.F12;
        }

        public bool Upgrade()
        {
            switch (fileVersion)
            {
                case 0:
                    fileVersion = FileVersion;
                    musicVol = 10;
                    soundVol = 10;
                    return true;
            }
            return false;
        }
    }
}
