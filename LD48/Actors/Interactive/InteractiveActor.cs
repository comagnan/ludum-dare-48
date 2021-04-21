using System;
using LD48.Dialogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD48.Actors.Interactive
{
    /// <summary>
    ///     Implementation of shared logic for all game actors
    ///     that can be interacted with.
    /// </summary>
    public class InteractiveActor : GameActor
    {
        protected bool Enabled;
        protected bool IsActive;
        private Vector2 m_BubbleOffset;
        private readonly Texture2D m_BubbleTexture;

        public InteractiveActor(Texture2D p_BubbleTexture)
        {
            IsActive = false;
            Enabled = true;
            m_BubbleTexture = p_BubbleTexture;
            m_BubbleOffset = new Vector2(-4, 0);
        }

        public void Update(GameTime p_Time, in PlayerActor p_Player)
        {
            IsActive = GetDistanceFrom(p_Player) < 30;
            if (!IsActive) // Reenable interaction after initial trigger if outside of the zone.
                Enabled = true;
            m_BubbleOffset.Y = (int) (Math.Sin(p_Time.TotalGameTime.TotalMilliseconds / 200) * 4) - 20;
        }

        public override void Draw(GameTime p_GameTime,
            SpriteBatch p_SpriteBatch)
        {
            base.Draw(p_GameTime, p_SpriteBatch);

            if (IsActive && Enabled)
                p_SpriteBatch.Draw(m_BubbleTexture,
                    new Vector2(CurrentPosition.X, (int) (CurrentPosition.Y + m_BubbleOffset.Y)), Color.White);
        }

        public virtual void Interact(in DialogueBox p_DialogueBox)
        {
            Enabled = false;
        }
    }
}