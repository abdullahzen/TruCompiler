using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class RelExprNode : Node<Token>
    {
        public ArithExprNode LeftArithExpr { get; set; }
        public ArithExprNode RightArithExpr { get; set; }
        public Node<Token> RelOp { get; set; }
        public RelExprNode(ArithExprNode leftArithExpr, Node<Token> relOp, ArithExprNode rightArithExpr) : base ()
        {
            this.Value = new Token(Lexeme.keyword, "RelExpr");
            leftArithExpr.Parent = this;
            rightArithExpr.Parent = this;
            relOp.Parent = this;
            this.LeftArithExpr = leftArithExpr;
            this.RightArithExpr = rightArithExpr;
            this.RelOp = relOp;
            this.AddChild(leftArithExpr, false);
            this.AddChild(rightArithExpr, false);
            this.AddChild(relOp, false);
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