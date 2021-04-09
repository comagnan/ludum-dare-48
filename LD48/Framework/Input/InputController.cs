using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace LD48.Framework.Input
{
    public class InputController
    {
        private KeyboardState m_LastKeyboardState;
        private GamePadState m_LastGamePadState;

        private KeyboardState m_KeyboardState;
        private GamePadState m_GamePadState;

        public InputController()
        {
            InitializeBuffer();
        }

        public bool IsButtonDown(CompositeInput p_Input)
        {
            return IsButtonDown(p_Input, false);
        }

        public bool IsButtonPress(CompositeInput p_Input)
        {
            return IsButtonDown(p_Input, false) && !IsButtonDown(p_Input, true);
        }

        public void UpdateState()
        {
            m_LastKeyboardState = m_KeyboardState;
            m_LastGamePadState = m_GamePadState;

            m_KeyboardState = Keyboard.GetState();
            m_GamePadState = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular);
        }

        private bool IsButtonDown(CompositeInput p_Input,
                                  bool p_UsePreviousState)
        {
            KeyboardState keyboardState = p_UsePreviousState ? m_LastKeyboardState : m_KeyboardState;
            GamePadState gamePadState = p_UsePreviousState ? m_LastGamePadState : m_GamePadState;

            return p_Input.KeyInputs.Any(key => keyboardState.IsKeyDown(key))
                || p_Input.ButtonInputs.Any(button => gamePadState.IsButtonDown(button));
        }

        private void InitializeBuffer()
        {
            UpdateState();
            UpdateState();
        }
    }
}