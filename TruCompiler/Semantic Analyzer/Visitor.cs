using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Semantic_Analyzer
{
    public abstract class Visitor<T>
    {
        public abstract void visit(Node<T> node);
    }
}
