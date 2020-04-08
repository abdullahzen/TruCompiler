using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class VariableDeclNode : Node<Token>
    {
        public TypeNode Type { get; set; }
        public IdNode Name { get; set; }
        public ArraySizeNode ArraySize { get; set; }
        public VariableDeclNode(Node<Token> parent, Node<Token> current) : base(parent, current)
        {
            Type = (TypeNode)this.AddChild(new TypeNode(this, current[0]), true);
            Name = (IdNode)this.AddChild(new IdNode(this, current[1]), true);
            if (current.Children.Count > 2)
            {
                ArraySize = (ArraySizeNode)this.AddChild(new ArraySizeNode(this, current[2]), true);
            } else
            {
                ArraySize = null;
            }
            this.Value.Line = current[0].Value.Line;
        }

        public bool IsValid()
        {
            return Value.Equals(new Token(Lexeme.keyword, "Variable")) && Value.IsValid;
        }

        public override void accept(Visitor<Token> visitor)
        {
            visitor.visit(this);
        }
    }
}