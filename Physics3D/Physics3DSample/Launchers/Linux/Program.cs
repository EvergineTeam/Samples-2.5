using System;

namespace Physics3DSample
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

