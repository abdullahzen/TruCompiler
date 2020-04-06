using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public static class FunctionCallOrVariableHelper
    {
        public static Node<Token> GetNode(Node<Token> parent, Node<Token> current, Node<Token> other)
        {
            if (other.Value.Lexeme == Lexeme.dot)
            {
                var temp = other;
                while (temp.Children.Count != 0)
                {
                    if (temp.Children.Count > 1)
                    {
                        if (temp[1].Value.Lexeme == Lexeme.dot)
                        {
                            temp = temp[1];
                            continue;
                        }
                        else if (temp[1].Value.Value == "AParams")
                        {
                            return new FunctionCallNode(parent, current, other);
                            break;
                        }
                        else if (temp[1].Value.Value == "ArraySizeValue")
                        {
                            return new VariableNode(parent, current, other);
                        }
                    }else
                    {
                        return new VariableNode(parent, current, other);
                    }
                }
                return new VariableNode(parent, current, other);
            }
            else if (other.Value.Value == "AParams")
            {
                return new FunctionCallNode(parent, current, other);
            }
            else if (other.Value.Value == "ArraySizeValue")
            {
                return new VariableNode(parent, current, other);
            }
            return new VariableNode(parent, current, other);
        }

        public static Node<Token> GetNode(Node<Token> parent, Node<Token> current)
        {
            if (current.Value.Lexeme == Lexeme.id)
            {
                if (current.Children.Count == 1 && current[0].Value.Lexeme == Lexeme.dot)
                {
                    var temp = current[0];
                    while (temp.Children.Count != 0)
                    {
                        if (temp.Children.Count > 1)
                        {
                            if (temp[1].Value.Lexeme == Lexeme.dot)
                            {
                                temp = temp[1];
                                continue;
                            }
                            else if (temp[1].Value.Value == "AParams")
                            {
                                return new FunctionCallNode(parent, current);
                                break;
                            }
                            else if (temp[1].Value.Value == "ArraySizeValue")
                            {
                                return new VariableNode(parent, current);
                            }
                        }
                    }
                    return new VariableNode(parent, current);
                }
            }
            return new VariableNode(parent, current);
        }
    }
}