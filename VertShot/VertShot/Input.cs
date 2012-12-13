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
        Fire,
        Debug1
    }

    public static class Input
    {
        public static GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
        public static KeyboardState keyboardState = Keyboard.GetState();
        public static GamePadState lastGamePadState = GamePad.GetState(PlayerIndex.One);
        public static KeyboardState lastKeyboardState = Keyboard.GetState();

        public static Dictionary<GameKeys, Keys> AssignKeyboard = new Dictionary<GameKeys, Keys>();

        public static void Initialize()
        {
            AssignKeyboard.Add(GameKeys.Menu, Keys.None);
            AssignKeyboard.Add(GameKeys.Left, Keys.None);
            AssignKeyboard.Add(GameKeys.Right, Keys.None);
            AssignKeyboard.Add(GameKeys.Up, Keys.None);
            AssignKeyboard.Add(GameKeys.Down, Keys.None);
            AssignKeyboard.Add(GameKeys.Fire, Keys.None);
            AssignKeyboard.Add(GameKeys.Debug1, Keys.None);
        }

        public static void UpdateBegin()
        {
            gamePadState = GamePad.GetState(PlayerIndex.One);
            keyboardState = Keyboard.GetState();
        }

        public static void UpdateEnd()
        {
            lastGamePadState = GamePad.GetState(PlayerIndex.One);
            lastKeyboardState = Keyboard.GetState();
        }

        public static Vector2 InputVector
        {
            get
            {
                Vector2 inputVector = Vector2.Zero;
                if (keyboardState.IsKeyDown(AssignKeyboard[GameKeys.Left])) inputVector.X += -1;
                if (keyboardState.IsKeyDown(AssignKeyboard[GameKeys.Right])) inputVector.X += 1;
                if (keyboardState.IsKeyDown(AssignKeyboard[GameKeys.Up])) inputVector.Y += -1;
                if (keyboardState.IsKeyDown(AssignKeyboard[GameKeys.Down])) inputVector.Y += 1;
                return inputVector;
            }
        }

        public static bool GamePadConnected
        {
            get
            {
                return gamePadState.IsConnected;
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

                case GameKeys.Fire:
                    if (GamePadConnected)
                        return gamePadState.IsButtonDown(Buttons.A);
                    else
                        return keyboardState.IsKeyDown(AssignKeyboard[GameKeys.Fire]);

                case GameKeys.Debug1:
                    return keyboardState.IsKeyDown(AssignKeyboard[GameKeys.Debug1]);
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

                case GameKeys.Fire:
                    if (GamePadConnected)
                        return gamePadState.IsButtonUp(Buttons.A) && lastGamePadState.IsButtonDown(Buttons.A);
                    else
                        return keyboardState.IsKeyUp(AssignKeyboard[GameKeys.Fire]) && lastKeyboardState.IsKeyDown(AssignKeyboard[GameKeys.Fire]);

                case GameKeys.Debug1:
                    return keyboardState.IsKeyUp(AssignKeyboard[GameKeys.Debug1]) && lastKeyboardState.IsKeyDown(AssignKeyboard[GameKeys.Debug1]);
                default:
                    return false;
            }
        }
    }
}
