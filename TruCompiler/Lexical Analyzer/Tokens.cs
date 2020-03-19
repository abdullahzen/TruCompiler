using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TruCompiler.Lexical_Analyzer
{
    public class Tokens
    {
        public enum Lexeme
        {
            eq, //=
            eqeq,//==
            plus, //+
            openpar, //(
            keyword, // if do read then end write else public return while private main class or inherits integer and local float not void
            noteq, //<>
            minus, //-
            closepar, //)
            lt, //<
            gt, //>
            mult, //*
            div, // /
            opencbr, //{
            closecbr, //}
            semi, //;
            dot, //.
            comma, //,
            colon, //:
            coloncolon, //::
            leq, //<=
            geq, //>=
            id, //identifier
            opensqbr, //[
            closesqbr, //]
            opencmt, ///*
            closecmt, //*/
            inlinecmt, // //
            intnum, // 123
            floatnum, //2.2
            blockcmt, // block of comment
            sr, // ?? serial? static?
        }

        public static Token CreateToken(string value, int location, ref IList<Token> tokens)
        {
            switch (value)
            {
                case "if":
                case "then":
                case "else":
                case "while":
                case "class":
                case "integer":
                case "float":
                case "do":
                case "end":
                case "public":
                case "private":
                case "or":
                case "and":
                case "not":
                case "read":
                case "write":
                case "return":
                case "main":
                case "inherits":
                case "local":
                case "void":
                    return new Token()
                    {
                        Lexeme = Lexeme.keyword,
                        Value = value,
                        Line = location,
                        IsValid = true
                    };
                case "==":
                    return new Token()
                    {
                        Lexeme = Lexeme.eqeq,
                        Value = value,
                        Line = location,
                        IsValid = true
                    };
                case "<>":
                    return new Token()
                    {
                        Lexeme = Lexeme.noteq,
                        Value = value,
                        Line = location,
                        IsValid = true
                    };
                case "<":
                    return new Token()
                    {
                        Lexeme = Lexeme.lt,
                        Value = value,
                        Line = location,
                        IsValid = true
                    };
                case ">":
                    return new Token()
                    {
                        Lexeme = Lexeme.gt,
                        Value = value,
                        Line = location,
                        IsValid = true
                    };
                case "<=":
                    return new Token()
                    {
                        Lexeme = Lexeme.leq,
                        Value = value,
                        Line = location,
                        IsValid = true
                    };
                case ">=":
                    return new Token()
                    {
                        Lexeme = Lexeme.geq,
                        Value = value,
                        Line = location,
                        IsValid = true
                    };
                case "+":
                    return new Token()
                    {
                        Lexeme = Lexeme.plus,
                        Value = value,
                        Line = location,
                        IsValid = true
                    };
                case "-":
                    return new Token()
                    {
                        Lexeme = Lexeme.minus,
                        Value = value,
                        Line = location,
                        IsValid = true
                    };
                case "*":
                    return new Token()
                    {
                        Lexeme = Lexeme.mult,
                        Value = value,
                        Line = location,
                        IsValid = true
                    };
                case "/":
                    return new Token()
                    {
                        Lexeme = Lexeme.div,
                        Value = value,
                        Line = location,
                        IsValid = true
                    };
                case "=":
                    return new Token()
                    {
                        Lexeme = Lexeme.eq,
                        Value = value,
                        Line = location,
                        IsValid = true
                    };
                case "(":
                    return new Token()
                    {
                        Lexeme = Lexeme.openpar,
                        Value = value,
                        Line = location,
                        IsValid = true
                    };
                case ")":
                    return new Token()
                    {
                        Lexeme = Lexeme.closepar,
                        Value = value,
                        Line = location,
                        IsValid = true
                    };
                case "{":
                    return new Token()
                    {
                        Lexeme = Lexeme.opencbr,
                        Value = value,
                        Line = location,
                        IsValid = true
                    };
                case "}":
                    return new Token()
                    {
                        Lexeme = Lexeme.closecbr,
                        Value = value,
                        Line = location,
                        IsValid = true
                    };
                case "[":
                    return new Token()
                    {
                        Lexeme = Lexeme.opensqbr,
                        Value = value,
                        Line = location,
                        IsValid = true
                    };
                case "]":
                    return new Token()
                    {
                        Lexeme = Lexeme.closesqbr,
                        Value = value,
                        Line = location,
                        IsValid = true
                    };
                case ";":
                    return new Token()
                    {
                        Lexeme = Lexeme.semi,
                        Value = value,
                        Line = location,
                        IsValid = true
                    };
                case ",":
                    return new Token()
                    {
                        Lexeme = Lexeme.comma,
                        Value = value,
                        Line = location,
                        IsValid = true
                    };
                case ".":
                    return new Token()
                    {
                        Lexeme = Lexeme.dot,
                        Value = value,
                        Line = location,
                        IsValid = true
                    };
                case ":":
                    return new Token()
                    {
                        Lexeme = Lexeme.colon,
                        Value = value,
                        Line = location,
                        IsValid = true
                    };
                case "::":
                    return new Token()
                    {
                        Lexeme = Lexeme.coloncolon,
                        Value = value,
                        Line = location,
                        IsValid = true
                    };
                case "//":
                    return new Token()
                    {
                        Lexeme = Lexeme.inlinecmt,
                        Value = value,
                        Line = location,
                        IsValid = true
                    };
                case "/*":
                    return new Token()
                    {
                        Lexeme = Lexeme.opencmt,
                        Value = value,
                        Line = location,
                        IsValid = true
                    };
                case "*/":
                    return new Token()
                    {
                        Lexeme = Lexeme.closecmt,
                        Value = value,
                        Line = location,
                        IsValid = true
                    };
                default:
                    DynamicLexValidator dynamicLexValidator = new DynamicLexValidator();
                    if (Regex.IsMatch(value, "^([0-9]*)$"))
                    {
                        return new Token()
                        {
                            Lexeme = Lexeme.intnum,
                            Value = value,
                            Line = location,
                            IsValid = dynamicLexValidator.Validate(value, "Integer")
                        };
                    } else if (Regex.IsMatch(value, "^([0-9]*(\\.)[0-9]*(e)*[0-9]*[-|+]*[0-9]*)*$"))
                    {
                        return new Token()
                        {
                            Lexeme = Lexeme.floatnum,
                            Value = value,
                            Line = location,
                            IsValid = dynamicLexValidator.Validate(value, "Float")
                        };
                    } else if (dynamicLexValidator.Validate(value, "Identifier"))
                    {
                        return new Token()
                        {
                            Lexeme = Lexeme.id,
                            Value = value,
                            Line = location,
                            IsValid = dynamicLexValidator.Validate(value, "Identifier")
                        };
                    } else
                    {
                        List<string> splitSingleWord = SplitSingleWord(value);
                        if (splitSingleWord != null && splitSingleWord.Count > 0)
                        {
                            foreach (string part in splitSingleWord)
                            {
                                if (!String.IsNullOrEmpty(part))
                                {
                                    tokens.Add(CreateToken(part, location, ref tokens));
                                }
                            }
                            //done with every token
                            return null;
                        } 
                        else
                        {
                            return new Token()
                            {
                                Lexeme = Lexeme.id,
                                Value = value,
                                Line = location,
                                IsValid = dynamicLexValidator.Validate(value, "Identifier")
                            };
                        }
                    } 
            }
        }

        private static List<string> SplitSingleWord(string value)
        {
            List<string> splitString = new List<string>();
            String[] reservedkeywords = {"if", "then", "else", "while", "class", "integer", "float", "do",
            "end", "public", "private", "or", "and", "not", "read", "write", "return", "main", "inherits",
            "local", "void", "==", "<>", "<", ">", "<=", ">=", "+", "-", "*", "/", "=", "(", ")", "{", "}", "[", "]",
            ";", ",", "::", ":", "."};

            foreach (string reserved in reservedkeywords)
            {
                if (value.Contains(reserved, StringComparison.InvariantCulture))
                {
                    splitString.Add(value.Substring(0, value.IndexOf(reserved)));
                    splitString.Add(value.Substring(value.IndexOf(reserved), reserved.Length));
                    splitString.Add(value.Substring(value.IndexOf(reserved) + reserved.Length));
                    break;
                }
            }
            return splitString;
        }

        public class Token
        {
            public Lexeme Lexeme { get; set; }
            public string Value { get; set; }
            public int Line { get; set; }
            public bool IsValid { get; set; }

            public Token() { }

            public Token(Lexeme lexeme, string value)
            {
                Lexeme = lexeme;
                Value = value;
                IsValid = true;
            }
            public Token(Lexeme lexeme)
            {
                Lexeme = lexeme;
                IsValid = true;
            }
            public override bool Equals(object obj)
            {
                if (obj != null)
                {
                    Token second = (Token)obj;
                    if (this.Lexeme == Lexeme.keyword && second.Lexeme == Lexeme.keyword)
                    {
                        if (this.Value == second.Value)
                        {
                            return true;
                        }
                    } else if (this.Lexeme == second.Lexeme)
                    {
                        return true;
                    }
                }
                return false;
            }

            public override string ToString()
            {
                if (this.Lexeme == Lexeme.keyword && this.IsValid)
                {
                    return String.Format("[{0}, {1}, {2}]", this.Value, this.Value, this.Line);
                }
                else if (this.IsValid)
                {
                    return String.Format("[{0}, {1}, {2}]", this.Lexeme, this.Value, this.Line);
                }
                else
                {
                    return String.Format("[invalid{0}, {1}, {2}]", this.Lexeme, this.Value, this.Line);
                }
            }

            public Token Clone()
            {
                Token copy = new Token()
                {
                    Lexeme = this.Lexeme,
                    IsValid = this.IsValid,
                    Line = this.Line,
                    Value = this.Value
                };
                return copy;
            }
        }

        public static string ToString(IEnumerable<Token> tokens)
        {
            string result = "";
            int lastLineNum = 1;    
            foreach (Token token in tokens)
            {
                if (lastLineNum != token.Line)
                {
                    result += "\n";
                } else
                {
                    result += " ";
                }
                if (token.Lexeme == Lexeme.keyword && token.IsValid)
                {
                    result += String.Format("[{0}, {1}, {2}]", token.Value, token.Value, token.Line);
                } else if (token.IsValid)
                {
                    result += String.Format("[{0}, {1}, {2}]", token.Lexeme, token.Value, token.Line);
                } else
                {
                    result += String.Format("[invalid{0}, {1}, {2}]", token.Lexeme, token.Value, token.Line);
                }
                lastLineNum = token.Line;
            }
            result = result.TrimStart();
            return result;
        }
    }
}
