using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Sentactical_Analyzer
{
    public class TokenScanner
    {
        public List<Token> Tokens { get; private set; }
        public int index { get; private set; }

        public Token Current { get; private set; }

        public TokenScanner(List<Token> tokens)
        {
            Tokens = tokens;
            index = -1;
        }
        public Token NextToken()
        {
            index++;
            Current = Tokens.ElementAt(index);
            return Current;
        }

        public Token GetCurrent()
        {
            return Current;
        }

        public Token Peek()
        {
            if (hasNext())
            {
                return Tokens.ElementAt(index + 1);
            }
            return new Token();
        }

        public bool hasNext()
        {
            if ((index + 1) < Tokens.Count)
            {
                return true;
            }
            return false;
        }
    }
}
