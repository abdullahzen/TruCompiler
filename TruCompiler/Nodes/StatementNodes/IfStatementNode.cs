﻿using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class IfStatementNode : StatementNode
    {
        public RelExprNode RelExpr { get; set; }
        public StatBlockNode ThenStatBlock { get; set; }
        public StatBlockNode ElseStatBlock { get; set; }
        public IfStatementNode(Node<Token> parent, Node<Token> current) : base(parent, current, "IfStatement")
        {
            RelExpr = new RelExprNode(
                new ArithExprNode(this, current[1]),
                current[2],
                new ArithExprNode(this, current[3]));
            this.AddChild(RelExpr, false);
            ThenStatBlock = new StatBlockNode(this, current[5], "ThenStatBlock");
            this.AddChild(ThenStatBlock, false);
            ElseStatBlock = new StatBlockNode(this, current[7], "ElseStatBlock");
            this.AddChild(ElseStatBlock, false);
        }

        public bool IsValid()
        {
            return Value.IsValid;
        }

        public void accept(Visitor<Token> visitor)
        {
            visitor.visit(this);
        }
    }
}