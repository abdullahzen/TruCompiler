using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class NumNode : Node<Token>
    {
        public int IntValue { get; set; }
        public float FloatValue { get; set; }
        public string Type { get; set; }
        public NumNode(Node<Token> parent, Node<Token> current) : base(parent, current)
        {
            if (current.Value.Lexeme == Lexeme.intnum)
            {
                var integer = 0;
                int.TryParse(current.Value.Value, out integer);
                IntValue = integer;
                Type = "integer";
            } else
            {
                float floating = 0;
                float.TryParse(current.Value.Value, out floating);
                FloatValue = floating;
                Type = "float";
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