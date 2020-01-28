using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TruCompiler.Lexical_Analyzer;

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
        public void Compile()
        {
            foreach (string file in InputFiles)
            {
                IDictionary<string, IList<LexicalAnalyzer.Token?>> tokens = new Dictionary<string, IList<LexicalAnalyzer.Token?>>();
                if (File.Exists(file))
                {
                    tokens.Add(file, LexicalAnalyzer.Tokenize(File.ReadAllLines(file)));
                    
                }
            }
        }
    }
}
