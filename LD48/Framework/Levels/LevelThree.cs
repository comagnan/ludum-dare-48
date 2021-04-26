using System;
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
    public class LevelThree : Level
    {
        /// <summary>
        /// Constructs a new level.
        /// </summary>
        public LevelThree(ContentManager p_Content) : base(p_Content, 3, "\"Seeing Double\"")
        {
            NumberBank = new List<char> {
                '2',
                '2',
                '4',
                '4',
                '6',
                '6',
                '8',
                '8'
            };
            GoalValue = 9;
            LevelPar = 3;
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
    }
}