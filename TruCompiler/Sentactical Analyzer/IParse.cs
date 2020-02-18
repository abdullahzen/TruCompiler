using System;
using System.Collections.Generic;
using System.Text;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Sentactical_Analyzer
{
    public interface IParse
    {
        bool match(Token token);
        void parse();
    }
}
