using System;
using System.Collections.Generic;
using System.Text;

namespace TruCompiler
{
    /// <summary>
    /// Driver class that is responsible for the main logic of TruCompiler.
    /// The below class includes calling and processing the lexical analyzer
    /// </summary>
    class Driver
    {
        public string[] InputFiles { get; private set; }
        public string OutputPath { get; private set; }
        public Driver(string[] inputFiles, string outputPath)
        {
            InputFiles = inputFiles;
            OutputPath = outputPath;
        }
        private void Compile()
        {

        }
    }
}
