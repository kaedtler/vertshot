using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace VertShot
{
    public static class Input
    {
        public static GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
        public static KeyboardState keyboardState = Keyboard.GetState();
        public static GamePadState lastGamePadState = GamePad.GetState(PlayerIndex.One);
        public static KeyboardState lastKeyboardState = Keyboard.GetState();

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
                if (keyboardState.IsKeyDown(Keys.Left)) inputVector.X += -1;
                if (keyboardState.IsKeyDown(Keys.Right)) inputVector.X += 1;
                if (keyboardState.IsKeyDown(Keys.Up)) inputVector.Y += -1;
                if (keyboardState.IsKeyDown(Keys.Down)) inputVector.Y += 1;
                return inputVector;
            }
        }
    }
}
