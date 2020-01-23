using System;

namespace TruCompiler
{
    /// <summary>
    /// Main program class to initialize the passed arguments and run the driver.
    /// </summary>
    class Program
    {
        private Program(string[] args)
        {

        }
        public static void Main(string[] args)
        {
            new Program(args).Run(); 
        }

        private void Run()
        {
            //Call driver with parsed args
        }
    }
}
