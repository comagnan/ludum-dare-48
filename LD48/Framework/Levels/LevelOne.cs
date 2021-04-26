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
    public class LevelOne : Level
    {
        /// <summary>
        /// Constructs a new level.
        /// </summary>
        public LevelOne(ContentManager p_Content) : base(p_Content, 1, "\"Humble Beginnings\"")
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
            DialogueBox.AddText(new DialogueEntry {
                Text = "Hello there! Want to play a quick game of Fore?",
                Speaker = GameInterface.Claire
            });
            DialogueBox.AddText(new DialogueEntry {
                Text = "You don't know what Fore is? Are you kidding me?",
                Speaker = GameInterface.Claire
            });
            DialogueBox.AddText(new DialogueEntry {
                Text = "Sorry, sorry. I'll tell you how it works.",
                Speaker = GameInterface.Claire
            });
            DialogueBox.AddText(new DialogueEntry {
                Text = "See the goal in the top right? You want to come up with an equation that equals this!",
                Speaker = GameInterface.Claire
            });
            DialogueBox.AddText(new DialogueEntry {
                Text = "So here, the goal is 9 right? I know what you're thinking -- \"That's easy, 4+5\"! But there's a twist.",
                Speaker = GameInterface.Claire
            });
            DialogueBox.AddText(new DialogueEntry {
                Text = "For starters, you can only use the digits in the Bank in the bottom half.",
                Speaker = GameInterface.Claire
            });
            DialogueBox.AddText(new DialogueEntry {
                Text = "You can only use the digits in the Bank once each, too.",
                Speaker = GameInterface.Claire
            });
            DialogueBox.AddText(new DialogueEntry {
                Text = "But, more importantly, the true goal is make the longest, most complicated equation you can think of.",
                Speaker = GameInterface.Claire
            });
            DialogueBox.AddText(new DialogueEntry {
                Text = "Instead of using 4 and 5 to make \"4+5\", you could use 1, 2 and 3 to make \"12-3\" for example.",
                Speaker = GameInterface.Claire
            });
            DialogueBox.AddText(new DialogueEntry {
                Text = "But why stop there! Go deeper and deeper, find wonderful twisted paths only your mind can come up with!",
                Speaker = GameInterface.Claire
            });
            DialogueBox.AddText(new DialogueEntry {
                Text = "Anyways, there's a \"Par\" at the bottom of the screen. That's the minimum complexity to clear a hole...",
                Speaker = GameInterface.Claire
            });
            DialogueBox.AddText(new DialogueEntry {
                Text = "But you can go as deep as you want! Or, until you run out of digits in the Bank. Whatever comes first.",
                Speaker = GameInterface.Claire
            });
            DialogueBox.AddText(new DialogueEntry {
                Text = "Only digits count towards your score, by the way. You don't have to things like \"----3\" or (((5))). Unless you want to.",
                Speaker = GameInterface.Claire
            });
            DialogueBox.AddText(new DialogueEntry {
                Text = "Give it a shot! I'm sure you'll do great.",
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