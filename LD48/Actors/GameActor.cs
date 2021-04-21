using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD48.Actors
{
    /// <summary>
    ///     Implementation of shared logic for all game actors
    ///     (Player, bullet, enemies, etc.)
    /// </summary>
    public class GameActor : IGameActor
    {
        protected SpriteEffects Flip = SpriteEffects.None;
        protected Vector2 CurrentPosition { get; set; }
        protected Vector2 CurrentVelocity { get; set; }

        protected bool Solid { get; set; }
        protected Point CollisionBoxSize { get; set; }

        public virtual void Update(GameTime p_Time)
        {
            throw new NotImplementedException();
        }

        public virtual void Draw(GameTime p_GameTime,
            SpriteBatch p_SpriteBatch)
        {
            if (CurrentVelocity.X < 0)
                Flip = SpriteEffects.FlipHorizontally;
            else if (CurrentVelocity.X > 0) Flip = SpriteEffects.None;
        }

        public Vector2 GetPosition()
        {
            return CurrentPosition;
        }

        public Vector2 GetVelocity()
        {
            return CurrentVelocity;
        }

        public double GetDistanceFrom(GameActor p_Other)
        {
            (var x, var y) = CurrentPosition - p_Other.GetPosition();
            return Math.Sqrt(x * x + y * y);
        }

        protected void MoveActor(GameTime p_GameTime)
        {
            CurrentPosition = GetEstimatedPosition(p_GameTime);
        }

        protected Vector2 GetEstimatedPosition(GameTime p_GameTime)
        {
            var dx = 100 * CurrentVelocity.X * p_GameTime.ElapsedGameTime.TotalSeconds;
            var dy = 100 * CurrentVelocity.Y * p_GameTime.ElapsedGameTime.TotalSeconds;
            return new Vector2(CurrentPosition.X + (float) dx, CurrentPosition.Y + (float) dy);
        }

        public virtual Rectangle GetCollisionBox()
        {
            Point topLeft = new((int) (CurrentPosition.X - CollisionBoxSize.X / 2),
                (int) (CurrentPosition.Y - CollisionBoxSize.Y / 2));

            return new Rectangle(topLeft.X, topLeft.Y, CollisionBoxSize.X, CollisionBoxSize.Y);
        }

        public bool IsSolid()
        {
            return Solid;
        }
    }
}