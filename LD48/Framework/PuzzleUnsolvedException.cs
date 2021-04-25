using System;

namespace LD48.Framework
{
    public class PuzzleUnsolvedException : Exception
    {
        public PuzzleUnsolvedException(string p_Message) : base(p_Message)
        {
        }
    }
}