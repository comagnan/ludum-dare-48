using System.Collections.Generic;
using System.Linq;
using LD48.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LD48.Framework.Levels
{
    public class LevelSeven : Level
    {
        /// <summary>
        /// Constructs a new level.
        /// </summary>
        public LevelSeven(ContentManager p_Content) : base(p_Content, 7, "Lady Luck")
        {
            NumberBank = new List<char> {
                '1',
                '2',
                '2',
                '3',
                '3',
                '4',
                '5',
                '6',
                '7',
                '7',
                '7',
                '8',
                '9',
                '9'
            };
            GoalValue = 17;
            LevelPar = 5;
            LevelZenPar = 8;
            LevelWarning = "You must use all sevens!";
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

        protected override bool IsEquationValid()
        {
            bool allSevens = TextBox.Text.String.Count(x => x == '7') == 3;

            if (!allSevens) {
                throw new PuzzleUnsolvedException("There's... You don't have the right number of sevens in there.");
            }

            return base.IsEquationValid() && allSevens;
        }
    }
}