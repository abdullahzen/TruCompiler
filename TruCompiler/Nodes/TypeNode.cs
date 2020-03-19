using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class TypeNode : Node<Token>
    {
        public TypeNode(Node<Token> parent, Node<Token> current) : base(parent, current)
        {
        }

        public bool IsValid()
        {
            return Value.IsValid && (Value.Lexeme == Lexeme.id || (Value.Lexeme == Lexeme.keyword && (Value.Value == "float" || Value.Value == "integer")));
        }

        public void accept(Visitor visitor)
        {
            visitor.visit(this);
        }
    }
}