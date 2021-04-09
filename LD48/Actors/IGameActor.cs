using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LD48.Actors
{
    public interface IGameActor: IUpdate
    {
        void Draw(GameTime p_Time, SpriteBatch p_SpriteBatch);
        Vector2 GetPosition();
        Vector2 GetVelocity();
    }
}