using System;

namespace JRPG
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (DemoGame demo = new DemoGame())
            {
                demo.Run();
            }
        }
    }
}
