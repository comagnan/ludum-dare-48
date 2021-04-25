using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using LD48.Content;
using LD48.Framework.Input;

namespace LD48.Dialogue
{
    public class DialogueBox : IDialogueBox
    {
        private const int MAXIMUM_CHARACTERS_PER_LINE = 38;
        private const int MAXIMUM_LINES = 3;
        private const int MOVEMENT_SPEED = 2;
        private readonly TimeSpan TIME_BETWEEN_CHARACTERS = TimeSpan.FromSeconds(0.03);

        private readonly IList<DialogueEntry> m_DialogueBuffer;
        private readonly ContentManager m_Content;
        private IList<string> m_CurrentText;
        private bool m_IsInitialized;

        // [line, character] format. Could be a vector, if you like reinitializing for no reason.
        private int[] m_CurrentPointer = new int[2];

        private Texture2D m_Background;
        private Rectangle m_BackgroundShape;
        private Vector2 m_TextPosition;
        private int m_Offset;

        private SpriteFont m_RegularFont;
        private SpriteFont m_LargeFont;

        private Texture2D m_AlexisSprite;
        private Texture2D m_BeatriceSprite;
        private Texture2D m_SignSprite;

        private Texture2D m_DownArrow;

        private SoundEffect m_DialogueSoundEffect;

        private TimeSpan m_Counter;
        private char m_LastChar;

        public DialogueBox(Rectangle p_BackgroundShape,
                           Vector2 p_TextPosition,
                           ContentManager p_Content)
        {
            m_DialogueBuffer = new List<DialogueEntry>();
            m_BackgroundShape = p_BackgroundShape;
            m_TextPosition = p_TextPosition;
            m_Content = p_Content;
            m_Counter = TimeSpan.Zero;
            m_LastChar = ' ';
            m_Offset = m_BackgroundShape.Height;
        }

        public void AddText(DialogueEntry p_Dialogue)
        {
            m_DialogueBuffer.Add(p_Dialogue);

            if (IsActive()) {
                m_CurrentText = WrapText(m_DialogueBuffer[0].Text);
            }
        }

        public bool IsActive() => GetBufferSize() > 0;

        public bool IsVisible() => IsActive() || m_Offset < m_BackgroundShape.Height;

        public int GetBufferSize() => m_DialogueBuffer.Count;

        public void Update(GameTime p_GameTime,
                           in InputController p_InputController)
        {
            if (!IsActive() && m_Offset < m_BackgroundShape.Height) {
                // TODO: Acceleration over constant speed would be a nice plus.
                m_Offset += p_GameTime.ElapsedGameTime.Milliseconds * MOVEMENT_SPEED;
            } else if (IsActive() && m_IsInitialized) {
                if (m_Offset > 0) {
                    // TODO: Deceleration over constant speed would be a nice plus.
                    m_Offset -= p_GameTime.ElapsedGameTime.Milliseconds * MOVEMENT_SPEED;
                } else if (!IsShowingFullText()) {
                    if (p_InputController.IsButtonPress(InputConfiguration.Confirm) || p_InputController.IsButtonPress(InputConfiguration.Return)) {
                        int maxColumns = Math.Min(m_CurrentText.Count, MAXIMUM_LINES);
                        m_CurrentPointer[0] = maxColumns - 1;
                        m_CurrentPointer[1] = m_CurrentText[m_CurrentPointer[0]].Length - 1;
                    } else {
                        int mult = 1;
                        if (m_LastChar == '.' || m_LastChar == '!' || m_LastChar == '?') {
                            mult = 4;
                        }

                        if (m_Counter > TIME_BETWEEN_CHARACTERS * mult) {
                            if (m_CurrentPointer[1] >= m_CurrentText[m_CurrentPointer[0]].Length) {
                                m_CurrentPointer[0]++;
                                m_CurrentPointer[1] = 0;
                            } else {
                                m_LastChar = m_CurrentText[m_CurrentPointer[0]][m_CurrentPointer[1]];
                                m_CurrentPointer[1]++;
                            }

                            m_Counter = TimeSpan.Zero;
                            m_DialogueSoundEffect.Play(1.0f, -0.5f, 0f);
                        } else {
                            m_Counter += p_GameTime.ElapsedGameTime;
                        }
                    }
                } else {
                    // Go over every line in string. Proceed on button press.
                    if (p_InputController.IsButtonPress(InputConfiguration.Confirm) || p_InputController.IsButtonPress(InputConfiguration.Return)) {
                        PopDialogue();
                    }
                }
            }
        }

