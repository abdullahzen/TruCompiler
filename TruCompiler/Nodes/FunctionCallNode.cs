using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class FunctionCallNode : Node<Token>
    {
        public IdNode Name { get; set; }
        public AParamsNode AParams { get; set; }
        public FunctionCallNode(Node<Token> parent, Node<Token> current, Node<Token> other) : base(parent, new Node<Token>(new Token(Lexeme.keyword, "FunctionCall")))
        {
            if (other.Value.Lexeme == Lexeme.dot)
            {
                this.AddChild(new IdNode(this, current), true);
                var temp = other;
                IdNode last = null;
                while(temp.Children.Count != 0)
                {
                    last = (IdNode)this.AddChild(new IdNode(this, temp[0]), true);
                    if (temp.Children.Count > 1)
                    {
                        if (temp[1].Value.Lexeme == Lexeme.dot)
                        {
                            temp = temp[1];
                            continue;
                        } else if (temp[1].Value.Value == "AParams")
                        {
                            Name = last;
                            AParams = (AParamsNode)this.AddChild(new AParamsNode(this, temp[1]), true);
                            break;
                        }
                    }

                }
            }
            else if (other.Value.Value == "AParams")
            {
                Name = (IdNode)this.AddChild(new IdNode(this, current), true);
                AParams = (AParamsNode)this.AddChild(new AParamsNode(this, other), true);
            }
        }

        public FunctionCallNode(Node<Token> parent, Node<Token> current) : base (parent, new Node<Token>(new Token(Lexeme.keyword, "FunctionCall")))
        {
            if (current.Value.Lexeme == Lexeme.id)
            {
                Name = (IdNode)this.AddChild(new IdNode(this, current), true);
                if (current.Children.Count == 1 && current[0].Value.Lexeme == Lexeme.dot)
                {
                    var temp = current[0];
                    IdNode last = null;
                    while (temp.Children.Count != 0)
                    {
                        last = (IdNode)this.AddChild(new IdNode(this, temp[0]), true);
                        if (temp.Children.Count > 1)
                        {
                            if (temp[1].Value.Lexeme == Lexeme.dot)
                            {
                                temp = temp[1];
                                continue;
                            }
                            else if (temp[1].Value.Value == "AParams")
                            {
                                Name = last;
                                AParams = (AParamsNode)this.AddChild(new AParamsNode(this, temp[1]), true);
                                break;
                            }
                        }
                    }
                } else if (current.Children.Count == 1 && current[0].Value.Value == "AParams")
                {
                    AParams = (AParamsNode)this.AddChild(new AParamsNode(this, current[0]), true);
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
    }
}