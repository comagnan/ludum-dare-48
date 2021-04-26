using System;
using System.Collections.Generic;
using LD48.Content;
using LD48.Dialogue;
using LD48.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LD48.Framework.Levels
{
    public class Epilogue : Level
    {
        /// <summary>
        /// Constructs a new level.
        /// </summary>
        public Epilogue(ContentManager p_Content) : base(p_Content, 8, "\"Thanks for playing!\"")
        {
            NumberBank = new List<char> {
                '0',
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
            GoalValue = 9;
            LevelPar = 6;
            LevelZenPar = 8;
            DialogueBox.AddText(new DialogueEntry {
                Text = "Hmm? You want to play more?",
                Speaker = GameInterface.Claire
            });
            DialogueBox.AddText(new DialogueEntry {
                Text = "I like the enthusiasm! But we've been at this for a while now haha. I'm too tired to come up with anything new...",
                Speaker = GameInterface.Claire
            });
            DialogueBox.AddText(new DialogueEntry {
                Text = "Hmm... We could redo the first hole. But then we're done for the day, you hear?",
                Speaker = GameInterface.Claire,
                Callback = () => PlaySong(false)
            });
            LevelRemainingTime = TimeSpan.FromMinutes(5);
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

        protected override void FinishLevel()
        {
            StopSong();
            DialogueBox.AddText(new DialogueEntry {
                Text = "You did it again!",
                Speaker = GameInterface.Claire
            });
            DialogueBox.AddText(new DialogueEntry {
                Text = "Now I reaaaally need some shut eye though...",
                Speaker = GameInterface.Claire
            });
            DialogueBox.AddText(new DialogueEntry {
                Text = "So...",
                Speaker = GameInterface.Claire
            });
            DialogueBox.AddText(new DialogueEntry {
                Text = "Thanks for playing!",
                Speaker = GameInterface.Claire,
                Callback = () => IsLevelOver = true
            });
        }
    }
}