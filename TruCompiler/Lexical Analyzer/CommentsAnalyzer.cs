using System;
using System.Collections.Generic;
using System.Text;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Lexical_Analyzer
{
    public class CommentsAnalyzer
    {
        public static string CheckInlineComments(string value, ref IList<Token?> tokens, int i, ref bool inlinecmt)
        {
            string[] splittedComment;
            if (!inlinecmt)
            {
                if (value.StartsWith("//"))
                {
                    tokens.Add(CreateToken("//", i));
                    inlinecmt = true;
                }
                else if (value.Contains("//"))
                {
                    splittedComment = value.Split("//");
                    tokens.Add(CreateToken(splittedComment[0], i));
                    tokens.Add(CreateToken("//", i));
                    value = String.Join("//", splittedComment);
                    value = value.Substring(value.IndexOf("//"), value.Length - splittedComment[0].Length);
                    inlinecmt = true;
                }
            }
            return value;
        }

        public static void AddInlineCommentContent(string comment, ref IList<Token?> tokens, int i)
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
                tokens.Add(CreateToken(line.Substring(0, 2), i));
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
                    // ^1 notation is for the length - 1 index
                    // range operator 0..^3 is from 0 to length - 3
                    Value = splitted[^1][0..^3],
                    Location = i,
                    IsValid = true
                });
                tokens.Add(CreateToken(line.Substring(line.Length - 2, 2), i));
                line = line.Substring(line.LastIndexOf("/*") - 1, (line.Length - line.LastIndexOf("/*") + 1));
                return CheckBlockComments(line, ref tokens, i);
            }
            else if (line.Contains("/*") && line.Contains("*/"))
            {

            }
            else if (line.Contains("/*"))
            {
                //check starts with 
                //and else

            }
            else if (line.Contains("*/"))
            {
                //check starts with and else
            }
            else
            {
                return line;
            }
            return line;
        }
    }
}
