using LD48.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LD48.Framework.Levels
{
    public class DebugLevel : Level
    {
        /// <summary>
        /// Constructs a new level.
        /// </summary>
        public DebugLevel(ContentManager p_Content) : base(p_Content, 0)
        {
        }

        public override void Initialize(GameWindow p_Window,
                                        GraphicsDevice p_GraphicsDevice)
        {
            base.Initialize(p_Window, p_GraphicsDevice);
        }

        public override void Update(GameTime p_GameTime,
                                    in InputController p_InputController)
        {
            base.Update(p_GameTime, p_InputController);

            // Pause level while the dialoguebox is open.
            if (!DialogueBox.IsVisible()) {
            }
        }

        public override void Draw(GameTime p_GameTime,
                                  SpriteBatch p_SpriteBatch)
        {
            p_SpriteBatch.Begin();

            base.Draw(p_GameTime, p_SpriteBatch);

            p_SpriteBatch.End();
        }
    }
}