using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LD48.Framework.TextBox
{
    public static class KeyboardInput
    {
        private static Game s_Game;

        private static KeyboardState prevKeyState;

        private static Keys? repChar;
        private static DateTime downSince = DateTime.Now;
        private static float timeUntilRepInMillis;
        private static int repsPerSec;
        private static DateTime lastRep = DateTime.Now;

        public static readonly char[] s_SpecialCharacters = {
            '\a',
            '\n',
            '\r',
            '\f',
            '\t',
            '\v'
        };

        public static bool ShiftDown
        {
            get
            {
                KeyboardState state = Keyboard.GetState();
                return state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift);
            }
        }

        public static bool CtrlDown
        {
            get
            {
                KeyboardState state = Keyboard.GetState();
                return state.IsKeyDown(Keys.LeftControl) || state.IsKeyDown(Keys.RightControl);
            }
        }

        public delegate void CharEnteredHandler(object sender,
                                                CharacterEventArgs e,
                                                KeyboardState ks);

        public delegate void KeyEventHandler(object sender,
                                             KeyEventArgs e,
                                             KeyboardState ks);

        public static event CharEnteredHandler CharPressed;
        public static event KeyEventHandler KeyPressed;
        public static event KeyEventHandler KeyDown;
        public static event KeyEventHandler KeyUp;

        public static void Initialize(Game g,
                                      float timeUntilRepInMilliseconds,
                                      int repsPerSecond)
        {
            s_Game = g;
            timeUntilRepInMillis = timeUntilRepInMilliseconds;
            repsPerSec = repsPerSecond;
            s_Game.Window.TextInput += TextEntered;
        }

        public static void Update()
        {
            KeyboardState keyState = Keyboard.GetState();

            foreach (Keys key in (Keys[]) Enum.GetValues(typeof(Keys))) {
                if (JustPressed(keyState, key)) {
                    KeyDown?.Invoke(null, new KeyEventArgs(key), keyState);
                    if (KeyPressed != null) {
                        downSince = DateTime.Now;
                        repChar = key;
                        KeyPressed(null, new KeyEventArgs(key), keyState);
                    }
                } else if (JustReleased(keyState, key)) {
                    if (KeyUp != null) {
                        if (repChar == key) {
                            repChar = null;
                        }

                        KeyUp(null, new KeyEventArgs(key), keyState);
                    }
                }

                if (repChar != null && repChar == key && keyState.IsKeyDown(key)) {
                    DateTime now = DateTime.Now;
                    TimeSpan downFor = now.Subtract(downSince);
                    if (downFor.CompareTo(TimeSpan.FromMilliseconds(timeUntilRepInMillis)) > 0) {
                        // Should repeat since the wait time is over now.
                        TimeSpan repeatSince = now.Subtract(lastRep);
                        if (repeatSince.CompareTo(TimeSpan.FromMilliseconds(1000f / repsPerSec)) > 0) {
                            // Time for another key-stroke.
                            if (KeyPressed != null) {
                                lastRep = now;
                                KeyPressed(null, new KeyEventArgs(key), keyState);
                            }
                        }
                    }
                }
            }

            prevKeyState = keyState;
        }

        public static void Dispose()
        {
            CharPressed = null;
            KeyDown = null;
            KeyPressed = null;
            KeyUp = null;
        }

        private static void TextEntered(object sender,
                                        TextInputEventArgs e)
        {
            if (CharPressed != null) {
                if (!s_SpecialCharacters.Contains(e.Character)) {
                    CharPressed(null, new CharacterEventArgs(e.Character), Keyboard.GetState());
                }
            }
        }

        private static bool JustPressed(KeyboardState keyState,
                                        Keys key)
        {
            return keyState.IsKeyDown(key) && prevKeyState.IsKeyUp(key);
        }

        private static bool JustReleased(KeyboardState keyState,
                                         Keys key)
        {
            return prevKeyState.IsKeyDown(key) && keyState.IsKeyUp(key);
        }

        public class CharacterEventArgs : EventArgs
        {
            public char Character { get; private set; }

            public CharacterEventArgs(char character)
            {
                Character = character;
            }
        }

        public class KeyEventArgs : EventArgs
        {
            public Keys KeyCode { get; private set; }

            public KeyEventArgs(Keys keyCode)
            {
                KeyCode = keyCode;
            }
        }
    }
}