using System;
using LD48.Content;
using LD48.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace LD48.UserInterface
{
    public class TitleScreen : IUserInterface
    {
        private const int ITEM_DISTANCE = 70;
        private const int MAXIMUM_POINTER = 3;
        private readonly Texture2D m_TitleScreenArt;
        private readonly Texture2D m_SelectionBubble;

        private readonly SpriteFont m_Font;
        private readonly Song m_TitleScreenSong;

        // Graphics
        private TimeSpan m_TimeSinceButtonPress;

        private int m_CurrentPointer;
        private int m_Offset;
        private bool m_ShowHowTo;
        private bool m_ShowCredits;

        public bool ShowOptions { get; set; }

        public bool IsClosed { get; set; }

        public bool ExitGame { get; set; }

        public TitleScreen(RenderTarget2D p_InternalResolution,
                           ContentManager p_Content)
        {
            m_TitleScreenArt = p_Content.Load<Texture2D>("Interface/title_screen");
            m_SelectionBubble = p_Content.Load<Texture2D>("Interface/selection");
            m_Font = p_Content.Load<SpriteFont>("Dialogue");
            m_TitleScreenSong = p_Content.Load<Song>("SFX/betterdays_todelete");
            MediaPlayer.Play(m_TitleScreenSong);
            MediaPlayer.IsRepeating = true;

            m_ShowHowTo = false;
            m_ShowCredits = false;
            ShowOptions = false;
            IsClosed = false;
            ExitGame = false;
            m_CurrentPointer = 0;
        }

        public void Update(GameTime p_Time,
                           in InputController p_InputController)
        {
            if (m_ShowHowTo || m_ShowCredits) {
                if (p_InputController.IsButtonPress(InputConfiguration.Confirm) || p_InputController.IsButtonPress(InputConfiguration.Return)) {
                    m_ShowHowTo = false;
                    m_ShowCredits = false;
                }
            } else if (ShowOptions) {
                if (p_InputController.IsButtonPress(InputConfiguration.Confirm)) {
                    switch (m_CurrentPointer) {
                        case 0:
                            IsClosed = true;
                            MediaPlayer.Stop();
                            break;
                        case 1:
                            m_ShowHowTo = true;
                            break;
                        case 2:
                            m_ShowCredits = true;
                            break;
                        case MAXIMUM_POINTER:
                            ExitGame = true;
                            break;
                    }
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
            } else {
                if (p_InputController.IsButtonPress(InputConfiguration.Confirm) || p_InputController.IsButtonPress(InputConfiguration.Pause)) {
                    ShowOptions = true;
                }
            }
        }

        public void Draw(GameTime p_Time,
                         SpriteBatch p_SpriteBatch,
                         SpriteFont p_TitleFont)
        {
            p_SpriteBatch.Draw(m_TitleScreenArt, new Rectangle(0, 0, 1280, 720), Color.White);
            //Console.WriteLine(m_Font.MeasureString(GameInterface.GameTitle));

            if (m_ShowHowTo) {
                p_SpriteBatch.DrawString(m_Font,
                    "This is where the tutorial goes.\n\nPress F to toggle full screen.",
                    new Vector2(100, 100),
                    Color.White,
                    0f,
                    Vector2.Zero,
                    1f,
                    SpriteEffects.None,
                    1f);
            } else if (m_ShowCredits) {
                p_SpriteBatch.DrawString(m_Font,
                    "A game by: \nCharles-Olivier Magnan (@TheBlondeBass) and \nTriggerPigKing (@triggerpigart) \nbuilt in C# over 72h for Ludum Dare 48.",
                    new Vector2(100, 100),
                    Color.White,
                    0f,
                    Vector2.Zero,
                    1f,
                    SpriteEffects.None,
                    1f);
            } else {
                p_SpriteBatch.DrawString(m_Font,
                    GameInterface.GameTitle,
                    new Vector2(302, 100),
                    Color.White,
                    0f,
                    Vector2.Zero,
                    2f,
                    SpriteEffects.None,
                    1f);


                if (ShowOptions) {
                    p_SpriteBatch.Draw(m_SelectionBubble,
                        new Rectangle(521, 308 + ITEM_DISTANCE * m_CurrentPointer, 240, 50),
                        Color.White * (0.2f + 0.05f * (float) Math.Cos(p_Time.TotalGameTime.TotalSeconds * 4.0)));
                    p_SpriteBatch.DrawString(m_Font,
                        GameInterface.Start,
                        new Vector2(594, 297),
                        Color.White,
                        0f,
                        Vector2.Zero,
                        1f,
                        SpriteEffects.None,
                        1f);
                    p_SpriteBatch.DrawString(m_Font,
                        GameInterface.HowToPlay,
                        new Vector2(530, 297 + ITEM_DISTANCE),
                        Color.White,
                        0f,
                        Vector2.Zero,
                        1f,
                        SpriteEffects.None,
                        1f);
                    p_SpriteBatch.DrawString(m_Font,
                        GameInterface.Credits,
                        new Vector2(574, 297 + 2 * ITEM_DISTANCE),
                        Color.White,
                        0f,
                        Vector2.Zero,
                        1f,
                        SpriteEffects.None,
                        1f);
                    p_SpriteBatch.DrawString(m_Font,
                        GameInterface.Quit,
                        new Vector2(599, 297 + 3 * ITEM_DISTANCE),
                        Color.White,
                        0f,
                        Vector2.Zero,
                        1f,
                        SpriteEffects.None,
                        1f);
                } else {
                    p_SpriteBatch.DrawString(m_Font,
                        GameInterface.PressAButton,
                        new Vector2(570, 600),
                        Color.White * (float) Math.Abs(Math.Cos(p_Time.TotalGameTime.TotalSeconds)),
                        0f,
                        Vector2.Zero,
                        1f,
                        SpriteEffects.None,
                        1f);
                }
            }
        }
    }
}