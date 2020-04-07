using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class ReturnStatementNode : StatementNode
    {
        public ExprNode Expression { get; set; }
        public ReturnStatementNode(Node<Token> parent, Node<Token> current) : base(parent, current, "ReturnStatement")
        {
            if (current.Children.Count > 1 && current[0].Value.Value == "return" && current[1].Value.Value == "Expr")
            {
                Expression = (ExprNode)this.AddChild(new ExprNode(this, current[1]), true);
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