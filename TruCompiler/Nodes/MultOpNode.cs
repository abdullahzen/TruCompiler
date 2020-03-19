using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class MultOpNode : Node<Token>
    {
        public List<Node<Token>> Left { get; set; }
        public List<Node<Token>> Right { get; set; }
        public MultOpNode(Node<Token> parent, List<Node<Token>> left, List<Node<Token>> right) : base(parent, new Node<Token>(new Token(Lexeme.keyword, "MultOp")))
        {
            Left = new List<Node<Token>>();
            left.ForEach(c => 
            {
                Left.Add(this.AddChild(new ArithExprNode(this, c), true));
            });
            Right = new List<Node<Token>>();
            right.ForEach(c =>
            {
                right.Add(this.AddChild(new ArithExprNode(this, c), true));
            });
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