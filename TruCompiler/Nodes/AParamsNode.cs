using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class FParamsNode : Node<Token>
    {
        public List<ParamNode> Params { get; set; }
        
        public FParamsNode(Node<Token> parent, Node<Token> current) : base(parent, current)
        {
            Params = new List<ParamNode>();
            current.Children.ForEach(c =>
            {
                Params.Add((ParamNode)this.AddChild(new ParamNode(this, c), true));
            });
        }

        public bool IsValid()
        {
            return Value.Equals(new Token(Lexeme.keyword, "FParams")) && Value.IsValid;
        }

        public void accept(Visitor visitor)
        {
            visitor.visit(this);
        }
    }
}