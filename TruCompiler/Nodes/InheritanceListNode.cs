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
        public List<ClassNode> Classes { get; set; }
        public InheritanceListNode() : base(new Token(Lexeme.keyword, "InheritanceList"))
        {
        }

        public InheritanceListNode(Node<Token> subtree) : base(subtree)
        {
            Value = new Token(Lexeme.keyword, "InheritanceList");
        }

        public InheritanceListNode(Node<Token> parent, Node<Token> current) : base(parent, current)
        {
            Classes = new List<ClassNode>();
            current.Children.ForEach(c =>
            {
                Classes.Add((ClassNode)this.AddChild(new ClassNode(this, c), true));
            });
        }

        public bool IsValid()
        {
            return Value.Equals(new Token(Lexeme.keyword, "InheritanceList")) && Value.IsValid;
        }

        public void accept(Visitor visitor)
        {
            visitor.visit(this);
        }


    }
}