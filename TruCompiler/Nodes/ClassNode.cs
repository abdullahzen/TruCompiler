using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class ClassNode : Node<Token>
    {
        public IdNode Name { get; set; }
        public ClassMembersNode ClassMembers { get; set; }
        public InheritanceListNode InheritanceList { get; set; }
        
        public ClassNode(Node<Token> parent, Node<Token> current) : base(parent, current)
        {
            Name = new IdNode(this, this[0]);
            this.AddChild(Name, false);
            if (Children.Count > 2)
            {
                InheritanceList = new InheritanceListNode(this, this[1]);
                this.AddChild(InheritanceList, false);
                ClassMembers = new ClassMembersNode(this, this[2]);
                this.AddChild(ClassMembers, false);
            } else
            {
                InheritanceList = null;
                ClassMembers = new ClassMembersNode(this, this[1]);
                this.AddChild(ClassMembers, false);
            }
        }

        public bool IsValid()
        {
            return Value.Equals(new Token(Lexeme.keyword, "Class")) && Value.IsValid;
        }

        public void accept(Visitor<Token> visitor)
        {
            visitor.visit(this);
        }
    }
}