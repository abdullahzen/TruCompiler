using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class AParamsNode : Node<Token>
    {
        public List<ExprNode> Expressions { get; set; }
        
        public AParamsNode(Node<Token> parent, Node<Token> current) : base(parent, current)
        {
            Expressions = new List<ExprNode>();
            current.Children.ForEach(c =>
            {
                Expressions.Add((ExprNode)this.AddChild(new ExprNode(this, c), true));
            });
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