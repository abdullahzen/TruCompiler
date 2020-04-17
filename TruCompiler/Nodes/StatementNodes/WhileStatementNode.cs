using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class WhileStatementNode : StatementNode
    {
        public RelExprNode RelExpr { get; set; }
        public StatBlockNode BodyStatBlock { get; set; }
        public WhileStatementNode(Node<Token> parent, Node<Token> current) : base(parent, current, "WhileStatement")
        {
            if (current.Children.Count > 1 && current[0].Value.Value == "while")
            {
                RelExpr = new RelExprNode(new ArithExprNode(this, current[1]), current[2], new ArithExprNode(this, current[3]));
                this.AddChild(RelExpr, true);
                if (current.Children.Count > 4)
                {
                    BodyStatBlock = new StatBlockNode(this, current[4], "WhileStatBlock");
                    this.AddChild(BodyStatBlock, true);
                }
            }
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