using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class AssignOpNode : Node<Token>
    {
        public AssignOpNode() : base(new Token(Lexeme.eq))
        {
        }

        public AssignOpNode(Node<Token> subtree) : base(subtree)
        {
            Value = new Token(Lexeme.eq);
        }

        public AssignOpNode(Node<Token> subtree, Node<Token> parent) : base(subtree, parent, new Token(Lexeme.eq))
        {
        }

        public bool IsValid()
        {
            return Value.Equals(new Token(Lexeme.eq)) && Value.IsValid;
        }

        public void accept(Visitor<Token> visitor)
        {
            visitor.visit(this);
        }


    }
}