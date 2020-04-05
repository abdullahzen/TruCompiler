using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class FuncDeclNode : Node<Token>
    {
        public IdNode Name { get; set; }
        public FParamsNode FParams{ get; set; }
        public TypeNode ReturnType { get; set; }
        public VoidNode Void { get; set; }

        public FuncDeclNode(Node<Token> parent, Node<Token> current) : base(parent, current)
        {
            Name = new IdNode(this, current[0]);
            AddChild(Name, false);
            if (current.Children.Count > 2)
            {
                FParams = new FParamsNode(this, current[1]);
                AddChild(FParams, false);
                if (current[2].Value.Value == "void")
                {
                    Void = new VoidNode(this, current[2]);
                    ReturnType = null;
                    this.AddChild(Void, false);
                }
                else
                {
                    ReturnType = new TypeNode(this, current[2]);
                    Void = null;
                    this.AddChild(ReturnType, false);
                }
            } else
            {
                FParams = null;
                if (current[2].Value.Value == "void")
                {
                    Void = new VoidNode(this, current[1]);
                    ReturnType = null;
                    this.AddChild(Void, false);
                }
                else
                {
                    ReturnType = new TypeNode(this, current[1]);
                    Void = null;
                    this.AddChild(ReturnType, false);
                }
            }

        }

        public bool IsValid()
        {
            return Value.Equals(new Token(Lexeme.keyword, "Function")) && Value.IsValid;
        }

        public void accept(Visitor<Token> visitor)
        {
            visitor.visit(this);
        }


    }
}