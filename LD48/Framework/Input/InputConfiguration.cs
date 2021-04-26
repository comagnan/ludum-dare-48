using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace LD48.Framework.Input
{
    public static class InputConfiguration
    {
        public static CompositeInput Up =>
            new CompositeInput {
                ButtonInputs = new List<Buttons> {
                    Buttons.LeftThumbstickUp,
                    Buttons.DPadUp
                },
                KeyInputs = new List<Keys> { Keys.Up }
            };

        public static CompositeInput Down =>
            new CompositeInput {
                ButtonInputs = new List<Buttons> {
                    Buttons.LeftThumbstickDown,
                    Buttons.DPadDown
                },
                KeyInputs = new List<Keys> { Keys.Down }
            };

        public static CompositeInput Left =>
            new CompositeInput {
                ButtonInputs = new List<Buttons> {
                    Buttons.LeftThumbstickLeft,
                    Buttons.DPadLeft
                },
                KeyInputs = new List<Keys> { Keys.Left }
            };

        public static CompositeInput Right =>
            new CompositeInput {
                ButtonInputs = new List<Buttons> {
                    Buttons.LeftThumbstickRight,
                    Buttons.DPadRight
                },
                KeyInputs = new List<Keys> { Keys.Right }
            };

        public static CompositeInput Confirm =>
            new CompositeInput {
                ButtonInputs = new List<Buttons> { Buttons.A },
                KeyInputs = new List<Keys> { Keys.X }
            };

        public static CompositeInput Submit =>
            new CompositeInput {
                ButtonInputs = new List<Buttons> { Buttons.A },
                KeyInputs = new List<Keys> {
                    Keys.X,
                    Keys.Enter
                }
            };

        public static CompositeInput Return =>
            new() {
                ButtonInputs = new List<Buttons> { Buttons.B },
                KeyInputs = new List<Keys> { Keys.C }
            };

        public static CompositeInput Exit =>
            new CompositeInput {
                ButtonInputs = new List<Buttons> { Buttons.Back },
                KeyInputs = new List<Keys> { Keys.Escape }
            };

        public static CompositeInput Pause =>
            new CompositeInput {
                ButtonInputs = new List<Buttons> { Buttons.Start },
                KeyInputs = new List<Keys> {
                    Keys.P
                }
            };

        public static CompositeInput FullScreen =>
            new CompositeInput {
                ButtonInputs = new List<Buttons> { Buttons.Back },
                KeyInputs = new List<Keys> {
                    Keys.F
                }
            };

        public static CompositeInput BassTwitter =>
            new CompositeInput {
                ButtonInputs = new List<Buttons> { Buttons.LeftShoulder },
                KeyInputs = new List<Keys> { Keys.NumPad1, Keys.D1 }
            };

        public static CompositeInput TriggerTwitter =>
            new CompositeInput {
                ButtonInputs = new List<Buttons> { Buttons.RightShoulder },
                KeyInputs = new List<Keys> { Keys.NumPad2, Keys.D2 }
            };

        public static CompositeInput AlphadeusBandcamp =>
            new CompositeInput {
                ButtonInputs = new List<Buttons> { Buttons.RightTrigger },
                KeyInputs = new List<Keys> { Keys.NumPad3, Keys.D3 }
            };
    }
}