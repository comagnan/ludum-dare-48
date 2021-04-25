using System.Collections.Generic;
using LD48.Content;
using LD48.Dialogue;
using LD48.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;

namespace LD48.Framework.Levels
{
    public class FifthLevel : Level
    {
        /// <summary>
        /// Constructs a new level.
        /// </summary>
        public FifthLevel(ContentManager p_Content) : base(p_Content, 5)
        {
            NumberBank = new List<char> {
                '0',
                '1',
                '1',
                '2',
                '2',
                '3',
                '3',
                '4',
                '4',
                '5'
            };
            GoalValue = 156;
            LevelPar = 4;
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

            // Pause level while the dialogue box is open.
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