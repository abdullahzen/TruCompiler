using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class FuncBodyNode : Node<Token>
    {
        public LocalNode Local { get; set; }
        public List<StatementNode> Statements { get; set; }
        public FuncBodyNode(Node<Token> parent, Node<Token> current) : base(parent, current)
        {
            if (current.Children.Count > 0)
            {
                int start = 0;
                if (current[0].Value.Value == "local")
                {
                    Local = new LocalNode(this, current[0]);
                    this.AddChild(Local, false);
                    start = 1;
                }
                Statements = StatementNode.GenerateStatements(start, current, this);
            }
        }

        public bool IsValid()
        {
            return Value.Equals(new Token(Lexeme.keyword, "FuncBody")) && Value.IsValid;
        }

        public void accept(Visitor<Token> visitor)
        {
            visitor.visit(this);
        }
    }
}