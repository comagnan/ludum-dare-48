using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace LD48.Framework
{
    /// <summary>
    /// Represents an animated texture.
    /// </summary>
    public class Animation
    {
        private Texture2D texture; 
        private List<int> frameTimingInMs;
        private int frameWidth;

        /// <summary>
        /// All frames in the animation arranged horizontally.
        /// </summary>
        public Texture2D Texture => texture;

        /// <summary>
        /// Duration of time to show each frame.
        /// </summary>
        public List<int> FrameTimingInMs => frameTimingInMs;

        /// <summary>
        /// When the end of the animation is reached, should it
        /// continue playing from the beginning?
        /// </summary>
        public bool IsLooping { get; }

        /// <summary>
        /// Gets the width of a frame in the animation.
        /// </summary>
        public int FrameWidth => frameWidth;

        /// <summary>
        /// Gets the height of a frame in the animation.
        /// </summary>
        public int FrameHeight => Texture.Height;

        /// <summary>
        /// Gets the number of frames in an animation.
        /// </summary>
        public int FrameCount => FrameTimingInMs.Count;

        /// <summary>
        /// Constructors a new animation.
        /// </summary>        
        public Animation(Texture2D p_Texture,
                         List<int> p_FrameTiming,
                         int p_FrameWidth,
                         bool p_IsLooping)
        {
            texture = p_Texture;
            frameTimingInMs = p_FrameTiming;
            IsLooping = p_IsLooping;
            frameWidth = p_FrameWidth;
        }
    }
}