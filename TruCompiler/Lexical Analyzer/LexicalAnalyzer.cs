using System;
using System.Collections.Generic;
using System.Text;

namespace TruCompiler.Lexical_Analyzer
{
    public class LexicalAnalyzer
    {
        public enum Lexeme {
            openoperand
        }
        public static List<Token> Tokenize(string[] lines)
        {

            return null;
        }

        public struct Token
        {
            public Lexeme Lexeme { get; set; }
            public string Value { get; set; }
            public int Location { get; set; }
            public bool IsValid { get; set; }
        }
    }
}
