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
            LevelWarning = "Use at most five operations!";
            DialogueBox.AddText(new DialogueEntry {
                Text = "That last hole got me thinking.",
                Speaker = GameInterface.Claire
            });
            DialogueBox.AddText(new DialogueEntry {
                Text = "What do you think they did... Before they invented the number 0?",
                Speaker = GameInterface.Claire
            });
            DialogueBox.AddText(new DialogueEntry {
                Text = "Do you think they finished a plate of cookies and went \"WHAT? WHAT KIND OF ARCANE NONSENSE AM I LOOKING AT\"?",
                Speaker = GameInterface.Claire
            });
            DialogueBox.AddText(new DialogueEntry {
                Text = "Oh! Right! Starting from now, the holes are going to have special rules you have to abide by.",
                Speaker = GameInterface.Claire
            });
            DialogueBox.AddText(new DialogueEntry {
                Text = "For this one, you have to use 5 operations or less. The funny symbols between the numbers. Parenthesis don't count though.",
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
                throw new PuzzleUnsolvedException("Bzzt! You've used too many operations! You need to stick to 5 or less.");
            }

            return base.IsEquationValid() && operationLimitRespected;
        }
    }
}