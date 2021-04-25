using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LD48.Framework.TextBox
{
    public class Box
    {
        private string m_Clipboard;
        
        private static readonly List<char> s_AcceptedCharacters = new () {
            '0',
            '1',
            '2',
            '3',
            '4',
            '5',
            '6',
            '7',
            '8',
            '9',
            '(',
            ')',
            '/',
            '*',
            '-',
            '+',
            ' '
        };

        public readonly Text Text;
        public readonly TextRenderer Renderer;
        public readonly Cursor Cursor;

        private SoundEffect m_KeyPressEffect;
        private SoundEffect m_RemoveEffect;
        public GraphicsDevice GraphicsDevice { get; set; }

        public Rectangle Area
        {
            get
            {
                return Renderer.Area;
            }
            set
            {
                Renderer.Area = value;
            }
        }

        public bool Active = true;

        public Box(Rectangle area,
                   int maxCharacters,
                   string text,
                   GraphicsDevice graphicsDevice,
                   SpriteFont spriteFont,
                   Color cursorColor,
                   Color selectionColor,
                   int ticksPerToggle,
                   SoundEffect p_AddSoundEffect,
                   SoundEffect p_RemoveSoundEffect)
        {
            GraphicsDevice = graphicsDevice;

            Text = new Text(maxCharacters) { String = text };

            Renderer = new TextRenderer(this) {
                Area = area,
                Font = spriteFont,
                Color = Color.White
            };

            Cursor = new Cursor(this, cursorColor, selectionColor, new Rectangle(0, 0, 1, 1), ticksPerToggle);

            KeyboardInput.CharPressed += CharacterTyped;
            KeyboardInput.KeyPressed += KeyPressed;

            m_KeyPressEffect = p_AddSoundEffect;
            m_RemoveEffect = p_RemoveSoundEffect;
        }

        public void Dispose()
        {
            KeyboardInput.Dispose();
        }

        public void Update()
        {
            Renderer.Update();
            Cursor.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Renderer.Draw(spriteBatch);
            if (Active) {
                Cursor.Draw(spriteBatch);
            }
        }

        public static bool IsLegalCharacter(SpriteFont font,
                                            char c)
        {
            return s_AcceptedCharacters.Contains(c) || c == '\r' || c == '\n';
        }

        public static int IndexOfNextCharAfterWhitespace(int pos,
                                                         char[] characters)
        {
            char[] chars = characters;
            char c = chars[pos];
            bool whiteSpaceFound = false;
            while (true) {
                if (c.Equals(' ')) {
                    whiteSpaceFound = true;
                } else if (whiteSpaceFound) {
                    return pos;
                }

                ++pos;
                if (pos >= chars.Length) {
                    return chars.Length;
                }

                c = chars[pos];
            }
        }

        public static int IndexOfLastCharBeforeWhitespace(int pos,
                                                          char[] characters)
        {
            char[] chars = characters;

            bool charFound = false;
            while (true) {
                --pos;
                if (pos <= 0) {
                    return 0;
                }

                char c = chars[pos];

                if (c.Equals(' ')) {
                    if (charFound) {
                        return ++pos;
                    }
                } else {
                    charFound = true;
                }
            }
        }

        private void KeyPressed(object sender,
                                KeyboardInput.KeyEventArgs e,
                                KeyboardState ks)
        {
            if (Active) {
                int oldPos = Cursor.TextCursor;
                switch (e.KeyCode) {
                    case Keys.Left:
                        if (KeyboardInput.CtrlDown) {
                            Cursor.TextCursor = IndexOfLastCharBeforeWhitespace(Cursor.TextCursor, Text.Characters);
                        } else {
                            Cursor.TextCursor--;
                        }

                        ShiftMod(oldPos);
                        break;
                    case Keys.Right:
                        if (KeyboardInput.CtrlDown) {
                            Cursor.TextCursor = IndexOfNextCharAfterWhitespace(Cursor.TextCursor, Text.Characters);
                        } else {
                            Cursor.TextCursor++;
                        }

                        ShiftMod(oldPos);
                        break;
                    case Keys.Home:
                        Cursor.TextCursor = 0;
                        ShiftMod(oldPos);
                        break;
                    case Keys.End:
                        Cursor.TextCursor = Text.Length;
                        ShiftMod(oldPos);
                        break;
                    case Keys.Delete:
                        if (DelSelection() == null && Cursor.TextCursor < Text.Length) {
                            m_RemoveEffect.Play();
                            Text.RemoveCharacters(Cursor.TextCursor, Cursor.TextCursor + 1);
                        }

                        break;
                    case Keys.Back:
                        if (DelSelection() == null && Cursor.TextCursor > 0) {
                            m_RemoveEffect.Play();
                            Text.RemoveCharacters(Cursor.TextCursor - 1, Cursor.TextCursor);
                            Cursor.TextCursor--;
                        }

                        break;
                    case Keys.A:
                        if (KeyboardInput.CtrlDown) {
                            if (Text.Length > 0) {
                                Cursor.SelectedChar = 0;
                                Cursor.TextCursor = Text.Length;
                            }
                        }

                        break;
                    case Keys.C:
                        if (KeyboardInput.CtrlDown) {
                            m_Clipboard = DelSelection(true);
                        }

                        break;
                    case Keys.X:
                        if (KeyboardInput.CtrlDown) {
                            if (Cursor.SelectedChar.HasValue) {
                                m_Clipboard = DelSelection();
                            }
                        }

                        break;
                    case Keys.V:
                        if (KeyboardInput.CtrlDown) {
                            if (m_Clipboard != null) {
                                DelSelection();
                                foreach (char c in m_Clipboard) {
                                    if (Text.Length < Text.MaxLength) {
                                        Text.InsertCharacter(Cursor.TextCursor, c);
                                        Cursor.TextCursor++;
                                    }
                                }
                            }
                        }

                        break;
                }
            }
        }

        private void ShiftMod(int oldPos)
        {
            if (KeyboardInput.ShiftDown) {
                if (Cursor.SelectedChar == null) {
                    Cursor.SelectedChar = oldPos;
                }
            } else {
                Cursor.SelectedChar = null;
            }
        }

        private void CharacterTyped(object p_Sender,
                                    KeyboardInput.CharacterEventArgs p_E,
                                    KeyboardState p_Ks)
        {
            if (Active && !KeyboardInput.CtrlDown) {
                if (IsLegalCharacter(Renderer.Font, p_E.Character) && !p_E.Character.Equals('\r') && !p_E.Character.Equals('\n')) {
                    DelSelection();
                    if (Text.Length < Text.MaxLength) {
                        m_KeyPressEffect.Play();
                        Text.InsertCharacter(Cursor.TextCursor, p_E.Character);
                        Cursor.TextCursor++;
                    }
                }
            }
        }

        private string DelSelection(bool fakeForCopy = false)
        {
            if (!Cursor.SelectedChar.HasValue) {
                return null;
            }

            int tc = Cursor.TextCursor;
            int sc = Cursor.SelectedChar.Value;
            int min = Math.Min(sc, tc);
            int max = Math.Max(sc, tc);
            string result = Text.String.Substring(min, max - min);

            if (!fakeForCopy) {
                Text.Replace(Math.Min(sc, tc), Math.Max(sc, tc), string.Empty);
                if (Cursor.SelectedChar.Value < Cursor.TextCursor) {
                    Cursor.TextCursor -= tc - sc;
                }

                Cursor.SelectedChar = null;
            }

            return result;
        }
    }
}