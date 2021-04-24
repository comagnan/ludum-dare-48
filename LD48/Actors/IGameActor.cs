using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD48.Actors
{
    public interface IGameActor
    {
        void Update(GameTime p_GameTime);
        void Draw(GameTime p_Time, SpriteBatch p_SpriteBatch);
        Vector2 GetPosition();
        Vector2 GetVelocity();
    }
}