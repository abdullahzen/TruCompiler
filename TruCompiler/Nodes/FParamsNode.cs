﻿using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class FParamsNode : Node<Token>
    {
        public List<ParamNode> Params { get; set; }
        
        public FParamsNode(Node<Token> parent, Node<Token> current) : base(parent, current)
        {
            Params = new List<ParamNode>();
            current.Children.ForEach(c =>
            {
                ParamNode p = (ParamNode)this.AddChild(new ParamNode(this, c), true);
                if (p.Children.Count != 0)
                {
                    Params.Add(p);
                }
            });
        }

        public bool IsValid()
        {
            return Value.Equals(new Token(Lexeme.keyword, "FParams")) && Value.IsValid;
        }

        public override void accept(Visitor<Token> visitor)
        {
            visitor.visit(this);
        }
    }
}