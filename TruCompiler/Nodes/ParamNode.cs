using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class ParamNode : Node<Token>
    {
        public TypeNode Type { get; set; }
        public IdNode Name { get; set; }
        public List<ArraySizeNode> ArraySize { get; set; }
        public ParamNode(Node<Token> parent, Node<Token> current) : base(parent, current)
        {
            if (current.Children.Count > 1)
            {
                Type = (TypeNode)this.AddChild(new TypeNode(this, current[0]), true);
                Name = (IdNode)this.AddChild(new IdNode(this, current[1]), true);
                if (current.Children.Count > 2)
                {
                    ArraySize = new List<ArraySizeNode>();
                    for (int i = 2; i < current.Children.Count; i++)
                    {
                        ArraySize.Add((ArraySizeNode)this.AddChild(new ArraySizeNode(this, current[i]), true));
                    }
                }
                else
                {
                    ArraySize = null;
                }
            } else
            {
                parent.RemoveChild(current);
            }
            
            this.Value.Line = current[0].Value.Line;
            this.Parent.Value.Line = current[0].Value.Line;
        }

        public bool IsValid()
        {
            return Value.Equals(new Token(Lexeme.keyword, "Param")) && Value.IsValid;
        }

        public override void accept(Visitor<Token> visitor)
        {
            visitor.visit(this);
        }
    }
}