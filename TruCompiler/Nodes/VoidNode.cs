using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class VoidNode : Node<Token>
    {
        public VoidNode(Node<Token> parent, Node<Token> current) : base(parent, current)
        {
        }

        public bool IsValid()
        {
            return Value.Value == "void" && Value.IsValid;
        }

        public override void accept(Visitor<Token> visitor)
        {
            visitor.visit(this);
        }
    }
}