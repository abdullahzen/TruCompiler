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

        public static void AddBlockCommentContent(string comment, ref IList<Token?> tokens, int i)
        {
            tokens.Add(new Token()
            {
                Lexeme = Lexeme.blockcmt,
                Value = comment,
                Location = i,
                IsValid = true
            });
        }

        public static string CheckBlockComments(string value, ref IList<Token?> tokens, int i, ref bool blockcmt)
        {
            string[] splittedComment;
            if (!blockcmt)
            {
                if (value.StartsWith("/*"))
                {
                    tokens.Add(CreateToken("/*", i));
                    blockcmt = true;
                }
                else if (value.Contains("/*"))
                {
                    splittedComment = value.Split("/*");
                    tokens.Add(CreateToken(splittedComment[0], i));
                    tokens.Add(CreateToken("/*", i));
                    value = String.Join("/*", splittedComment);
                    value = value.Substring(value.IndexOf("/*"), value.Length - splittedComment[0].Length);
                    blockcmt = true;
                }
            }
            return value;
        }

        public static string CheckBlockCommentsEnd(string value, ref IList<Token?> tokens, int i, ref bool blockcmt, bool blockcmtMult)
        {
            string[] splittedComment;
            if (blockcmt || blockcmtMult)
            {
                if (value.EndsWith("*/"))
                {
                    tokens.Add(CreateToken("*/", i));
                    blockcmt = false;
                }
                else if (value.Contains("*/"))
                {
                    splittedComment = value.Split("*/");
                    if (splittedComment.Length > 1)
                    {
                        tokens.Add(CreateToken(splittedComment[1], i));
                    }
                    tokens.Add(CreateToken("*/", i));
                    value = String.Join("*/", splittedComment);
                    value = value.Substring(0, value.LastIndexOf("*/") + 2);
                    blockcmt = false;
                }
            }
            return value;
        }



    }
         
}
