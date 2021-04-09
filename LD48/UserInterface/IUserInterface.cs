using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD48.UserInterface
{
    public interface IUserInterface
    {
        void Draw(GameTime p_Time,
                  SpriteBatch p_SpriteBatch,
                  SpriteFont p_TitleFont);
    }
}