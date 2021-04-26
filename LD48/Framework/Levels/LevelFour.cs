using System;
using System.Collections.Generic;
using System.Linq;
using LD48.Content;
using LD48.Dialogue;
using LD48.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;

namespace LD48.Framework.Levels
{
    public class LevelFour : Level
    {
        /// <summary>
        /// Constructs a new level.
        /// </summary>
        public LevelFour(ContentManager p_Content) : base(p_Content, 4, "\"Too Much of a Good Thing\"")
        {
            NumberBank = new List<char> {
                '1',
                '2',
                '3',
                '3',
                '4',
                '4',
                '4',
                '4',
                '5',
                '5'
            };
            GoalValue = 27;
            LevelPar = 3;
            LevelZenPar = 7;
            LevelWarning = "Use at most five operators!";
            DialogueBox.AddText(new DialogueEntry {
                Text = "Welcome to Fore!",
                Speaker = GameInterface.Claire,
                Callback = () => PlaySong(false)
            });
            LevelRemainingTime = TimeSpan.FromMinutes(6);
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
            bool operationLimitRespected = TextBox.Text.String.Count(x => x == '/' || x == '*' || x == '-' || x == '+') <= 5;

            if (!operationLimitRespected) {
                throw new PuzzleUnsolvedException("Bzzt! You've used too many operators! You need to stick to 5 or less.");
            }

            return base.IsEquationValid() && operationLimitRespected;
        }
    }
}