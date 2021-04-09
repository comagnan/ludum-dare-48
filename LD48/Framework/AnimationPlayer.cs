using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD48.Framework
{
    /// <summary>
    /// Controls playback of an Animation.
    /// </summary>
    public struct AnimationPlayer
    {
        /// <summary>
        /// Gets the animation which is currently playing.
        /// </summary>
        public Animation Animation { get; private set; }

        /// <summary>
        /// Gets the index of the current frame in the animation.
        /// </summary>
        public int FrameIndex { get; private set; }

        /// <summary>
        /// The amount of time in seconds that the current frame has been shown for.
        /// </summary>
        private float m_Time;

        /// <summary>
        /// Gets a texture origin at the center of each frame.
        /// </summary>
        public Vector2 Origin => new(Animation.FrameWidth / 2.0f, Animation.FrameHeight / 2.0f);

        private Random m_RandomSeed;
        private bool m_Random;

        /// <summary>
        /// Begins or continues playback of an animation.
        /// </summary>
        public void PlayAnimation(Animation p_Animation,
                                  bool p_Random = false)
        {
            m_Random = p_Random;
            if (m_Random && m_RandomSeed == null) {
                m_RandomSeed = new Random();
            }

            // If this animation is already running, do not restart it.
            if (Animation == p_Animation) {
                return;
            }

            // Start the new animation.
            Animation = p_Animation;
            FrameIndex = 0;
            m_Time = 0.0f;
        }

        public void Update(GameTime p_GameTime)
        {
            if (Animation == null) {
                return;
            }

            // Process passing time.
            m_Time += (float) p_GameTime.ElapsedGameTime.TotalMilliseconds;
            while (m_Time > Animation.FrameTimingInMs[FrameIndex]) {
                m_Time -= Animation.FrameTimingInMs[FrameIndex];

                // Advance the frame index; looping or clamping as appropriate.
                if (m_Random) {
                    int newFrame = m_RandomSeed.Next(0, Animation.FrameCount);
                    if (newFrame == FrameIndex) {
                        FrameIndex = (FrameIndex + 1) % Animation.FrameCount;
                    } else {
                        FrameIndex = newFrame;
                    }
                } else if (Animation.IsLooping) {
                    FrameIndex = (FrameIndex + 1) % Animation.FrameCount;
                } else {
                    FrameIndex = Math.Min(FrameIndex + 1, Animation.FrameCount - 1);
                }
            }
        }

        /// <summary>
        /// Advances the time position and draws the current frame of the animation.
        /// </summary>
        public void Draw(SpriteBatch p_SpriteBatch,
                         Vector2 p_Position,
                         SpriteEffects p_SpriteEffects,
                         bool p_Visible = true)
        {
            if (p_Visible) {
                // Calculate the source rectangle of the current frame.
                Rectangle source = new(FrameIndex * Animation.Texture.Height, 0, Animation.Texture.Height, Animation.Texture.Height);

                // Draw the current frame.
                p_SpriteBatch.Draw(Animation.Texture, p_Position, source, Color.White, 0.0f, Origin, 1.0f, p_SpriteEffects, 0.0f);
            }
        }
    }
}