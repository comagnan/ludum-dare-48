using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD48.Framework.TextBox
{
    public class Cursor
    {
        public Color Color { get; set; }
        public Color Selection { get; set; }
        public Rectangle Icon { get; set; }

        public bool Active { get; set; }

        private bool visible;
        private readonly int ticksPerBlink;
        private int ticks;

        /// <summary>
        ///     The current location of the cursor in the array
        /// </summary>
        public int TextCursor
        {
            get { return textCursor; }
            set { textCursor = value.Clamp(0, m_Box.Text.Length); }
        }

        /// <summary>
        ///     All characters between SelectedChar and the TextCursor are selected
        ///     when SelectedChar != null. Cannot be the same as the TextCursor value.
        /// </summary>
        public int? SelectedChar
        {
            get { return selectedChar; }
            set
            {
                if (value.HasValue)
                {
                    if (value.Value != TextCursor)
                    {
                        selectedChar = (short) value.Value.Clamp(0, m_Box.Text.Length);
                    }
                }
                else
                {
                    selectedChar = null;
                }
            }
        }

        private readonly Box m_Box;

        private int textCursor;
        private int? selectedChar;

        public Cursor(Box p_Box, Color color, Color selection, Rectangle icon, int ticksPerBlink)
        {
            m_Box = p_Box;
            Color = color;
            Selection = selection;
            Icon = icon;
            Active = true;
            visible = false;
            this.ticksPerBlink = ticksPerBlink;
            ticks = 0;
        }

        public void Update()
        {
            ticks++;

            if (ticks <= ticksPerBlink)
            {
                return;
            }

            visible = !visible;
            ticks = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Top left corner of the text area.
            int x = m_Box.Renderer.Area.X;
            int y = m_Box.Renderer.Area.Y;

            Point cp = GetPosition(x, y, TextCursor);
            if (selectedChar.HasValue)
            {
                Point sc = GetPosition(x, y, selectedChar.Value);
                if (sc.X > cp.X)
                {
                    spriteBatch.Draw(spriteBatch.GetWhitePixel(),
                        new Rectangle(cp.X, cp.Y, sc.X - cp.X, m_Box.Renderer.Font.LineSpacing), Icon, Selection);
                }
                else
                {
                    spriteBatch.Draw(spriteBatch.GetWhitePixel(),
                        new Rectangle(sc.X, sc.Y, cp.X - sc.X, m_Box.Renderer.Font.LineSpacing), Icon, Selection);
                }
            }

            if (!visible)
            {
                return;
            }

            spriteBatch.Draw(spriteBatch.GetWhitePixel(),
                new Rectangle(cp.X, cp.Y, Icon.Width, m_Box.Renderer.Font.LineSpacing), Icon, Color);
        }

        private Point GetPosition(int x, int y, int pos)
        {
            if (pos > 0)
            {
                if (m_Box.Text.Characters[pos - 1] == '\n'
                    || m_Box.Text.Characters[pos - 1] == '\r')
                {
                    // Beginning of next line.
                    y += m_Box.Renderer.Y[pos - 1] + m_Box.Renderer.Font.LineSpacing;
                }
                else if (pos == m_Box.Text.Length)
                {
                    // After last character.
                    x += m_Box.Renderer.X[pos - 1] + m_Box.Renderer.Width[pos - 1];
                    y += m_Box.Renderer.Y[pos - 1];
                }
                else
                {
                    // Beginning of current character.
                    x += m_Box.Renderer.X[pos];
                    y += m_Box.Renderer.Y[pos];
                }
            }
            return new Point(x, y);
        }
    }
}