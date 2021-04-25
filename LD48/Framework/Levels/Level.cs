using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using LD48.Content;
using LD48.Dialogue;
using LD48.Framework.Input;
using LD48.Framework.TextBox;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LD48.Framework.Levels
{
    // As you can see, I've knocked over many chairs because I get so tilted at the towers.
    public class Level : IDisposable
    {
        private DataTable m_Table;

        protected readonly DialogueBox DialogueBox;

        // The puzzling happens here:
        protected List<char> NumberBank;
        protected int GoalValue;
        protected int LevelPar;
        protected string LevelName;
        protected string LevelWarning;

        // GET CUSTOM OBJECT FOR THIS EVENTUALLY
        protected GraphicsDevice GraphicsDevice;
        protected Texture2D Rectangle;
        protected Texture2D Background;
        protected Texture2D Mascot;
        protected Box TextBox;

        protected SpriteFont EquationFont;

        // Level content.        
        protected ContentManager Content { get; }
        public readonly int LevelId;

        public bool IsLevelOver { get; protected set; }

        public Level(ContentManager p_Content,
                     int p_LevelId,
                     string p_LevelName)
        {
            // Create a new content manager to load content used just by this level.
            Content = p_Content;
            DialogueBox = new DialogueBox(new Rectangle(340, 800, 1240, 240), new Vector2(420, 820), Content);
            LevelId = p_LevelId;
            GoalValue = 9;
            LevelPar = 3;
            m_Table = new DataTable();
            NumberBank = new List<char>();
            IsLevelOver = false;
            LevelName = p_LevelName;
        }

        public void Dispose()
        {
            Content.Unload();
        }

        // Must be called after initializing inheritors.
        public virtual void Initialize(GameWindow p_Window,
                                       GraphicsDevice p_GraphicsDevice)
        {
            GraphicsDevice = p_GraphicsDevice;
            Rectangle = new Texture2D(GraphicsDevice, 1, 1);
            Rectangle.SetData(new[] { Color.White });

            Background = Content.Load<Texture2D>("Interface/stage");
            Mascot = Content.Load<Texture2D>("Interface/mascot");
            EquationFont = Content.Load<SpriteFont>("Title");

            SoundEffect keyPressEffect = Content.Load<SoundEffect>("SFX/add");
            SoundEffect removeEffect = Content.Load<SoundEffect>("SFX/delete");
            TextBox = new Box(new Rectangle(260, 210, 860, 300),
                86,
                "",
                GraphicsDevice,
                EquationFont,
                Color.White,
                Color.Aqua * 0.25f,
                30,
                keyPressEffect,
                removeEffect);
        }

        public virtual void Update(GameTime p_GameTime,
                                   in InputController p_InputController)
        {
            if (!DialogueBox.IsVisible()) {
                TextBox.Active = true;
                TextBox.Update();

                if (p_InputController.IsButtonPress(InputConfiguration.Submit)) {
                    try {
                        if (IsEquationValid()) {
                            DialogueBox.AddText(new DialogueEntry {
                                Text = "Congratulations!",
                                Sprite = GameInterface.Beatrice,
                                Callback = () => IsLevelOver = true
                            });
                        }
                    } catch (PuzzleUnsolvedException e) {
                        DialogueBox.AddText(new DialogueEntry { Text = e.Message });
                    }
                }
            }

            DialogueBox.Update(p_GameTime, p_InputController);
        }

        public virtual void Draw(GameTime p_GameTime,
                                 SpriteBatch p_SpriteBatch)
        {
            p_SpriteBatch.Draw(Rectangle, new Rectangle(0, 0, 1920, 1080), Color.LightGreen);
            p_SpriteBatch.Draw(Background, new Rectangle(0, 0, 1920, 1080), Color.White);
            p_SpriteBatch.Draw(Mascot,
                new Vector2(1645, 750),
                new Rectangle(0, 0, 500, 600),
                Color.White,
                (float) Math.Sin(p_GameTime.TotalGameTime.TotalSeconds / 2f) / 8f,
                new Vector2(250, 300),
                1f,
                SpriteEffects.None,
                1f);
            TextBox.Draw(p_SpriteBatch);
        }

        public void DrawInterface(GameTime p_GameTime,
                                  SpriteBatch p_SpriteBatch,
                                  SpriteFont p_SpriteFont)
        {
            p_SpriteBatch.DrawString(p_SpriteFont,
                $"{GoalValue}",
                new Vector2(1720, 150),
                Color.Black,
                0f,
                p_SpriteFont.MeasureString(GoalValue.ToString()) / 2,
                2f,
                SpriteEffects.None,
                1f);
            p_SpriteBatch.DrawString(p_SpriteFont,
                $"{GetCurrentResult() ?? "?"}",
                new Vector2(1720, 400),
                Color.Black,
                0f,
                p_SpriteFont.MeasureString(GetCurrentResult()) / 2,
                1.75f,
                SpriteEffects.None,
                1f);
            p_SpriteBatch.DrawString(p_SpriteFont,
                LevelName,
                new Vector2(650, 85),
                Color.White,
                0f,
                p_SpriteFont.MeasureString(LevelName) * 3 / 8,
                0.75f,
                SpriteEffects.None,
                1f);
            p_SpriteBatch.DrawString(p_SpriteFont, "Bank", new Vector2(650, 810), Color.Black, 0f, new Vector2(5, 50), 0.75f, SpriteEffects.None, 1f);
            p_SpriteBatch.DrawString(p_SpriteFont,
                $"{LevelPar}",
                new Vector2(190, 920),
                Color.White,
                0f,
                p_SpriteFont.MeasureString(LevelPar.ToString()) / 2,
                2f,
                SpriteEffects.None,
                1f);
            p_SpriteBatch.DrawString(p_SpriteFont,
                $"{GetScore()}",
                new Vector2(335, 920),
                Color.White,
                0f,
                p_SpriteFont.MeasureString(GetScore().ToString()) / 2,
                2f,
                SpriteEffects.None,
                1f);
            RenderBank(p_SpriteBatch, EquationFont);
            DialogueBox.Draw(p_GameTime, p_SpriteBatch);
        }

        protected string GetCurrentResult()
        {
            try {
                decimal currentValue = Convert.ToDecimal(m_Table.Compute(TextBox.Text.String, ""));
                if (currentValue > 9999) {
                    return "Hum...";
                }

                return decimal.Round(currentValue, 2).ToString(CultureInfo.InvariantCulture);
            } catch (Exception) {
                return "???";
            }
        }

        protected int GetScore()
        {
            return TextBox.Text.String.Count(char.IsDigit);
        }

        protected virtual bool IsEquationValid()
        {
            bool correctValue = GoalValue.ToString() == GetCurrentResult();
            if (!correctValue) {
                throw new PuzzleUnsolvedException($"Wrong! We're looking for an equation that equals {GoalValue}.");
            }


            bool scorePositive = GetScore() >= LevelPar;
            if (!scorePositive) {
                throw new PuzzleUnsolvedException("Your equation is BORING! You have to go deeper!");
            }

            bool respectsBank = true;
            List<char> bankCopy = NumberBank.ToList();
            foreach (char character in TextBox.Text.Characters) {
                if (char.IsDigit(character)) {
                    if (bankCopy.Contains(character)) {
                        bankCopy.Remove(character);
                    } else {
                        respectsBank = false;
                    }
                }
            }

            if (!respectsBank) {
                throw new PuzzleUnsolvedException("Not quite -- you've used illegal digits in there! Check the allowed digits in the bank.");
            }

            return scorePositive && correctValue && respectsBank;
        }

        private void RenderBank(SpriteBatch p_SpriteBatch,
                                SpriteFont p_SpriteFont)
        {
            for (int i = 0; i < NumberBank.Count; i++) {
                int column = i % 10;
                int row = (i - column) / 3;
                p_SpriteBatch.DrawString(p_SpriteFont, NumberBank[i].ToString(), new Vector2(200 + 100 * column, 550 + 50 * row), Color.Black);
            }
        }
    }
}