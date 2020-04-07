using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class FuncDefNode : Node<Token>
    {
        public FuncHeadNode FunctionHead { get; set; }
        public FuncBodyNode FunctionBody { get; set; }

        public FuncDefNode(Node<Token> parent, Node<Token> current) : base(parent, current)
        {
            FunctionHead = new FuncHeadNode(this, current[0]);
            this.AddChild(FunctionHead, false);
            FunctionBody = new FuncBodyNode(this, current[1]);
            this.AddChild(FunctionBody, false);
        }

        public bool IsValid()
        {
            return Value.Equals(new Token(Lexeme.keyword, "Function")) && Value.IsValid;
        }

        public override void accept(Visitor<Token> visitor)
        {
            visitor.visit(this);
        }

        public Node<Token> GetReturnType()
        {
            if (FunctionHead.Void != null)
            {
                return FunctionHead.Void;
            }
            return FunctionHead.ReturnType;
        }

        public bool IsVoid()
        {
            if (FunctionHead.Void != null)
            {
                return true;
            }
            return false;
        }
    }
}