using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class ExprNode : Node<Token>
    {
        public ExprNode(Node<Token> parent, Node<Token> current) : base(parent, current)
        {
            if (current.Children.Count == 1)
            {
                this.AddChild(new ArithExprNode(this, current[0]), true);
            } else if (current.Children.Count > 1)
            {
                this.AddChild(new RelExprNode(new ArithExprNode(this, current[0]), current[1], new ArithExprNode(this, current[0])));
            }
        }

        public bool IsValid()
        {
            return Value.IsValid;
        }

        public void accept(Visitor<Token> visitor)
        {
            visitor.visit(this);
        }
    }
}