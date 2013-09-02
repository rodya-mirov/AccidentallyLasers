using System;

namespace AccidentallyLasers
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (LaserGame game = new LaserGame())
            {
                game.Run();
            }
        }
    }
#endif
}

