using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class ClassListNode : Node<Token>
    {
        public List<ClassNode> Classes { get; set; }
       
        public ClassListNode(Node<Token> parent, Node<Token> current) : base(parent, current)
        {
            Classes = new List<ClassNode>();
            current.Children.ForEach(c =>
            {
                Classes.Add((ClassNode)this.AddChild(new ClassNode(this, c), true));
            });
        }

        public bool IsValid()
        {
            return Value.Equals(new Token(Lexeme.keyword, "Classes")) && Value.IsValid;
        }

        public override void accept(Visitor<Token> visitor)
        {
            visitor.visit(this);
        }


    }
}