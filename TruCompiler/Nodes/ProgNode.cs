using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class ProgNode : Node<Token>
    {
        public ClassListNode ClassList { get; set; }
        public FuncDefsNode FunctionDefinitions { get; set; }
        public MainNode Main { get; set; }
        public FuncBodyNode FuncBody { get; set; }
        public ProgNode(Node<Token> parent, Node<Token> current) : base(parent, current)
        {
            ClassList = null;
            FunctionDefinitions = null;
            Main = null;
            current.Children.ForEach(c =>
            {
                if (c.Value.Value == "Classes")
                {
                    ClassList = (ClassListNode)this.AddChild(new ClassListNode(this, c), true);
                } 
                else if (c.Value.Value == "FunctionDefinitions")
                {
                    FunctionDefinitions = (FuncDefsNode)this.AddChild(new FuncDefsNode(this, c), true);
                } 
                else if (c.Value.Value == "main")
                {
                    Main = (MainNode)this.AddChild(new MainNode(this, c), true);
                }
                else if (c.Value.Value == "FuncBody")
                {
                    FuncBody = (FuncBodyNode)this.AddChild(new FuncBodyNode(this, c), true);
                }
            });
        }

        public bool IsValid()
        {
            return Value.Equals(new Token(Lexeme.keyword, "Program")) && Value.IsValid;
        }

        public void accept(Visitor visitor)
        {
            visitor.visit(this);
        }
    }
}