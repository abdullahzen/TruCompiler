using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class AssignStatementNode : StatementNode
    {
        public VariableNode Left { get; set; }
        public ExprNode Right { get; set; }
        public AssignStatementNode(Node<Token> parent, Node<Token> current) : base(parent, current, "AssignmentStatement")
        {
            Left = (VariableNode)this.AddChild(new VariableNode(this, current, current[0]), true);
            Right = (ExprNode)this.AddChild(new ExprNode(this, current[1]), true);
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