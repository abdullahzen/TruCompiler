using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class ArraySizeNode : Node<Token>
    {
        public int ArraySizeValue { get; set; }
     
        public ArraySizeNode(Node<Token> parent, Node<Token> current) : base(parent, current)
        {
            var intvalue = 0;
            int.TryParse(current[0].Value.Value, out intvalue);
            ArraySizeValue = intvalue;
            this.AddChild(current[0], true);
        }

        public bool IsValid()
        {
            return Value.IsValid;
        }

        public void accept(Visitor<Token> visitor)
        {
            visitor.visit(this);
        }
    }
}