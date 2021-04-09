using System.Collections.Generic;
using System.Linq;
using LD48.Actors;
using LD48.Actors.Interactive;
using LD48.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;

namespace LD48.Framework.Levels
{
    public class DebugLevel : Level
    {
        /// <summary>
        /// Constructs a new level.
        /// </summary>
        public DebugLevel(ContentManager p_Content) : base(p_Content, new Vector2(32, 32))
        {
            Player = new PlayerActor(new Vector2(32, 32), 100, 5000, Content);
        }

        public override void Initialize(GameWindow p_Window,
                                        GraphicsDevice p_GraphicsDevice)
        {
            TiledMap = Content.Load<TiledMap>("Tilemaps/debugLevel");

            base.Initialize(p_Window, p_GraphicsDevice);
        }

        public override void Update(GameTime p_GameTime,
                                    in InputController p_InputController)
        {
            base.Update(p_GameTime, p_InputController);

            // Pause while the player is dead.
            if (Player.IsAlive && !DialogueBox.IsVisible()) {
                Player.Update(p_GameTime, p_InputController, CollisionLayer);

                foreach (InteractiveActor actor in InteractiveActors) {
                    actor.Update(p_GameTime, Player);
                    if (p_InputController.IsButtonPress(InputConfiguration.Interact)) {
                        actor.Interact(DialogueBox);
                    }
                }
            }
        }

        public override void Draw(GameTime p_GameTime,
                                  SpriteBatch p_SpriteBatch)
        {
            p_SpriteBatch.Begin(transformMatrix: OrthographicCamera.GetViewMatrix());

            base.Draw(p_GameTime, p_SpriteBatch);

            // Draw actors in drawing order (Includes Player)
            List<GameActor> actors = new() {
                Player,
            };
            actors.AddRange(InteractiveActors);
            
            foreach (GameActor gameActor in actors.OrderBy(a => a.GetPosition().Y)) {
                gameActor.Draw(p_GameTime, p_SpriteBatch);
            }

            p_SpriteBatch.End();
        }
    }
}