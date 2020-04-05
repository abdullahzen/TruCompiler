using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class StartNode : Node<Token>
    {
        public ProgNode Program { get; set; }
        
        public StartNode(Node<Token> parent, Node<Token> current) : base(parent, current)
        {
            Program = (ProgNode)this.AddChild(new ProgNode(this, current[0]), true);
        }

        public bool IsValid()
        {
            return Value.Equals(new Token(Lexeme.keyword, "Start")) && Value.IsValid;
        }

        public void accept(Visitor<Token> visitor)
        {
            visitor.visit(this);
        }
    }
}