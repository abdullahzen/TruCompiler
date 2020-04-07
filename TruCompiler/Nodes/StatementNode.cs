using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public abstract class StatementNode : Node<Token>
    {
        public StatementNode(Node<Token> parent, Node<Token> current, string statementType) : base(parent, GetBaseNodeWithType(current, statementType))
        {
        }

        public bool IsValid()
        {
            return Value.IsValid;
        }

        public override void accept(Visitor<Token> visitor)
        {
            visitor.visit(this);
        }

        public static Node<Token> GetBaseNodeWithType(Node<Token> current, string statementType)
        {
            if (!String.IsNullOrEmpty(statementType))
            {
                current.Value.Lexeme = Lexeme.keyword;
                current.Value.Value = statementType;
            }
            return current;
        }

        public static List<StatementNode> GenerateStatements(int start, Node<Token> current, Node<Token> thisNode)
        {
            List<StatementNode> Statements = new List<StatementNode>();
            for (int i = start; i < current.Children.Count; i++)
            {
                if (current[i].Children.Count > 0)
                {
                    switch (current[i][0].Value.Value)
                    {
                        case "if":
                            Statements.Add((StatementNode)thisNode.AddChild(new IfStatementNode(thisNode, current[i]), true));
                            break;
                        case "while":
                            Statements.Add((StatementNode)thisNode.AddChild(new WhileStatementNode(thisNode, current[i]), true));
                            break;
                        case "read":
                            Statements.Add((StatementNode)thisNode.AddChild(new ReadStatementNode(thisNode, current[i]), true));
                            break;
                        case "write":
                            Statements.Add((StatementNode)thisNode.AddChild(new WriteStatementNode(thisNode, current[i]), true));
                            break;
                        case "return":
                            Statements.Add((StatementNode)thisNode.AddChild(new ReturnStatementNode(thisNode, current[i]), true));
                            break;
                        default:
                            if (current[i][0].Value.Lexeme == Lexeme.id)
                            {
                                if (current[i][0].Children.Count == 1 && current[i][0][0].Value.Value == "Assign")
                                {
                                    Statements.Add((StatementNode)thisNode.AddChild(new AssignStatementNode(thisNode, current[i]), true));
                                    break;
                                }
                                else if (current[i][0].Children.Count == 1 && current[i][0][0].Value.Value == "AParams")
                                {
                                    Statements.Add((StatementNode)thisNode.AddChild(new FunctionCallStatementNode(thisNode, current[i]), true));
                                    break;
                                }
                                else if (current[i][0].Children.Count == 1 && current[i][0][0].Value.Lexeme == Lexeme.dot)
                                {
                                    Statements.Add((StatementNode)thisNode.AddChild(new FunctionCallStatementNode(thisNode, current[i]), true));
                                    break;
                                }
                                else if (current[i][0].Children.Count == 2 && current[i][0][0].Value.Lexeme == Lexeme.dot)
                                {
                                    Statements.Add((StatementNode)thisNode.AddChild(new AssignStatementNode(thisNode, current[i]), true));
                                    break;
                                }
                                else if (current[i][0].Children.Count == 2 && current[i][0][1].Value.Value == "Assign")
                                {
                                    Statements.Add((StatementNode)thisNode.AddChild(new AssignStatementNode(thisNode, current[i]), true));
                                    break;
                                }
                                else if (current[i][0].Children.Count == 2 && current[i][0][1].Value.Value == "AParams")
                                {
                                    Statements.Add((StatementNode)thisNode.AddChild(new FunctionCallStatementNode(thisNode, current[i]), true));
                                    break;
                                }
                            }
                            break;
                    }
                }
            }
            return Statements;
        }
    }
}