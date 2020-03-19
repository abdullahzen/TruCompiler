using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class StatBlockNode : Node<Token>
    {
        public List<StatementNode> Statements { get; set; }
        public StatBlockNode(Node<Token> parent, Node<Token> current, string blockType) : base(parent, GetBaseNodeWithType(current, blockType))
        {
            Statements = StatementNode.GenerateStatements(0, current, this);
        }

        public bool IsValid()
        {
            return Value.IsValid;
        }

        public void accept(Visitor visitor)
        {
            visitor.visit(this);
        }

        public static Node<Token> GetBaseNodeWithType(Node<Token> current, string blockType)
        {
            if (!String.IsNullOrEmpty(blockType))
            {
                current.Value.Value = blockType;
            }
            return current;
        }
    }
}