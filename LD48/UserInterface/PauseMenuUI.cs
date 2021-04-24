using System;
using LD48.Content;
using LD48.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LD48.UserInterface
{
    public class PauseMenuUI : IUserInterface
    {
        private const int ITEM_DISTANCE = 70;
        private const int MAXIMUM_POINTER = 6;
        private const int OFFSCREEN_OFFSET = 700;
        private const int MOVEMENT_VELOCITY = 3;
        private readonly Texture2D m_BookmarkTexture;
        private readonly Texture2D m_SelectionBubble;

        private readonly SpriteFont m_Font;

        // Graphics
        private RenderTarget2D m_InternalResolution;

        private int m_CurrentPointer;
        private int m_Offset;
        public bool IsPausedOrTransitioning => Paused || m_Offset < OFFSCREEN_OFFSET;

        public bool Paused { get; set; }

        public PauseMenuUI(RenderTarget2D p_InternalResolution,
                           ContentManager p_Content)
        {
            m_InternalResolution = p_InternalResolution;

            m_BookmarkTexture = p_Content.Load<Texture2D>("Interface/pause");
            m_SelectionBubble = p_Content.Load<Texture2D>("Interface/selection");
            m_Font = p_Content.Load<SpriteFont>("Dialogue");

            Paused = false;
            m_Offset = OFFSCREEN_OFFSET;
            m_CurrentPointer = 0;
        }

        public void Update(GameTime p_Time,
                           in InputController p_InputController)
        {
            if (Paused) {
                if (m_Offset > 0) {
                    m_Offset = Math.Max(0, m_Offset - (int) (p_Time.ElapsedGameTime.TotalMilliseconds * MOVEMENT_VELOCITY));
                } else {
                    if ((p_InputController.IsButtonPress(InputConfiguration.Confirm) && m_CurrentPointer == 0)
                        || p_InputController.IsButtonPress(InputConfiguration.Pause)) {
                        Paused = false;
                    } else if (p_InputController.IsButtonPress(InputConfiguration.Down)) {
                        m_CurrentPointer++;
                        if (m_CurrentPointer > MAXIMUM_POINTER) {
                            m_CurrentPointer = 0;
                        }
                    } else if (p_InputController.IsButtonPress(InputConfiguration.Up)) {
                        m_CurrentPointer--;
                        if (m_CurrentPointer < 0) {
                            m_CurrentPointer = MAXIMUM_POINTER;
                        }
                    }
                }
            } else if (m_Offset < OFFSCREEN_OFFSET) {
                m_Offset = Math.Min(OFFSCREEN_OFFSET, m_Offset + (int) (p_Time.ElapsedGameTime.TotalMilliseconds * MOVEMENT_VELOCITY));
                m_CurrentPointer = 0;
            }
        }

        public void Draw(GameTime p_Time,
                         SpriteBatch p_SpriteBatch,
                         SpriteFont p_TitleFont)
        {
            if (IsPausedOrTransitioning) {
                p_SpriteBatch.Draw(m_InternalResolution, new Rectangle(0, 0, 1920, 1080), Color.Black * 0.75f);

                p_SpriteBatch.Draw(m_BookmarkTexture, new Rectangle(150, -m_Offset, 300, 720), Color.White);
                p_SpriteBatch.Draw(m_SelectionBubble,
                    new Rectangle(188, 100 - m_Offset + ITEM_DISTANCE * m_CurrentPointer, 222, 50),
                    Color.White * 0.75f);
                p_SpriteBatch.DrawString(m_Font,
                    GameInterface.Resume,
                    new Vector2(200, 97 - m_Offset),
                    Color.Black,
                    0f,
                    Vector2.Zero,
                    0.75f,
                    SpriteEffects.None,
                    1f);
                p_SpriteBatch.DrawString(m_Font,
                    GameInterface.Collection,
                    new Vector2(200, 97 + ITEM_DISTANCE - m_Offset),
                    Color.Black,
                    0f,
                    Vector2.Zero,
                    0.75f,
                    SpriteEffects.None,
                    1f);
                p_SpriteBatch.DrawString(m_Font,
                    GameInterface.History,
                    new Vector2(200, 97 + 2 * ITEM_DISTANCE - m_Offset),
                    Color.Black,
                    0f,
                    Vector2.Zero,
                    0.75f,
                    SpriteEffects.None,
                    1f);
                p_SpriteBatch.DrawString(m_Font,
                    GameInterface.Options,
                    new Vector2(200, 97 + 3 * ITEM_DISTANCE - m_Offset),
                    Color.Black,
                    0f,
                    Vector2.Zero,
                    0.75f,
                    SpriteEffects.None,
                    1f);
                p_SpriteBatch.DrawString(m_Font,
                    GameInterface.Restart,
                    new Vector2(200, 97 + 4 * ITEM_DISTANCE - m_Offset),
                    Color.Black,
                    0f,
                    Vector2.Zero,
                    0.75f,
                    SpriteEffects.None,
                    1f);
                p_SpriteBatch.DrawString(m_Font,
                    GameInterface.Home,
                    new Vector2(200, 97 + 5 * ITEM_DISTANCE - m_Offset),
                    Color.Black,
                    0f,
                    Vector2.Zero,
                    0.75f,
                    SpriteEffects.None,
                    1f);
                p_SpriteBatch.DrawString(m_Font,
                    GameInterface.Save_Quit,
                    new Vector2(200, 97 + 6 * ITEM_DISTANCE - m_Offset),
                    Color.Black,
                    0f,
                    Vector2.Zero,
                    0.75f,
                    SpriteEffects.None,
                    1f);
            }
        }
    }
}