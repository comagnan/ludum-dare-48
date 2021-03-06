using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
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
        private const int ITEM_DISTANCE = 120;
        private const int MAXIMUM_POINTER = 3;
        private readonly Texture2D m_TitleScreenArt;
        private readonly Texture2D m_HowToPlay;
        private readonly Texture2D m_Credits;

        private readonly SpriteFont m_Font;
        private readonly Song m_TitleScreenSong;

        // Graphics
        private Texture2D m_ButtonTexture;

        private int m_CurrentPointer;
        private bool m_ShowHowTo;
        private bool m_ShowCredits;

        public bool ShowOptions { get; set; }

        public bool IsClosed { get; set; }

        public bool ExitGame { get; set; }

        public TitleScreen(RenderTarget2D p_InternalResolution,
                           ContentManager p_Content)
        {
            m_TitleScreenArt = p_Content.Load<Texture2D>("Interface/title_screen");
            m_HowToPlay = p_Content.Load<Texture2D>("Interface/howto");
            m_Credits = p_Content.Load<Texture2D>("Interface/credits");
            m_Font = p_Content.Load<SpriteFont>("Dialogue");
            m_TitleScreenSong = p_Content.Load<Song>("SFX/titlescreen");
            m_ButtonTexture = p_Content.Load<Texture2D>("Interface/button");
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
                if (m_ShowCredits) {
                    if (p_InputController.IsButtonPress(InputConfiguration.BassTwitter)) {
                        try {
                            OpenUrl("https://twitter.com/TheBlondeBass");
                        } catch (Exception e) {
                            Console.WriteLine(e.Message);
                        }
                    } else if (p_InputController.IsButtonPress(InputConfiguration.TriggerTwitter)) {
                        try {
                            OpenUrl("https://twitter.com/triggerpigart");
                        } catch (Exception e) {
                            Console.WriteLine(e.Message);
                        }
                    } else if (p_InputController.IsButtonPress(InputConfiguration.AlphadeusBandcamp)) {
                        try {
                            OpenUrl("https://alphadeus.bandcamp.com/");
                        } catch (Exception e) {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                if (p_InputController.IsButtonPress(InputConfiguration.Submit) || p_InputController.IsButtonPress(InputConfiguration.Return)) {
                    m_ShowHowTo = false;
                    m_ShowCredits = false;
                }
            } else if (ShowOptions) {
                if (p_InputController.IsButtonPress(InputConfiguration.Submit)) {
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
            p_SpriteBatch.Draw(m_TitleScreenArt, new Rectangle(0, 0, 1920, 1080), Color.White);
            //Console.WriteLine(m_Font.MeasureString(GameInterface.GameTitle));

            if (m_ShowHowTo) {
                p_SpriteBatch.Draw(m_HowToPlay, new Vector2(0, 0), Color.White);
            } else if (m_ShowCredits) {
                p_SpriteBatch.Draw(m_Credits, new Vector2(0, 0), Color.White);
            } else {
                if (ShowOptions) {
                    p_SpriteBatch.Draw(m_ButtonTexture,
                        new Vector2(960, 600),
                        new Rectangle(0, 0, 610, 114),
                        m_CurrentPointer == 0 ? Color.Gray : Color.White,
                        0f,
                        new Vector2(305, 57),
                        1f,
                        SpriteEffects.None,
                        1f);
                    p_SpriteBatch.Draw(m_ButtonTexture,
                        new Vector2(960, 600 + ITEM_DISTANCE),
                        new Rectangle(0, 0, 610, 114),
                        m_CurrentPointer == 1 ? Color.Gray : Color.White,
                        0f,
                        new Vector2(305, 57),
                        1f,
                        SpriteEffects.None,
                        1f);
                    p_SpriteBatch.Draw(m_ButtonTexture,
                        new Vector2(960, 600 + 2 * ITEM_DISTANCE),
                        new Rectangle(0, 0, 610, 114),
                        m_CurrentPointer == 2 ? Color.Gray : Color.White,
                        0f,
                        new Vector2(305, 57),
                        1f,
                        SpriteEffects.None,
                        1f);
                    p_SpriteBatch.Draw(m_ButtonTexture,
                        new Vector2(960, 600 + 3 * ITEM_DISTANCE),
                        new Rectangle(0, 0, 610, 114),
                        m_CurrentPointer == 3 ? Color.Gray : Color.White,
                        0f,
                        new Vector2(305, 57),
                        1f,
                        SpriteEffects.None,
                        1f);
                    //Console.WriteLine($"START: {m_Font.MeasureString(GameInterface.GameTitle)}");
                    p_SpriteBatch.DrawString(m_Font,
                        GameInterface.Start,
                        new Vector2(976, 600),
                        Color.White,
                        0f,
                        new Vector2(62, 31),
                        1f,
                        SpriteEffects.None,
                        1f);
                    //Console.WriteLine($"HOW TO PLAY: {m_Font.MeasureString(GameInterface.HowToPlay)}");
                    p_SpriteBatch.DrawString(m_Font,
                        GameInterface.HowToPlay,
                        new Vector2(960, 600 + ITEM_DISTANCE),
                        Color.White,
                        0f,
                        new Vector2(110, 31),
                        1f,
                        SpriteEffects.None,
                        1f);
                    //Console.WriteLine($"CREDITS: {m_Font.MeasureString(GameInterface.Credits)}");
                    p_SpriteBatch.DrawString(m_Font,
                        GameInterface.Credits,
                        new Vector2(960, 600 + 2 * ITEM_DISTANCE),
                        Color.White,
                        0f,
                        new Vector2(66, 31),
                        1f,
                        SpriteEffects.None,
                        1f);
                    //Console.WriteLine($"EXIT: {m_Font.MeasureString(GameInterface.Quit)}");
                    p_SpriteBatch.DrawString(m_Font,
                        GameInterface.Quit,
                        new Vector2(960, 600 + 3 * ITEM_DISTANCE),
                        Color.White,
                        0f,
                        new Vector2(41, 31),
                        1f,
                        SpriteEffects.None,
                        1f);
                } else {
                    p_SpriteBatch.DrawString(m_Font,
                        GameInterface.PressAButton,
                        new Vector2(890, 900),
                        Color.Black * (float) Math.Abs(Math.Cos(p_Time.TotalGameTime.TotalSeconds)),
                        0f,
                        Vector2.Zero,
                        1f,
                        SpriteEffects.None,
                        1f);
                }
            }
        }

        private void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}