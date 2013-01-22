using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace VertShot
{
    public enum GameKeys
    {
        Menu,
        Left,
        Right,
        Up,
        Down,
        Fire1,
        Fire2,
        Debug1,
        Debug2,
        Debug3,
        Debug4,
        Debug5,
        Debug6,
        Debug7,
        Debug8,
        Debug9,
        Debug10,
        Debug11,
        Debug12,
        None
    }

    public enum MouseKeys
    {
        Left,
        Middle,
        Right,
        X1,
        X2
    }

    public static class Input
    {
        static float vibrateTimer = -1;

        public static GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
        public static KeyboardState keyboardState = Keyboard.GetState();
        public static MouseState mouseState = Mouse.GetState();
        public static GamePadState lastGamePadState = GamePad.GetState(PlayerIndex.One);
        public static KeyboardState lastKeyboardState = Keyboard.GetState();
        public static MouseState lastMouseState = Mouse.GetState();

        public static Dictionary<GameKeys, Keys> AssignKeyboard = new Dictionary<GameKeys, Keys>();

        public static void Initialize()
        {
            AssignKeyboard.Add(GameKeys.Menu, Keys.None);
            AssignKeyboard.Add(GameKeys.Left, Keys.None);
            AssignKeyboard.Add(GameKeys.Right, Keys.None);
            AssignKeyboard.Add(GameKeys.Up, Keys.None);
            AssignKeyboard.Add(GameKeys.Down, Keys.None);
            AssignKeyboard.Add(GameKeys.Fire1, Keys.None);
            AssignKeyboard.Add(GameKeys.Fire2, Keys.None);
            AssignKeyboard.Add(GameKeys.Debug1, Keys.None);
            AssignKeyboard.Add(GameKeys.Debug2, Keys.None);
            AssignKeyboard.Add(GameKeys.Debug3, Keys.None);
            AssignKeyboard.Add(GameKeys.Debug4, Keys.None);
            AssignKeyboard.Add(GameKeys.Debug5, Keys.None);
            AssignKeyboard.Add(GameKeys.Debug6, Keys.None);
            AssignKeyboard.Add(GameKeys.Debug7, Keys.None);
            AssignKeyboard.Add(GameKeys.Debug8, Keys.None);
            AssignKeyboard.Add(GameKeys.Debug9, Keys.None);
            AssignKeyboard.Add(GameKeys.Debug10, Keys.None);
            AssignKeyboard.Add(GameKeys.Debug11, Keys.None);
            AssignKeyboard.Add(GameKeys.Debug12, Keys.None);
        }

        public static void UpdateBegin(GameTime gameTime)
        {
            gamePadState = GamePad.GetState(PlayerIndex.One);
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            if (Game1.game.IsMouseVisible && keyboardState.GetPressedKeys().Length != 0)
                Game1.game.IsMouseVisible = false;
            else if (!Game1.game.IsMouseVisible && mouseState.X - lastMouseState.X != 0 || mouseState.Y - lastMouseState.Y != 0)
                Game1.game.IsMouseVisible = true;

            vibrateTimer = MathHelper.Max(0, vibrateTimer - (float)gameTime.ElapsedGameTime.TotalSeconds);
            if (vibrateTimer == 0)
            {
                GamePad.SetVibration(PlayerIndex.One, 0, 0);
                vibrateTimer = -1;
            }
        }

        public static void UpdateEnd()
        {
            lastGamePadState = GamePad.GetState(PlayerIndex.One);
            lastKeyboardState = Keyboard.GetState();
            lastMouseState = Mouse.GetState();
        }

        public static Keys GetPressedKeyCode()
        {
            Keys[] keys = keyboardState.GetPressedKeys();
            if (keys.Length > 0)
                return keys[0];
            else
                return Keys.None;
        }

        public static string GetGameKeyCodeString(GameKeys gameKey)
        {
            switch (AssignKeyboard[gameKey])
            {
                case Keys.Left: return "Links";
                case Keys.Right: return "Rechts";
                case Keys.Up: return "Oben";
                case Keys.Down: return "Unten";
                case Keys.LeftAlt: return "AltLinks";
                case Keys.LeftControl: return "StrgLinks";
                case Keys.LeftShift: return "ShiftLinks";
                case Keys.RightAlt: return "AltRechts";
                case Keys.RightControl: return "StrgRechts";
                case Keys.RightShift: return "ShiftRechts";
                default: return AssignKeyboard[gameKey].ToString();
            }
        }

        public static string GetGameKeyString(GameKeys gameKey)
        {
            switch (gameKey)
            {
                case GameKeys.Left: return "Links";
                case GameKeys.Right: return "Rechts";
                case GameKeys.Up: return "Oben";
                case GameKeys.Down: return "Unten";
                case GameKeys.Fire1: return "Feuer1";
                case GameKeys.Fire2: return "Feuer2";
                default: return gameKey.ToString();
            }
        }

        public static Vector2 InputVector
        {
            get
            {
                Vector2 inputVector = Vector2.Zero;
                if (GamePadConnected)
                {
                    inputVector.X = gamePadState.ThumbSticks.Left.X;
                    inputVector.Y = -gamePadState.ThumbSticks.Left.Y;
                }
                else
                {
                    if (keyboardState.IsKeyDown(AssignKeyboard[GameKeys.Left])) inputVector.X += -1;
                    if (keyboardState.IsKeyDown(AssignKeyboard[GameKeys.Right])) inputVector.X += 1;
                    if (keyboardState.IsKeyDown(AssignKeyboard[GameKeys.Up])) inputVector.Y += -1;
                    if (keyboardState.IsKeyDown(AssignKeyboard[GameKeys.Down])) inputVector.Y += 1;
                }
                return inputVector;
            }
        }

        public static Point MousePoint
        {
            get
            {
                return new Point(Mouse.GetState().X, Mouse.GetState().Y);
            }
        }

        public static Point MouseScaledPoint
        {
            get
            {
                Vector2 vector = Vector2.Transform(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Game1.mouseMatrix);
                return new Point((int)vector.X, (int)vector.Y);
            }
        }

        public static Vector2 MouseVector
        {
            get
            {
                return new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            }
        }

        public static Vector2 MouseScaledVector
        {
            get
            {
                return Vector2.Transform(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Game1.scaleMatrix * Game1.transMatrix);
            }
        }

        public static bool GamePadConnected { get { return gamePadState.IsConnected; } }

        public static void Vibrate(float seconds, float leftMotor, float rightMotor)
        {
            if (GamePadConnected)
            {
                vibrateTimer = seconds;
                GamePad.SetVibration(PlayerIndex.One, leftMotor, rightMotor);
            }
        }

        public static bool IsGameKeyDown(GameKeys gameKey)
        {
            switch (gameKey)
            {
                case GameKeys.Menu:
                    if (GamePadConnected)
                        return gamePadState.IsButtonDown(Buttons.Start);
                    else
                        return keyboardState.IsKeyDown(AssignKeyboard[GameKeys.Menu]);

                case GameKeys.Fire1:
                    if (GamePadConnected)
                        return gamePadState.IsButtonDown(Buttons.A);
                    else
                        return keyboardState.IsKeyDown(AssignKeyboard[GameKeys.Fire1]);

                case GameKeys.Fire2:
                    if (GamePadConnected)
                        return gamePadState.IsButtonDown(Buttons.X);
                    else
                        return keyboardState.IsKeyDown(AssignKeyboard[GameKeys.Fire2]);

                case GameKeys.Debug1: return keyboardState.IsKeyDown(AssignKeyboard[GameKeys.Debug1]);
                case GameKeys.Debug2: return keyboardState.IsKeyDown(AssignKeyboard[GameKeys.Debug2]);
                case GameKeys.Debug3: return keyboardState.IsKeyDown(AssignKeyboard[GameKeys.Debug3]);
                case GameKeys.Debug4: return keyboardState.IsKeyDown(AssignKeyboard[GameKeys.Debug4]);
                case GameKeys.Debug5: return keyboardState.IsKeyDown(AssignKeyboard[GameKeys.Debug5]);
                case GameKeys.Debug6: return keyboardState.IsKeyDown(AssignKeyboard[GameKeys.Debug6]);
                case GameKeys.Debug7: return keyboardState.IsKeyDown(AssignKeyboard[GameKeys.Debug7]);
                case GameKeys.Debug8: return keyboardState.IsKeyDown(AssignKeyboard[GameKeys.Debug8]);
                case GameKeys.Debug9: return keyboardState.IsKeyDown(AssignKeyboard[GameKeys.Debug9]);
                case GameKeys.Debug10: return keyboardState.IsKeyDown(AssignKeyboard[GameKeys.Debug10]);
                case GameKeys.Debug11: return keyboardState.IsKeyDown(AssignKeyboard[GameKeys.Debug11]);
                case GameKeys.Debug12: return keyboardState.IsKeyDown(AssignKeyboard[GameKeys.Debug12]);
                default:
                    return false;
            }
        }

        public static bool IsGameKeyReleased(GameKeys gameKey)
        {
            switch (gameKey)
            {
                case GameKeys.Menu:
                    if (GamePadConnected)
                        return gamePadState.IsButtonUp(Buttons.Start) && lastGamePadState.IsButtonDown(Buttons.Start);
                    else
                        return keyboardState.IsKeyUp(AssignKeyboard[GameKeys.Menu]) && lastKeyboardState.IsKeyDown(AssignKeyboard[GameKeys.Menu]);

                case GameKeys.Fire1:
                    if (GamePadConnected)
                        return gamePadState.IsButtonUp(Buttons.A) && lastGamePadState.IsButtonDown(Buttons.A);
                    else
                        return keyboardState.IsKeyUp(AssignKeyboard[GameKeys.Fire1]) && lastKeyboardState.IsKeyDown(AssignKeyboard[GameKeys.Fire1]);

                case GameKeys.Fire2:
                    if (GamePadConnected)
                        return gamePadState.IsButtonUp(Buttons.X) && lastGamePadState.IsButtonDown(Buttons.X);
                    else
                        return keyboardState.IsKeyUp(AssignKeyboard[GameKeys.Fire2]) && lastKeyboardState.IsKeyDown(AssignKeyboard[GameKeys.Fire2]);

                case GameKeys.Debug1: return keyboardState.IsKeyUp(AssignKeyboard[GameKeys.Debug1]) && lastKeyboardState.IsKeyDown(AssignKeyboard[GameKeys.Debug1]);
                case GameKeys.Debug2: return keyboardState.IsKeyUp(AssignKeyboard[GameKeys.Debug2]) && lastKeyboardState.IsKeyDown(AssignKeyboard[GameKeys.Debug2]);
                case GameKeys.Debug3: return keyboardState.IsKeyUp(AssignKeyboard[GameKeys.Debug3]) && lastKeyboardState.IsKeyDown(AssignKeyboard[GameKeys.Debug3]);
                case GameKeys.Debug4: return keyboardState.IsKeyUp(AssignKeyboard[GameKeys.Debug4]) && lastKeyboardState.IsKeyDown(AssignKeyboard[GameKeys.Debug4]);
                case GameKeys.Debug5: return keyboardState.IsKeyUp(AssignKeyboard[GameKeys.Debug5]) && lastKeyboardState.IsKeyDown(AssignKeyboard[GameKeys.Debug5]);
                case GameKeys.Debug6: return keyboardState.IsKeyUp(AssignKeyboard[GameKeys.Debug6]) && lastKeyboardState.IsKeyDown(AssignKeyboard[GameKeys.Debug6]);
                case GameKeys.Debug7: return keyboardState.IsKeyUp(AssignKeyboard[GameKeys.Debug7]) && lastKeyboardState.IsKeyDown(AssignKeyboard[GameKeys.Debug7]);
                case GameKeys.Debug8: return keyboardState.IsKeyUp(AssignKeyboard[GameKeys.Debug8]) && lastKeyboardState.IsKeyDown(AssignKeyboard[GameKeys.Debug8]);
                case GameKeys.Debug9: return keyboardState.IsKeyUp(AssignKeyboard[GameKeys.Debug9]) && lastKeyboardState.IsKeyDown(AssignKeyboard[GameKeys.Debug9]);
                case GameKeys.Debug10: return keyboardState.IsKeyUp(AssignKeyboard[GameKeys.Debug10]) && lastKeyboardState.IsKeyDown(AssignKeyboard[GameKeys.Debug10]);
                case GameKeys.Debug11: return keyboardState.IsKeyUp(AssignKeyboard[GameKeys.Debug11]) && lastKeyboardState.IsKeyDown(AssignKeyboard[GameKeys.Debug11]);
                case GameKeys.Debug12: return keyboardState.IsKeyUp(AssignKeyboard[GameKeys.Debug12]) && lastKeyboardState.IsKeyDown(AssignKeyboard[GameKeys.Debug12]);
                default:
                    return false;
            }
        }


        public static bool IsMouseKeyDown(MouseKeys mouseKey)
        {
            switch (mouseKey)
            {
                case MouseKeys.Left: return mouseState.LeftButton == ButtonState.Pressed;
                case MouseKeys.Middle: return mouseState.MiddleButton == ButtonState.Pressed;
                case MouseKeys.Right: return mouseState.RightButton == ButtonState.Pressed;
                case MouseKeys.X1: return mouseState.XButton1 == ButtonState.Pressed;
                case MouseKeys.X2: return mouseState.XButton2 == ButtonState.Pressed;
                default: return false;
            }
        }

        public static bool IsMouseKeyReleased(MouseKeys mouseKey)
        {
            switch (mouseKey)
            {
                case MouseKeys.Left: return mouseState.LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed;
                case MouseKeys.Middle: return mouseState.MiddleButton == ButtonState.Released && lastMouseState.MiddleButton == ButtonState.Pressed;
                case MouseKeys.Right: return mouseState.RightButton == ButtonState.Released && lastMouseState.RightButton == ButtonState.Pressed;
                case MouseKeys.X1: return mouseState.XButton1 == ButtonState.Released && lastMouseState.XButton1 == ButtonState.Pressed;
                case MouseKeys.X2: return mouseState.XButton2 == ButtonState.Released && lastMouseState.XButton2 == ButtonState.Pressed;
                default: return false;
            }
        }

        public static void RefreshMouseInput()
        {
            mouseState = Mouse.GetState();
            lastMouseState = Mouse.GetState();
        }

        public static void RefreshGameKeyInput()
        {
            gamePadState = GamePad.GetState(PlayerIndex.One);
            lastGamePadState = GamePad.GetState(PlayerIndex.One);
            keyboardState = Keyboard.GetState();
            lastKeyboardState = Keyboard.GetState();
        }
    }
}
