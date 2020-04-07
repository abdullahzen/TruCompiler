using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class InheritanceListNode : Node<Token>
    {
        public List<IdNode> Classes { get; set; }
        public InheritanceListNode() : base(new Token(Lexeme.keyword, "InheritanceList"))
        {
        }

        public InheritanceListNode(Node<Token> subtree) : base(subtree)
        {
            Value = new Token(Lexeme.keyword, "InheritanceList");
        }

        public InheritanceListNode(Node<Token> parent, Node<Token> current) : base(parent, current)
        {
            Classes = new List<IdNode>();
            current.Children.ForEach(c =>
            {
                Classes.Add((IdNode)this.AddChild(new IdNode(this, c), true));
            });
        }

        public bool IsValid()
        {
            return Value.Equals(new Token(Lexeme.keyword, "InheritanceList")) && Value.IsValid;
        }

        public override void accept(Visitor<Token> visitor)
        {
            visitor.visit(this);
        }


    }
}