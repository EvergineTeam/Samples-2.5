using System;
using WaveEngine.Common.Input;
using WaveEngine.Framework.Services;

namespace Physics3DSample.Extensions
{
    public static class InputServiceExtensions
    {
        private static TimeSpan KeyDetectionMaxTime = TimeSpan.FromMilliseconds(600);

        public static bool KeyTouched(this KeyboardState keyboardState, ref KeyboardState lastKeyboardStatus, ref Keys key, ref TimeSpan gameTime, ref TimeSpan pressedTime)
        {
            var keyPressedInCurrentState = keyboardState.IsKeyPressed(key);
            var keyPressedInlastState = lastKeyboardStatus.IsKeyPressed(key);

            var keyTouched = false;
            if (!keyPressedInlastState && keyPressedInCurrentState)
            {
                pressedTime = TimeSpan.Zero;
            }
            else if (keyPressedInlastState && keyPressedInCurrentState)
            {
                pressedTime += gameTime;
            }
            else if (keyPressedInlastState && !keyPressedInCurrentState && pressedTime < KeyDetectionMaxTime)
            {
                keyTouched = true;
            }

            return keyTouched;
        }
    }
}
