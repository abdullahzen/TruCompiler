using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class MainNode : Node<Token>
    {
        public FuncBodyNode FuncBody { get; set; }
        public MainNode(Node<Token> parent, Node<Token> current) : base(parent, current)
        {
            if (current.Children.Count > 0)
            {
                FuncBody = (FuncBodyNode)this.AddChild(new FuncBodyNode(this, current[0]), true);
            }
        }

        public bool IsValid()
        {
            return Value.Equals(new Token(Lexeme.keyword, "main")) && Value.IsValid;
        }

        public override void accept(Visitor<Token> visitor)
        {
            visitor.visit(this);
        }
    }
}