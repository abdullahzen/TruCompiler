using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class SignNode : Node<Token>
    {
        public string Sign { get; set; }
        public Node<Token> Factor { get; set; }
        public SignNode(Node<Token> parent, Node<Token> sign, Node<Token> factor) : base(parent, new Node<Token>(new Token(Lexeme.keyword, "Sign")))
        {
            if (sign.Value.Lexeme == Lexeme.minus)
            {
                Sign = "-";
            } else
            {
                Sign = "+";
            }
            ArithExprNode.GetFactor(factor, this);
            Factor = this[0];
        }

        public bool IsValid()
        {
            return Value.IsValid;
        }

        public void accept(Visitor visitor)
        {
            visitor.visit(this);
        }
    }
}