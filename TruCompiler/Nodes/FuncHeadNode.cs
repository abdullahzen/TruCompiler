using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class FuncHeadNode : Node<Token>
    {
        public IdNode ClassName { get; set; }
        public IdNode FunctionName { get; set; }
        public FParamsNode FParams { get; set; }
        public TypeNode ReturnType { get; set; }
        public VoidNode Void { get; set; }
        public FuncHeadNode(Node<Token> parent, Node<Token> current) : base(parent, current)
        {
            if (current.Children.Count == 3)
            {
                if (current[0].Value.Lexeme == Lexeme.id && current[1].Value.Lexeme == Lexeme.id)
                {
                    ClassName = new IdNode(this, current[0]);
                    this.AddChild(ClassName, false);
                    FunctionName = new IdNode(this, current[1]);
                    this.AddChild(FunctionName, false);
                    FParams = null;
                } else
                {
                    ClassName = null;
                    FunctionName = new IdNode(this, current[0]);
                    this.AddChild(FunctionName, false);
                    FParams = new FParamsNode(this, current[1]);
                    this.AddChild(FParams, false);
                }

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
            } else if (current.Children.Count == 4)
            {
                ClassName = new IdNode(this, current[0]);
                this.AddChild(ClassName, false);
                FunctionName = new IdNode(this, current[1]);
                this.AddChild(FunctionName, false);
                FParams = new FParamsNode(this, current[2]);
                this.AddChild(FParams, false);
                if (current[3].Value.Value == "void")
                {
                    Void = new VoidNode(this, current[3]);
                    ReturnType = null;
                    this.AddChild(Void, false);
                }
                else
                {
                    ReturnType = new TypeNode(this, current[3]);
                    Void = null;
                    this.AddChild(ReturnType, false);
                }
            } else if (current.Children.Count == 2)
            {
                ClassName = null;
                FunctionName = new IdNode(this, current[0]);
                this.AddChild(FunctionName, false);
                FParams = null;
                if (current[1].Value.Value == "void")
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
            if (current.Children.Count > 0)
            {
                this.Parent.Value.Line = current[0].Value.Line;
            }
        }

        public bool IsValid()
        {
            return Value.Equals(new Token(Lexeme.keyword, "FuncHead")) && Value.IsValid;
        }

        public override void accept(Visitor<Token> visitor)
        {
            visitor.visit(this);
        }
    }
}