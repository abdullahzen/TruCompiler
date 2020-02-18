﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TruCompiler.Lexical_Analyzer;
using TruCompiler.Sentactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

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
            try
            {
                foreach (string file in InputFiles)
                {
                    //Generate Tokens through the lexical analyzer
                    IDictionary<string, IList<Token>> tokens = new Dictionary<string, IList<Token>>();
                    if (File.Exists(file))
                    {
                        tokens.Add(file, LexicalAnalyzer.Tokenize(File.ReadAllLines(file)));

                        //Output to file by default the file location is in the same location as the source file
                        string outlextokens = Tokens.ToString(tokens[file].Where<Token>(t => t.IsValid));
                        string outlexerrors = Tokens.ToString(tokens[file].Where<Token>(t => t != null && !t.IsValid));

                        if (String.IsNullOrEmpty(OutputPath))
                        {
                            OutputPath = file.Substring(0, file.LastIndexOf("\\"));
                        }

                        if (Directory.Exists(OutputPath))
                        {
                            string outlextokensFile = OutputPath + file.Substring(file.LastIndexOf("\\"), file.LastIndexOf(".") - OutputPath.Length) + ".outlextokens";
                            string outlexerrorsFile = OutputPath + file.Substring(file.LastIndexOf("\\"), file.LastIndexOf(".") - OutputPath.Length) + ".outlexerrors";

                            WriteToFile(outlextokensFile, outlextokens);
                            WriteToFile(outlexerrorsFile, outlexerrors);
                        }   
                    }
                    //Generate Abstract syntax tree through the syntactical analyzer
                    List<Token> nonNullableTokens;
                    foreach(string key in tokens.Keys) {
                        nonNullableTokens = new List<Token>();
                        nonNullableTokens = tokens[key].Where(t => t != null 
                        && t.Lexeme != Lexeme.inlinecmt
                        && t.Lexeme != Lexeme.blockcmt
                        && t.Lexeme != Lexeme.closecmt
                        && t.Lexeme != Lexeme.opencmt).ToList();
                        TokenScanner tokenScanner = new TokenScanner(nonNullableTokens);
                        SyntacticalAnalyzer.AnalyzeSyntax(tokenScanner);
                    }
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
        private void WriteToFile(string filePath, string input)
        {
            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                writer.Write(input);
            }
        }
    }
}

