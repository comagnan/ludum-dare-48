using System;

namespace LD48
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            using GameCore game = new();
            game.Run();
        }
    }
}