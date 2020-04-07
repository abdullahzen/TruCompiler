using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class MemberNode : Node<Token>
    {
        public VisibilityNode Visibility { get; set; }
        public FuncDeclNode Function { get; set; }
        public VariableDeclNode Variable { get; set; }
        
        public MemberNode(Node<Token> parent, Node<Token> current) : base(parent, current)
        {
            Visibility = (VisibilityNode)this.AddChild(new VisibilityNode(this, current[0]), true);
            if (current[1].Value.Value == "Function")
            {
                Function = (FuncDeclNode)this.AddChild(new FuncDeclNode(this, current[1]), true);
                Variable = null;
            } else
            {
                Function = null;
                Variable = (VariableDeclNode)this.AddChild(new VariableDeclNode(this, current[1]), true);
            }
        }

        public bool IsValid()
        {
            return Value.Equals(new Token(Lexeme.keyword, "Member")) && Value.IsValid;
        }

        public override void accept(Visitor<Token> visitor)
        {
            visitor.visit(this);
        }


    }
}