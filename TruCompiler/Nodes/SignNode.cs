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
        public SignNode(Node<Token> parent, Node<Token> sign, Node<Token> factor) : base(parent, new Node<Token>(new Token(Lexeme.keyword, "Signed")))
        {
            if (sign.Value.Lexeme == Lexeme.minus)
            {
                Sign = "-";
            } else
            {
                Sign = "+";
            }
            this.AddChild(sign, false);
            ArithExprNode.GetFactor(factor, this);
            Factor = this[1];
        }

        public SignNode(Node<Token> parent, Node<Token> current) : base(parent, new Node<Token>(new Token(Lexeme.keyword, "Signed")))
        {
            if (current[0][0].Value.Lexeme == Lexeme.minus)
            {
                Sign = "-";
            }
            else
            {
                Sign = "+";
            }
            current[0][0].Parent = this;
            this.AddChild(current[0][0], false);
            current.RemoveAt(0);
            ArithExprNode.GetFactor(current, this);
            Factor = this[1];
        }

        public bool IsValid()
        {
            return Value.IsValid;
        }

        public override void accept(Visitor<Token> visitor)
        {
            visitor.visit(this);
        }
    }
}