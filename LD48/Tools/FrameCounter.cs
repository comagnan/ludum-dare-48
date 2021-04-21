using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace LD48.Tools
{
    public class FrameCounter
    {
        public const int MAXIMUM_SAMPLES = 5;
        private readonly Queue<float> m_SampleBuffer = new();
        public long TotalFrames { get; private set; }
        public float TotalSeconds { get; private set; }
        public float AverageFramesPerSecond { get; private set; }
        public float CurrentFramesPerSecond { get; private set; }

        public bool Update(GameTime p_GameTime)
        {
            var deltaTime = (float) p_GameTime.ElapsedGameTime.TotalSeconds;
            CurrentFramesPerSecond = 1 / deltaTime;

            m_SampleBuffer.Enqueue(CurrentFramesPerSecond);

            if (m_SampleBuffer.Count > MAXIMUM_SAMPLES)
            {
                m_SampleBuffer.Dequeue();
                AverageFramesPerSecond = (float) Math.Ceiling(m_SampleBuffer.Average(i => i));
            }
            else
            {
                AverageFramesPerSecond = CurrentFramesPerSecond;
            }

            TotalFrames++;
            TotalSeconds += deltaTime;
            return true;
        }
    }
}