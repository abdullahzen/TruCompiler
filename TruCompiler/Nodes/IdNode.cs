using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class IdNode : Node<Token>
    {
        public string IdValue { get; set; }
        public IdNode(Node<Token> parent, Node<Token> current) : base(parent, current)
        {
            IdValue = current.Value.Value;
        }

        public bool IsValid()
        {
            return Value.Equals(new Token(Lexeme.id)) && Value.IsValid;
        }

        public void accept(Visitor visitor)
        {
            visitor.visit(this);
        }


    }
}