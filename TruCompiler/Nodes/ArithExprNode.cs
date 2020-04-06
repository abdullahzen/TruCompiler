using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class ArithExprNode : Node<Token>
    {
        public ArithExprNode(Node<Token> parent, Node<Token> current) : base(parent, current)
        {
            if (current.Children.Count <= 2)
            {
                GetFactor(current, this);
            } else
            {
                var left = new List<Node<Token>>();
                var right = new List<Node<Token>>();
                var foundOp = false;
                var multOrAdd = -1;
                Node<Token> op = null;
                foreach(Node<Token> c in current.Children)
                {
                    if ((c.Value.Lexeme == Lexeme.mult || c.Value.Lexeme == Lexeme.div || (c.Value.Lexeme == Lexeme.keyword && c.Value.Value == "and")) && !foundOp)
                    {
                        multOrAdd = 0;
                        foundOp = true;
                        op = c;
                        continue;
                    } else 
                    if ((c.Value.Lexeme == Lexeme.plus || c.Value.Lexeme == Lexeme.minus || (c.Value.Lexeme == Lexeme.keyword && c.Value.Value == "or")) && !foundOp)
                    {
                        multOrAdd = 1;
                        foundOp = true;
                        op = c;
                        continue;
                    } 
                    if (!foundOp)
                    {
                        left.Add(c);
                    } else
                    {
                        right.Add(c);
                    }
                }
                if (foundOp && multOrAdd == 0)
                {
                    this.AddChild(new MultOpNode(this, left, right, op), false);
                } else if (foundOp && multOrAdd == 1)
                {
                    this.AddChild(new AddOpNode(this, left, right, op), false);
                }
            }
        }

        public bool IsValid()
        {
            return Value.IsValid;
        }

        public void accept(Visitor<Token> visitor)
        {
            visitor.visit(this);
        }

        public static void GetFactor(Node<Token> current, Node<Token> thisNode)
        {
            if (current.Children.Count > 0)
            {
                switch (current[0].Value.Lexeme)
                {
                    case Lexeme.keyword:
                        if (current[0].Value.Value == "not")
                        {
                            thisNode.AddChild(new NegateFactorNode(thisNode, current[1]), false);
                        }
                        else if (current[0].Value.Value == "ArithExpr")
                        {
                            thisNode.AddChild(new ArithExprNode(thisNode, current[0]), false);
                        }
                        else if (current[0].Value.Value == "Sign")
                        {
                            switch (current[0][0].Value.Lexeme)
                            {
                                case Lexeme.minus:
                                case Lexeme.plus:
                                    thisNode.AddChild(new SignNode(thisNode, current), false);
                                    break;
                            }
                        }
                        break;
                    case Lexeme.id:
                        if (current.Children.Count > 1)
                        {
                            if (current[1].Value.Lexeme == Lexeme.dot)
                            {
                                thisNode.AddChild(FunctionCallOrVariableHelper.GetNode(thisNode, current[0], current[1]), false);
                            }
                            else if (current[1].Value.Value == "AParams")
                            {
                                thisNode.AddChild(new FunctionCallNode(thisNode, current[0], current[1]), false);
                            }
                            else if (current[1].Value.Value == "ArraySizeValue")
                            {
                                thisNode.AddChild(new VariableNode(thisNode, current[0], current[1]), false);
                            }
                        } else if (current.Children.Count == 1 && current[0].Children.Count == 0)
                        {
                            thisNode.AddChild(new VariableNode(thisNode, current[0]), false);
                        } else if (current.Children.Count == 1)
                        {
                            thisNode.AddChild(FunctionCallOrVariableHelper.GetNode(thisNode, current[0]), false);
                        }
                        break;

                    case Lexeme.floatnum:
                    case Lexeme.intnum:
                        thisNode.AddChild(new NumNode(thisNode, current[0]), false);
                        break;
                    case Lexeme.minus:
                    case Lexeme.plus:
                        thisNode.AddChild(new SignNode(thisNode, current[0], current[1]), false);
                        break;
                }
            }
        }
    
    }

}