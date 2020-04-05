using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Nodes
{
    public class VariableNode : Node<Token>
    {
        public string Name { get; set; }
        public ArraySizeNode ArraySizeValue { get; set; }
        public VariableNode(Node<Token> parent, Node<Token> current) : base(parent, new Node<Token>(new Token(Lexeme.keyword, "Variable")))
        {
            if (current.Value.Lexeme == Lexeme.id)
            {
                Name = this.AddChild(new IdNode(this, current), true).Value.Value;
            } else
            {
                Name = this.AddChild(new IdNode(this, current[0]), true).Value.Value;
            }
        }
        public VariableNode(Node<Token> parent, Node<Token> current, Node<Token> other) : base(parent, new Node<Token>(new Token(Lexeme.keyword, "Variable")))
        {
            if (other.Value.Lexeme == Lexeme.dot)
            {
                this.AddChild(new IdNode(this, current), true);
                var temp = other;
                var last = "";
                while(temp.Children.Count != 0)
                {
                    last = this.AddChild(new IdNode(this, temp[0]), true).Value.Value;
                    if (temp.Children.Count > 1)
                    {
                        if (temp[1].Value.Lexeme == Lexeme.dot)
                        {
                            temp = temp[1];
                            continue;
                        } else if (temp[1].Value.Value == "ArraySizeValue")
                        {
                            Name = last;
                            ArraySizeValue = (ArraySizeNode)this.AddChild(new ArraySizeNode(this, temp[1]), true);
                            break;
                        }
                    } else
                    {
                        break;
                    }
                }
            }
            else if (other.Value.Value == "ArraySizeValue")
            {
                Name = this.AddChild(new IdNode(this, current), true).Value.Value;
                ArraySizeValue = (ArraySizeNode)this.AddChild(new ArraySizeNode(this, other), true);
            }
        }

        public bool IsValid()
        {
            return Value.Equals(new Token(Lexeme.keyword, "Variable")) && Value.IsValid;
        }

        public void accept(Visitor<Token> visitor)
        {
            visitor.visit(this);
        }
    }
}