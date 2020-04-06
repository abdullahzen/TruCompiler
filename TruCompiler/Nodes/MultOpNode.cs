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
        public Node<Token> Left { get; set; }
        public Node<Token> Right { get; set; }
        public Node<Token> Operation { get; set; }
        public MultOpNode(Node<Token> parent, List<Node<Token>> left, List<Node<Token>> right, Node<Token> op) : base(parent, new Node<Token>(new Token(Lexeme.keyword, "MultOp")))
        {
            Node<Token> leftFactor = new Node<Token>();
            leftFactor.Parent = this;
            leftFactor.Value = new Token(Lexeme.keyword, "LeftFactor");
            leftFactor.Children = left;
            Left = this.AddChild(new ArithExprNode(this, leftFactor), true);

            Node<Token> rightFactor = new Node<Token>();
            rightFactor.Parent = this;
            rightFactor.Value = new Token(Lexeme.keyword, "RightFactor");
            rightFactor.Children = right;
            Right = this.AddChild(new ArithExprNode(this, rightFactor), true);

            Operation = op;
            Operation.Parent = null;
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