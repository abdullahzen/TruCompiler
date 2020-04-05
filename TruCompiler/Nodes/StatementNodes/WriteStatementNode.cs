using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class WriteStatementNode : StatementNode
    {
        public ExprNode Expression { get; set; }
        public WriteStatementNode(Node<Token> parent, Node<Token> current) : base(parent, current, "WriteStatement")
        {
            if (current.Children.Count > 1 && current[0].Value.Value == "write" && current[1].Value.Value == "Expr")
            {
                Expression = (ExprNode)this.AddChild(new ExprNode(this, current[1]), true);
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