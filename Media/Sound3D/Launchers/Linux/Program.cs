using System;

namespace Sound3D
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (App game = new App())
            {
                game.Run();
            }
        }
    }
}