        public void Draw(GameTime p_GameTime, SpriteBatch p_SpriteBatch)
        {
            if (!m_IsInitialized) {
                Initialize(p_SpriteBatch);
            }

            if (m_Offset < m_BackgroundShape.Height && m_Offset > 0) {
                // Only draw background fill texture
                Rectangle box = m_BackgroundShape;
                box.Y += m_Offset;
                p_SpriteBatch.Draw(m_Background, box, Color.Black);

                Rectangle nameTag = new (box.X, box.Y-50, box.Width, 50);
                p_SpriteBatch.Draw(m_Background, nameTag, Color.White);
            } else if (IsActive() && m_Offset < m_BackgroundShape.Height) {
                DialogueEntry dialogueEntry = m_DialogueBuffer[0];

                Rectangle nameTag = new (m_BackgroundShape.X, m_BackgroundShape.Y-50, m_BackgroundShape.Width, 50);
                p_SpriteBatch.Draw(m_Background, nameTag, Color.White);
                // Draw background fill texture
                p_SpriteBatch.Draw(m_Background, m_BackgroundShape, Color.Black);

                if (!string.IsNullOrEmpty(dialogueEntry.Speaker)) {
                    p_SpriteBatch.DrawString(GetFont(dialogueEntry.Font),
                        dialogueEntry.Speaker,
                        new Vector2(m_TextPosition.X, m_TextPosition.Y - 80),
                        Color.Black);
                }

                int maxLines = Math.Min(MAXIMUM_LINES, m_CurrentPointer[0] + 1);

                for (int i = 0; i < maxLines; i++) {
                    if (i < m_CurrentPointer[0]) {
                        p_SpriteBatch.DrawString(GetFont(dialogueEntry.Font),
                            m_CurrentText[i],
                            new Vector2(m_TextPosition.X, m_TextPosition.Y + 50 * i),
                            Color.White);
                    } else if (i == m_CurrentPointer[0]) {
                        p_SpriteBatch.DrawString(GetFont(dialogueEntry.Font),
                            m_CurrentText[i].Substring(0, m_CurrentPointer[1]),
                            new Vector2(m_TextPosition.X, m_TextPosition.Y + 50 * i),
                            Color.White);
                    }
                }

                if (!string.IsNullOrEmpty(dialogueEntry.Sprite)) {
                    int spriteSize = 320;
                    Rectangle destination = new(m_BackgroundShape.Right - spriteSize, m_BackgroundShape.Bottom - spriteSize, spriteSize, spriteSize);
                    p_SpriteBatch.Draw(GetSprite(dialogueEntry.Sprite), destination, Color.White);
                }

                if (m_CurrentText.Count > MAXIMUM_LINES && IsShowingFullText()) {
                    int xscale = 410;
                    Rectangle destination = new Rectangle(m_BackgroundShape.Right - xscale,
                        m_BackgroundShape.Bottom - 80 - (int) (10 * Math.Cos(p_GameTime.TotalGameTime.TotalSeconds * 5)),
                        50,
                        50);
                    p_SpriteBatch.Draw(m_DownArrow, destination, Color.White);
                }
            }
        }

        private void PopDialogue()
        {
            if (GetBufferSize() > 0) {
                if (m_CurrentText.Count > MAXIMUM_LINES) {
                    for (int i = MAXIMUM_LINES; i > 0; i--) {
                        m_CurrentText.RemoveAt(i - 1);
                    }
                } else {
                    m_DialogueBuffer[0].Callback?.Invoke();
                    m_DialogueBuffer.RemoveAt(0);
                    m_CurrentText = GetBufferSize() > 0 ? WrapText(m_DialogueBuffer[0].Text) : new List<string>();
                }

                m_CurrentPointer = new[] {
                    0,
                    0
                };
            }
        }

        /// <summary>
        /// Splits a string into lines based on maximum characters in dialogue box.
        /// </summary>
        private List<string> WrapText(string p_Text)
        {
            List<string> output = new();
            StringBuilder currentWord = new();
            StringBuilder currentLine = new();

            for (int i = 0; i < p_Text.Length; i++) {
                char currentCharacter = p_Text[i];
                bool endReached = i == p_Text.Length - 1;
                if (currentCharacter == ' ' || currentCharacter == '\n' || endReached) {
                    if (currentCharacter != '\n') {
                        currentWord.Append(currentCharacter);
                    }

                    if (currentLine.Length + currentWord.Length > MAXIMUM_CHARACTERS_PER_LINE) {
                        output.Add(currentLine.ToString());
                        currentLine = new StringBuilder(currentWord.ToString());
                        if (currentCharacter != ' ') {
                            output.Add(currentLine.ToString());
                            currentLine.Clear();
                        }
                    } else {
                        currentLine.Append(currentWord);
                        if (currentCharacter == '\n' || endReached) {
                            output.Add(currentLine.ToString());
                            currentLine.Clear();
                        }
                    }

                    currentWord.Clear();
                } else if (currentCharacter == '\n') {
                    currentLine.Append(currentWord);
                    output.Add(currentLine.ToString());
                    currentLine.Clear();
                } else {
                    currentWord.Append(currentCharacter);
                }
            }

            return output;
        }

        private SpriteFont GetFont(DialogueFont p_Font)
        {
            return p_Font == DialogueFont.Standard ? m_RegularFont : m_LargeFont;
        }

        private Texture2D GetSprite(string p_String)
        {
            if (p_String == GameInterface.Alexis) {
                return m_AlexisSprite;
            }

            if (p_String == GameInterface.Beatrice) {
                return m_BeatriceSprite;
            }
            return m_SignSprite;
        }

        private bool IsShowingFullText()
        {
            int maxColumns = Math.Min(m_CurrentText.Count, MAXIMUM_LINES);

            return m_CurrentPointer[0] >= maxColumns - 1 && m_CurrentPointer[1] >= m_CurrentText[m_CurrentPointer[0]].Length;
        }

        private void Initialize(SpriteBatch p_SpriteBatch)
        {
            m_Background = new(p_SpriteBatch.GraphicsDevice, 1, 1);
            m_Background.SetData(new[] { Color.White });

            m_RegularFont = m_Content.Load<SpriteFont>("Dialogue");
            m_LargeFont = m_Content.Load<SpriteFont>("Dialogue");
            m_AlexisSprite = m_Content.Load<Texture2D>("Portraits/alexis_worried");
            m_BeatriceSprite = m_Content.Load<Texture2D>("Portraits/beatrice_smiling");
            m_SignSprite = m_Content.Load<Texture2D>("Portraits/Sign");
            m_DownArrow = m_Content.Load<Texture2D>("arrow");

            m_DialogueSoundEffect = m_Content.Load<SoundEffect>("SFX/alexis_dialogue");

            m_IsInitialized = true;
        }
    }
}