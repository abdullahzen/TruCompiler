using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Sentactical_Analyzer
{
    public class SyntacticalAnalyzer
    { 
        public static void AnalyzeSyntax(TokenScanner tokenScanner)
        {
            Rules rules = new Rules(tokenScanner);
            rules.Start();
        }
    }
}
