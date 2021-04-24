using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using LD48.Dialogue;
using LD48.Framework.Input;
using LD48.Framework.TextBox;
using Microsoft.Xna.Framework;
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
        protected int LevelId;

        // GET CUSTOM OBJECT FOR THIS EVENTUALLY
        protected GraphicsDevice GraphicsDevice;
        protected Texture2D Rectangle;
        protected Texture2D Background;
        protected Texture2D Mascot;
        protected Box TextBox;

        protected SpriteFont EquationFont;

        // Level content.        
        protected ContentManager Content { get; }

        public bool IsLevelOver { get; protected set; }
        public Level NextLevel { get; protected set; }

        public Level(ContentManager p_Content,
                     int p_LevelId)
        {
            // Create a new content manager to load content used just by this level.
            Content = p_Content;
            DialogueBox = new DialogueBox(new Rectangle(20, 470, 1240, 240), new Vector2(100, 490), Content);
            LevelId = p_LevelId;
            GoalValue = 9;
            LevelPar = 3;
            m_Table = new DataTable();
            NumberBank = new List<char>();
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
            TextBox = new Box(new Rectangle(80, 240, 920, 530), 100, "", GraphicsDevice, EquationFont, Color.Black, Color.Aqua * 0.25f, 30);
        }

        public virtual void Update(GameTime p_GameTime,
                                   in InputController p_InputController)
        {
            DialogueBox.Update(p_GameTime, p_InputController);
            TextBox.Active = true;
            TextBox.Update();
        }

        public virtual void Draw(GameTime p_GameTime,
                                 SpriteBatch p_SpriteBatch)
        {
            p_SpriteBatch.Draw(Background, new Rectangle(0, 0, 1920, 1080), Color.White);
            DialogueBox.Draw(p_GameTime, p_SpriteBatch);
            p_SpriteBatch.Draw(Mascot, new Rectangle(1395, 450, 500, 600), Color.White);
            TextBox.Draw(p_SpriteBatch);
        }

        public void DrawInterface(GameTime p_GameTime,
                                  SpriteBatch p_SpriteBatch,
                                  SpriteFont p_SpriteFont)
        {
            p_SpriteBatch.DrawString(p_SpriteFont, LevelId.ToString(), new Vector2(180, 950), Color.Black);
            p_SpriteBatch.DrawString(p_SpriteFont, $"{GoalValue}", new Vector2(650, 950), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            p_SpriteBatch.DrawString(p_SpriteFont,
                $"{GetCurrentResult() ?? "?"}",
                new Vector2(1100, 950),
                Color.Black,
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                1f);
            p_SpriteBatch.DrawString(p_SpriteFont, "My super smart equation!", new Vector2(180, 80), Color.Black);
            p_SpriteBatch.DrawString(p_SpriteFont, "Bank", new Vector2(1120, 80), Color.Black);
            p_SpriteBatch.DrawString(p_SpriteFont,
                $"PAR: {LevelPar}",
                new Vector2(1420, 150),
                Color.Black,
                0f,
                Vector2.Zero,
                0.75f,
                SpriteEffects.None,
                1f);
            p_SpriteBatch.DrawString(p_SpriteFont,
                $"CURRENT SCORE: {GetScore()}",
                new Vector2(1420, 270),
                Color.Black,
                0f,
                Vector2.Zero,
                0.7f,
                SpriteEffects.None,
                1f);
            RenderBank(p_SpriteBatch, EquationFont);
        }

        protected string GetCurrentResult()
        {
            try {
                decimal currentValue = Convert.ToDecimal(m_Table.Compute(TextBox.Text.String, ""));
                if (currentValue > 9999) {
                    return "Hum...";
                }

                return decimal.Round(currentValue, 2).ToString(CultureInfo.InvariantCulture);
            } catch (Exception e) {
                return "???";
            }
        }

        protected string GetScore()
        {
            return (TextBox.Text.String.Count(char.IsDigit) - LevelPar).ToString();
        }

        private void RenderBank(SpriteBatch p_SpriteBatch,
                                SpriteFont p_SpriteFont)
        {
            for (int i = 0; i < NumberBank.Count; i++) {
                int column = i % 3;
                int row = (i - column) / 3;
                p_SpriteBatch.DrawString(p_SpriteFont, NumberBank[i].ToString(), new Vector2(1070 + 100 * column, 300 + 100 * row), Color.Black);
            }
        }
    }
}