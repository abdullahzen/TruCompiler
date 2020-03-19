using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class NegateFactorNode : Node<Token>
    {
        public Node<Token> Factor { get; set; }
        public NegateFactorNode(Node<Token> parent, Node<Token> factor) : base(parent, new Node<Token>(new Token(Lexeme.keyword, "NegateFactor")))
        {
            ArithExprNode.GetFactor(factor, this);
            Factor = this[0];
        }

        public bool IsValid()
        {
            return Value.IsValid;
        }

        public void accept(Visitor visitor)
        {
            visitor.visit(this);
        }
    }
}