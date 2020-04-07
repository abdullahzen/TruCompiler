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
            Node<Token> IdNode = new Node<Token>();
            IdNode.Parent = current[0].Parent;
            IdNode.Value = current[0].Value;
            Node<Token> dotNode = null;

            if (current[0].Children.Count > 0)
            {
                current[0].Children.ForEach(c =>
                {
                    if (c.Value != null && c.Value.Lexeme == Lexeme.dot)
                    {
                        dotNode = c;
                    }
                });
            }

            if (dotNode != null)
            {
                Left = (VariableNode)this.AddChild(new VariableNode(this, IdNode, dotNode), true);

            } else
            {
                Left = (VariableNode)this.AddChild(new VariableNode(this, IdNode), true);
            }

            if (current[0].Children.Count > 0)
            {
                Node<Token> tempAssign = null;
                current[0].Children.ForEach(c =>
                {
                    if (c.Value != null && c.Value.Value == "Assign")
                    {
                        tempAssign = c;
                    }
                });
                if (tempAssign != null)
                {
                    Right = (ExprNode)this.AddChild(new ExprNode(this, tempAssign[0]), true);
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