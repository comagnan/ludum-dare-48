using System;
using System.Collections.Generic;
using LD48.Actors;
using LD48.Actors.Interactive;
using LD48.Dialogue;
using LD48.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.ViewportAdapters;

namespace LD48.Framework.Levels
{
    // As you can see, I've knocked over many chairs because I get so tilted at the towers.
    public class Level : IDisposable
    {
        protected readonly DialogueBox DialogueBox;
        protected Vector2 ResetPosition;
        protected GraphicsDevice GraphicsDevice;
        protected Texture2D HeartTexture;
        protected Texture2D ManaTexture;
        protected Texture2D Rectangle;

        protected TiledMap TiledMap;
        protected TiledMapRenderer TiledMapRenderer;
        protected TiledMapTileLayer CollisionLayer;
        protected OrthographicCamera OrthographicCamera;
        protected List<InteractiveActor> InteractiveActors;

        // Level content.        
        protected ContentManager Content { get; }

        protected PlayerActor Player { get; set; }

        public bool IsLevelOver { get; protected set; }
        public Level NextLevel { get; protected set; }

        public Level(ContentManager p_Content,
                     Vector2 p_ResetPosition)
        {
            // Create a new content manager to load content used just by this level.
            Content = p_Content;
            ResetPosition = p_ResetPosition;
            DialogueBox = new DialogueBox(new Rectangle(20, 470, 1240, 240), new Vector2(100, 490), Content);
            InteractiveActors = new List<InteractiveActor>();
        }

        public void Dispose()
        {
            Content.Unload();
        }

        // Must be called after initializing inheritors.
        public virtual void Initialize(GameWindow p_Window,
                                       GraphicsDevice p_GraphicsDevice)
        {
            GraphicsDevice = p_GraphicsDevice;
            HeartTexture = Content.Load<Texture2D>("Interface/heart");
            ManaTexture = Content.Load<Texture2D>("Interface/mana");
            Rectangle = new Texture2D(GraphicsDevice, 1, 1);
            Rectangle.SetData(new[] { Color.White });

            CollisionLayer = TiledMap.GetLayer<TiledMapTileLayer>("Collisions");
            TiledMapRenderer = new TiledMapRenderer(p_GraphicsDevice, TiledMap);
            BoxingViewportAdapter viewport = new(p_Window, p_GraphicsDevice, 640, 360);
            OrthographicCamera = new OrthographicCamera(viewport);
        }

        public virtual void Update(GameTime p_GameTime,
                                   in InputController p_InputController)
        {
            TiledMapRenderer.Update(p_GameTime);
            OrthographicCamera.LookAt(GetCameraPosition());

            DialogueBox.Update(p_GameTime, p_InputController);
        }

        public virtual void Draw(GameTime p_GameTime,
                                 SpriteBatch p_SpriteBatch)
        {
            TiledMapRenderer.Draw(OrthographicCamera.GetViewMatrix());
        }

        public void DrawInterface(GameTime p_GameTime,
                                  SpriteBatch p_SpriteBatch,
                                  SpriteFont p_SpriteFont)
        {
            p_SpriteBatch.Draw(Rectangle, new Rectangle(90, 625, 100, 22), Color.Black);
            p_SpriteBatch.Draw(Rectangle, new Rectangle(92, 627, (int) ((float) Player.CurrentHP / Player.MaxHP * 96), 18), Color.Red);
            p_SpriteBatch.Draw(Rectangle, new Rectangle(90, 665, 100, 22), Color.Black);
            p_SpriteBatch.Draw(Rectangle, new Rectangle(92, 667, (int) ((float) Player.CurrentMP / Player.MaxMP * 96), 18), Color.Teal);
            p_SpriteBatch.Draw(HeartTexture, new Rectangle(50, 620, 32, 32), Color.White);
            p_SpriteBatch.Draw(ManaTexture, new Rectangle(50, 660, 32, 32), Color.White);
            DialogueBox.Draw(p_SpriteBatch, p_GameTime);

            p_SpriteBatch.DrawString(p_SpriteFont, Player.GetRoomId().ToString(), new Vector2(50, 180), Color.Chartreuse);
        }

        /// <summary>
        /// Restores the level to the last checkpoint. Level must be rewound accordingly.
        /// </summary>
        public void StartNewLife()
        {
            Player.Reset(ResetPosition);
        }

        // Default camera position. Currently snaps between rooms. Very basic.
        protected virtual Vector2 GetCameraPosition()
        {
            int xMultiplier = (int) Math.Floor(Player.GetPosition().X / 640);
            int yMultiplier = (int) Math.Floor(Player.GetPosition().Y / 360);

            int x = 320 + 640 * xMultiplier;
            int y = 180 + 360 * yMultiplier;

            return new Vector2(x, y);
        }
    }
}