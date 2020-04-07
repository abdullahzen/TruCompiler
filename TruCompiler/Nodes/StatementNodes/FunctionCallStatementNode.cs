using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class FunctionCallStatementNode : StatementNode
    {
        public FunctionCallNode Function { get; set; }
        public FunctionCallStatementNode(Node<Token> parent, Node<Token> current) : base(parent, current, "FunctionCallStatement")
        {
            Function = (FunctionCallNode)this.AddChild(new FunctionCallNode(this, current[0]),true);
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