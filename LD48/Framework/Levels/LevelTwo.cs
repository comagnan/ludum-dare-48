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
    public class LevelTwo : Level
    {
        /// <summary>
        /// Constructs a new level.
        /// </summary>
        public LevelTwo(ContentManager p_Content) : base(p_Content, 2, "\"Go Home\"")
        {
            NumberBank = new List<char> {
                '1',
                '2',
                '3',
                '4',
                '5',
                '6',
                '7',
                '8',
                '9'
            };
            GoalValue = 0;
            LevelPar = 4;
            DialogueBox.AddText(new DialogueEntry {
                Text = "See? I told you you were going to be a natural.",
                Speaker = GameInterface.Claire
            });
            DialogueBox.AddText(new DialogueEntry {
                Text = "The goal's not always going to be 9, of course.",
                Speaker = GameInterface.Claire
            });
            DialogueBox.AddText(new DialogueEntry {
                Text = "For this one, make something complicated that amounts to nothing!",
                Speaker = GameInterface.Claire,
                Callback = () => PlaySong(false)
            });
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