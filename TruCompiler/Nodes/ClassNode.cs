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
            Name = new IdNode(this, current[0]);
            this.AddChild(Name, false);
            if (current.Children.Count > 2)
            {
                InheritanceList = new InheritanceListNode(this, current[1]);
                this.AddChild(InheritanceList, false);
                ClassMembers = new ClassMembersNode(this, current[2]);
                this.AddChild(ClassMembers, false);
            } else
            {
                InheritanceList = null;
                ClassMembers = new ClassMembersNode(this, current[1]);
                this.AddChild(ClassMembers, false);
            }
        }

        public bool IsValid()
        {
            return Value.Equals(new Token(Lexeme.keyword, "Class")) && Value.IsValid;
        }

        public override void accept(Visitor<Token> visitor)
        {
            visitor.visit(this);
        }

        public bool HasCircularInheritance()
        {
            return HasCircularInheritance(null);
        }

        public bool HasCircularInheritance(List<ClassNode> visitedClasses)
        {
            bool hasCircular = false;
            if (visitedClasses == null)
            {
                visitedClasses = new List<ClassNode>();
                visitedClasses.Add(this);
            } 
            if (InheritanceList != null && InheritanceList.Children.Count > 0)
            {
                foreach(IdNode idNode in InheritanceList.Children)
                {
                    ClassNode classNode = ((ClassListNode)this.Parent).Classes.Find(c => c.Name.IdValue == idNode.IdValue);
                    if (classNode.Name.IdValue == this.Name.IdValue)
                    {
                        return true;
                    }
                    else if (!visitedClasses.Contains(classNode) && classNode != null)
                    {
                        visitedClasses.Add(classNode);
                        hasCircular = classNode.HasCircularInheritance(visitedClasses);
                    } else if (visitedClasses.Contains(classNode))
                    {
                        return true;
                    }
                }
            } else
            {
                return hasCircular;
            }
            return hasCircular;
        }
    }
}