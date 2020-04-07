using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class LocalNode : Node<Token>
    {
        public List<VariableDeclNode> Variables { get; set; }
        public LocalNode(Node<Token> parent, Node<Token> current) : base(parent, current)
        {
            Variables = new List<VariableDeclNode>();
            current.Children.ForEach(c =>
            {
                Variables.Add((VariableDeclNode)this.AddChild(new VariableDeclNode(this, c), true));
            });
        }

        public bool IsValid()
        {
            return Value.Equals(new Token(Lexeme.keyword, "local")) && Value.IsValid;
        }

        public override void accept(Visitor<Token> visitor)
        {
            visitor.visit(this);
        }
    }
}