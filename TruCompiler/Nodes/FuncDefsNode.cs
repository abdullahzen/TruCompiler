using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class FuncDefsNode : Node<Token>
    {
        public List<FuncDefNode> FunctionDefitions { get; set; }
        public FuncDefsNode(Node<Token> parent, Node<Token> current) : base(parent, current)
        {
            FunctionDefitions = new List<FuncDefNode>();
            current.Children.ForEach(c =>
            {
                FunctionDefitions.Add((FuncDefNode)this.AddChild(new FuncDefNode(this, c), true));
            });
        }

        public bool IsValid()
        {
            return Value.Equals(new Token(Lexeme.keyword, "FunctionDefitions")) && Value.IsValid;
        }

        public void accept(Visitor<Token> visitor)
        {
            visitor.visit(this);
        }
    }
}