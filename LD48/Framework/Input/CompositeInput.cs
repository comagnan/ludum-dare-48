using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace LD48.Framework.Input
{
    public class CompositeInput
    {
        public IList<Buttons> ButtonInputs { get; set; }
        public IList<Keys> KeyInputs { get; set; }
    }
}