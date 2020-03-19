using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class ReadStatementNode : StatementNode
    {
        public VariableNode Variable { get; set; }
        public ReadStatementNode(Node<Token> parent, Node<Token> current) : base(parent, current, "ReadStatmenet")
        {
            if (current.Children.Count > 1 && current[0].Value.Value == "read" && current[1].Value.Value == "Variable")
            {
                Variable = (VariableNode)this.AddChild(new VariableNode(this, current[1]), true);
            }
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