﻿using System.Globalization;
using LD48.Framework.Input;
using LD48.Framework.Levels;
using LD48.Tools;
using LD48.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD48
{
    public class GameCore : Game
    {
        private const string BASE_GAME_TITLE = "Ludum Dare 48";
        private readonly FrameCounter m_FrameCounter;

        private readonly GraphicsDeviceManager m_Graphics;

        // Controls
        private readonly InputController m_InputController;

        private SpriteBatch m_SpriteBatch;
        private SpriteFont m_TitleFont;
        private TitleScreen m_TitleScreen;
        private Level m_CurrentLevel;

        private PauseMenuUI m_PauseMenu;

        // Graphics
        private RenderTarget2D m_InternalResolution;

        public GameCore()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            m_FrameCounter = new FrameCounter();
            Content.RootDirectory = "Content";
            IsMouseVisible = false;

            // Unlock framerate.
            //m_Graphics.SynchronizeWithVerticalRetrace = false;
            //IsFixedTimeStep = false;

            m_InputController = new InputController();
            m_CurrentLevel = new DebugLevel(Content);
        }

        protected override void Initialize()
        {
            base.Initialize();

            m_InternalResolution = new RenderTarget2D(GraphicsDevice, 1280, 720);
            m_Graphics.PreferredBackBufferWidth = 1280;
            m_Graphics.PreferredBackBufferHeight = 720;
            m_Graphics.ApplyChanges();

            m_TitleScreen = new TitleScreen(m_InternalResolution, Content);
            m_PauseMenu = new PauseMenuUI(m_InternalResolution, Content);

            Window.Title = BASE_GAME_TITLE;

            m_CurrentLevel.Initialize(Window, GraphicsDevice);
        }

        protected override void LoadContent()
        {
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);
            m_TitleFont = Content.Load<SpriteFont>("Title");
        }

        protected override void Update(GameTime p_GameTime)
        {
            base.Update(p_GameTime);
            m_InputController.UpdateState();

            if (m_InputController.IsButtonDown(InputConfiguration.Exit) || m_TitleScreen.ExitGame) {
                Exit();
            }

            if (!m_TitleScreen.IsClosed) {
                m_TitleScreen.Update(p_GameTime, m_InputController);
            } else if (m_PauseMenu.IsPausedOrTransitioning) {
                m_PauseMenu.Update(p_GameTime, m_InputController);
            } else {
                if (m_InputController.IsButtonPress(InputConfiguration.Pause)) {
                    m_PauseMenu.Paused = !m_PauseMenu.Paused;
                }

                m_CurrentLevel.Update(p_GameTime, m_InputController);
                m_FrameCounter.Update(p_GameTime);
            }
        }

        protected override void Draw(GameTime p_GameTime)
        {
            GraphicsDevice.SetRenderTarget(m_InternalResolution);
            GraphicsDevice.Clear(Color.White);

            if (m_TitleScreen.IsClosed) {
                m_CurrentLevel.Draw(p_GameTime, m_SpriteBatch);
            }

            // Zoom to screen.
            GraphicsDevice.SetRenderTarget(null);
            m_SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            m_SpriteBatch.Draw(m_InternalResolution, new Rectangle(0, 0, 1280, 720), Color.White);

            // Draw UI at native resolution.
            if (m_TitleScreen.IsClosed) {
                m_CurrentLevel.DrawInterface(p_GameTime, m_SpriteBatch, m_TitleFont);
                m_SpriteBatch.DrawString(m_TitleFont,
                    m_FrameCounter.AverageFramesPerSecond.ToString(CultureInfo.InvariantCulture),
                    new Vector2(1200, 50),
                    Color.Yellow);
                m_PauseMenu.Draw(p_GameTime, m_SpriteBatch, m_TitleFont);
            } else {
                m_TitleScreen.Draw(p_GameTime, m_SpriteBatch, m_TitleFont);
            }
            m_SpriteBatch.End();

            base.Draw(p_GameTime);
        }
    }
}