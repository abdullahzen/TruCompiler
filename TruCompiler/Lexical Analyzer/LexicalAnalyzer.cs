using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TruCompiler.Lexical_Analyzer
{
    public class LexicalAnalyzer
    {
        public enum Lexeme {
            eq, //==
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
        public static IList<Token?> Tokenize(string line)
        {
            string[] arr = { line };
            return LexicalAnalyzer.Tokenize(arr);
        }
        public static IList<Token?> Tokenize(string[] lines)
        {
            IList<Token?> tokens = new List<Token?>();
            if (lines != null && lines.Length != 0)
            {
                //bool comment = false;
                //string commentcode = "";
                for (int k = 0; k < lines.Length; k++)
                {
                    int i = k + 1;
                    if (!String.IsNullOrEmpty(lines[k]))
                    {
                        /*string line = CheckInlineComments(lines[k], ref tokens, i);
                        line = CheckBlockComments(line, ref tokens, i);*/
                        if (String.IsNullOrEmpty(lines[k]))
                        {
                            continue;
                        }
                        string[] values = lines[k].Split();
                        bool inlinecmt = false;
                        string commentContent = "";
                        int charCount = 0;
                        for (int j = 0; j < values.Length; j++)
                        {
                            if (!String.IsNullOrEmpty(values[j]))
                            {
                                CheckInlineComments(values[j], ref tokens, i, ref inlinecmt);

                                if (!inlinecmt)
                                {
                                    tokens.Add(CreateToken(values[j], i));
                                } else
                                {
                                    if (lines[k][charCount] == 32 || lines[k][charCount] == '\t')
                                    {
                                        commentContent += lines[k][charCount].ToString() + values[j];
                                        charCount++;
                                    }
                                    else
                                    {
                                        commentContent += values[j];
                                    }
                                    charCount += values[j].Length;
                                }
                            }
                            else
                            {
                                if (inlinecmt)
                                {
                                    commentContent += lines[k][charCount].ToString();
                                    charCount++;
                                }
                            }
                        }

                        if (inlinecmt)
                        {
                            AddInlineComment(commentContent, ref tokens, i);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return tokens;
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
                        Lexeme = Lexeme.eq,
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
            }
            return null;
        }
        public static void CheckInlineComments(string value, ref IList<Token?> tokens, int i, ref bool inlinecmt)
        {
            string[] splittedComment;
            if (value.StartsWith("//") && !inlinecmt)
            {
                splittedComment = value.Split("//");
                tokens.Add(CreateToken("//", i));
                inlinecmt = true;
            }
            else if (value.Contains("//") && inlinecmt)
            {
                splittedComment = value.Split("//");
                tokens.Add(CreateToken(splittedComment[0], i));
                tokens.Add(CreateToken("//", i));
                if (splittedComment.Length > 1)
                {
                    tokens.Add(CreateToken(splittedComment[1], i));
                }
                inlinecmt = true;
            }
            /*//inline comment
            if (line.StartsWith("//"))
            {
                tokens.Add(CreateToken(line.Substring(0, 2), i));
                if (line.TrimStart('/').Length > 0)
                {
                    tokens.Add(new Token()
                    {
                        Lexeme = Lexeme.inlinecmt,
                        Value = line,
                        Location = i,
                        IsValid = true
                    });
                }
                return string.Empty;
            } else if (line.Contains("//"))
            {
                tokens.Add(CreateToken(line.Substring(line.IndexOf("//"), 2), i));
                List<string> splitted = line.Split("//").ToList<string>();
                line = splitted[0];
                splitted.RemoveAt(0);
                string comment = String.Join("//", splitted);
                if (comment.Trim('/').Length > 0)
                {
                    tokens.Add(new Token()
                    {
                        Lexeme = Lexeme.inlinecmt,
                        Value = comment,
                        Location = i,
                        IsValid = true
                    });
                }
                return line;
            }
            return line;*/
        }

        public static void AddInlineComment(string comment, ref IList<Token?> tokens, int i)
        {
            tokens.Add(new Token()
            {
                Lexeme = Lexeme.inlinecmt,
                Value = comment,
                Location = i,
                IsValid = true
            });
        }
        public static string CheckBlockComments(string line, ref IList<Token?> tokens, int i)
        {
            
            // block comment
            if (line.StartsWith("/*") && line.EndsWith("*/"))
            {
                tokens.Add(CreateToken(line.Substring(0, 2), i));
                tokens.Add(new Token()
                {
                    Lexeme = Lexeme.blockcmt,
                    Value = line,
                    Location = i,
                    IsValid = true
                });
                tokens.Add(CreateToken(line.Substring(line.Length - 2, 2), i));
                return string.Empty;
            }
            //block cmt start and code after
            else if (line.StartsWith("/*") && line.Contains("*/"))
            {
                tokens.Add(CreateToken(line.Substring(0,2), i));
                string[] splitted = line.Split("*/");
                tokens.Add(new Token()
                {
                    Lexeme = Lexeme.blockcmt,
                    Value = splitted[0].Substring(1, splitted[0].Length),
                    Location = i,
                    IsValid = true
                });
                tokens.Add(CreateToken(line.Substring(line.IndexOf("*/"), 2), i));
                line = line.Substring(line.IndexOf("*/") + 1, (line.Length - line.IndexOf("*/") + 1));
                return CheckBlockComments(line, ref tokens, i);
            }
            // code before block cmt
            else if (line.Contains("/*") && line.EndsWith("*/"))
            {
                tokens.Add(CreateToken(line.Substring(line.LastIndexOf("/*"), 2), i));
                string[] splitted = line.Split("/*");
                tokens.Add(new Token()
                {
                    Lexeme = Lexeme.blockcmt,
                    Value = splitted[splitted.Length - 1].Substring(0, splitted[splitted.Length -1].Length - 3),
                    Location = i,
                    IsValid = true
                });
                tokens.Add(CreateToken(line.Substring(line.Length - 2, 2), i));
                line = line.Substring(line.LastIndexOf("/*") - 1, (line.Length - line.LastIndexOf("/*") + 1));
                return CheckBlockComments(line, ref tokens, i);
            }
            else if (line.Contains("/*") && line.Contains("*/"))
            {

            } else if (line.Contains("/*"))
            {
                //check starts with 
                //and else

            } else if (line.Contains("*/"))
            {
                //check starts with and else
            } else
            {
                return line;
            }
            return line;
        }

        public struct Token
        {
            public Lexeme Lexeme { get; set; }
            public string Value { get; set; }
            public int Location { get; set; }
            public bool IsValid { get; set; }
        }
    }
}
