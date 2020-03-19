using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class ClassMembersNode : Node<Token>
    {
        public List<MemberNode> ClassMembers { get; set; }
       
        public ClassMembersNode(Node<Token> parent, Node<Token> current) : base(parent, current)
        {
            ClassMembers = new List<MemberNode>();
            current.Children.ForEach(c =>
            {
                ClassMembers.Add((MemberNode)this.AddChild(new MemberNode(this, c), true));
            });
        }

        public bool IsValid()
        {
            return Value.Equals(new Token(Lexeme.keyword, "ClassMembers")) && Value.IsValid;
        }

        public void accept(Visitor visitor)
        {
            visitor.visit(this);
        }


    }
}