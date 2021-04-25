﻿using System.Collections.Generic;
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
    public class LevelSix : Level
    {
        /// <summary>
        /// Constructs a new level.
        /// </summary>
        public LevelSix(ContentManager p_Content) : base(p_Content, 6, "No Going Back")
        {
            NumberBank = new List<char> {
                '1',
                '1',
                '2',
                '3',
                '4',
                '5',
                '6',
                '8',
                '9',
                '9'
            };
            GoalValue = 13;
            LevelPar = 6;
            LevelWarning = "\"-\" is forbidden!";
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
            bool noMinus = TextBox.Text.String.Count(x => x == '-') == 0;
            if (!noMinus) {
                throw new PuzzleUnsolvedException("Nope! You can't use subtraction in this one.");
            }

            return base.IsEquationValid() && noMinus;
        }
    }
}