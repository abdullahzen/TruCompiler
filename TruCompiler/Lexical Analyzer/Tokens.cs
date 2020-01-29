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
            keyword, // if do read then end write else public return while private main class or inherits integer and local float not
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
        }

        public static Token? CreateToken(string value, int location)
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
                    return new Token()
                    {
                        Lexeme = Lexeme.keyword,
                        Value = value,
                        Location = location,
                        IsValid = true
                    };
                case "==":
                    return new Token()
                    {
                        Lexeme = Lexeme.eqeq,
                        Value = value,
                        Location = location,
                        IsValid = true
                    };
                case "<>":
                    return new Token()
                    {
                        Lexeme = Lexeme.noteq,
                        Value = value,
                        Location = location,
                        IsValid = true
                    };
                case "<":
                    return new Token()
                    {
                        Lexeme = Lexeme.lt,
                        Value = value,
                        Location = location,
                        IsValid = true
                    };
                case ">":
                    return new Token()
                    {
                        Lexeme = Lexeme.gt,
                        Value = value,
                        Location = location,
                        IsValid = true
                    };
                case "<=":
                    return new Token()
                    {
                        Lexeme = Lexeme.leq,
                        Value = value,
                        Location = location,
                        IsValid = true
                    };
                case ">=":
                    return new Token()
                    {
                        Lexeme = Lexeme.geq,
                        Value = value,
                        Location = location,
                        IsValid = true
                    };
                case "+":
                    return new Token()
                    {
                        Lexeme = Lexeme.plus,
                        Value = value,
                        Location = location,
                        IsValid = true
                    };
                case "-":
                    return new Token()
                    {
                        Lexeme = Lexeme.minus,
                        Value = value,
                        Location = location,
                        IsValid = true
                    };
                case "*":
                    return new Token()
                    {
                        Lexeme = Lexeme.mult,
                        Value = value,
                        Location = location,
                        IsValid = true
                    };
                case "/":
                    return new Token()
                    {
                        Lexeme = Lexeme.div,
                        Value = value,
                        Location = location,
                        IsValid = true
                    };
                case "=":
                    return new Token()
                    {
                        Lexeme = Lexeme.eq,
                        Value = value,
                        Location = location,
                        IsValid = true
                    };
                case "(":
                    return new Token()
                    {
                        Lexeme = Lexeme.openpar,
                        Value = value,
                        Location = location,
                        IsValid = true
                    };
                case ")":
                    return new Token()
                    {
                        Lexeme = Lexeme.closepar,
                        Value = value,
                        Location = location,
                        IsValid = true
                    };
                case "{":
                    return new Token()
                    {
                        Lexeme = Lexeme.opencbr,
                        Value = value,
                        Location = location,
                        IsValid = true
                    };
                case "}":
                    return new Token()
                    {
                        Lexeme = Lexeme.closecbr,
                        Value = value,
                        Location = location,
                        IsValid = true
                    };
                case "[":
                    return new Token()
                    {
                        Lexeme = Lexeme.opensqbr,
                        Value = value,
                        Location = location,
                        IsValid = true
                    };
                case "]":
                    return new Token()
                    {
                        Lexeme = Lexeme.closesqbr,
                        Value = value,
                        Location = location,
                        IsValid = true
                    };
                case ";":
                    return new Token()
                    {
                        Lexeme = Lexeme.semi,
                        Value = value,
                        Location = location,
                        IsValid = true
                    };
                case ",":
                    return new Token()
                    {
                        Lexeme = Lexeme.comma,
                        Value = value,
                        Location = location,
                        IsValid = true
                    };
                case ".":
                    return new Token()
                    {
                        Lexeme = Lexeme.dot,
                        Value = value,
                        Location = location,
                        IsValid = true
                    };
                case ":":
                    return new Token()
                    {
                        Lexeme = Lexeme.colon,
                        Value = value,
                        Location = location,
                        IsValid = true
                    };
                case "::":
                    return new Token()
                    {
                        Lexeme = Lexeme.coloncolon,
                        Value = value,
                        Location = location,
                        IsValid = true
                    };
                case "//":
                    return new Token()
                    {
                        Lexeme = Lexeme.inlinecmt,
                        Value = value,
                        Location = location,
                        IsValid = true
                    };
                case "/*":
                    return new Token()
                    {
                        Lexeme = Lexeme.opencmt,
                        Value = value,
                        Location = location,
                        IsValid = true
                    };
                case "*/":
                    return new Token()
                    {
                        Lexeme = Lexeme.closecmt,
                        Value = value,
                        Location = location,
                        IsValid = true
                    };
                default:
                    DynamicLexValidator dynamicLexValidator = new DynamicLexValidator();
                    if (Regex.IsMatch(value, "^([0-9]*[^.])$"))
                    {
                        return new Token()
                        {
                            Lexeme = Lexeme.intnum,
                            Value = value,
                            Location = location,
                            IsValid = dynamicLexValidator.Validate(value, "Integer")
                        };
                    } else if (Regex.IsMatch(value, "^([0-9]*(\\.)[0-9]*(e)*[0-9]*[-|+]*[0-9]*)*$"))
                    {
                        return new Token()
                        {
                            Lexeme = Lexeme.floatnum,
                            Value = value,
                            Location = location,
                            IsValid = dynamicLexValidator.Validate(value, "Float")
                        };
                    } else
                    {
                        return new Token()
                        {
                            Lexeme = Lexeme.id,
                            Value = value,
                            Location = location,
                            IsValid = dynamicLexValidator.Validate(value, "Identifier")
                        };
                    }
            }
        }

        public struct Token
        {
            public Lexeme Lexeme { get; set; }
            public string Value { get; set; }
            public int Location { get; set; }
            public bool IsValid { get; set; }
        }

        public static string ToString(IEnumerable<Token?> tokens)
        {
            string result = "";
            int lastLineNum = 1;    
            foreach (Token token in tokens)
            {
                if (lastLineNum != token.Location)
                {
                    result += "\n";
                } else
                {
                    result += " ";
                }
                if (token.Lexeme == Lexeme.keyword && token.IsValid)
                {
                    result += String.Format("[{0}, {1}, {2}]", token.Value, token.Value, token.Location);
                } else if (token.IsValid)
                {
                    result += String.Format("[{0}, {1}, {2}]", token.Lexeme, token.Value, token.Location);
                } else
                {
                    result += String.Format("[invalid{0}, {1}, {2}]", token.Lexeme, token.Value, token.Location);
                }
                lastLineNum = token.Location;
            }
            result = result.TrimStart();
            return result;
        }
    }
}
