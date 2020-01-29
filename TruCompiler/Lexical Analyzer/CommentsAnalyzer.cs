using System;
using System.Collections.Generic;
using System.Text;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Lexical_Analyzer
{
    public class CommentsAnalyzer
    {
        public static void CheckInlineComments(string value, ref IList<Token?> tokens, int i)
        {
            string[] splittedComment;
           
            if (value.StartsWith("//"))
            {
                tokens.Add(CreateToken("//", i));
            }
            else if (value.Contains("//"))
            {
                splittedComment = value.Split("//");
                ((List<Token?>)tokens).AddRange(LexicalAnalyzer.Tokenize(splittedComment[0], i));
                tokens.Add(CreateToken("//", i));
                value = String.Join("//", splittedComment);
                value = value.Substring(value.IndexOf("//"), value.Length - splittedComment[0].Length);
            }
            if (!String.IsNullOrEmpty(value) && value.Length > 2)
            {
                AddInlineCommentContent(value, ref tokens, i);
            }
        }

        public static void CheckLineBlockComments(string value, ref IList<Token?> tokens, int i)
        {
            string[] splittedComment;
            if (value.StartsWith("/*") && value.EndsWith("*/"))
            {
                tokens.Add(CreateToken("/*", i));
                AddBlockCommentContent(value, ref tokens, i);
                tokens.Add(CreateToken("*/", i));
            }
            else if (value.StartsWith("/*") && value.Contains("*/"))
            {
                tokens.Add(CreateToken("/*", i));
                splittedComment = value.Split("*/");
                value = String.Join("*/", splittedComment);
                value = value.Substring(0, value.IndexOf("*/") + 2);
                AddBlockCommentContent(value, ref tokens, i);
                tokens.Add(CreateToken("*/", i));
                ((List<Token?>)tokens).AddRange(LexicalAnalyzer.Tokenize(splittedComment[1], i));
            } else if (value.Contains("/*") && value.EndsWith("*/"))
            {
                splittedComment = value.Split("/*");
                ((List<Token?>)tokens).AddRange(LexicalAnalyzer.Tokenize(splittedComment[0], i));
                tokens.Add(CreateToken("/*", i));
                value = String.Join("/*", splittedComment);
                value = value.Substring(value.IndexOf("/*"), value.Length - splittedComment[0].Length);
                AddBlockCommentContent(value, ref tokens, i);
                tokens.Add(CreateToken("*/", i));
            }
            else if (value.Contains("/*") && value.Contains("*/"))
            {
                splittedComment = value.Split("/*");
                ((List<Token?>)tokens).AddRange(LexicalAnalyzer.Tokenize(splittedComment[0], i));
                tokens.Add(CreateToken("/*", i));
                value = String.Join("/*", splittedComment);
                value = value.Substring(value.IndexOf("/*"), value.Length - splittedComment[0].Length);
                splittedComment = value.Split("*/");
                value = String.Join("*/", splittedComment);
                value = value.Substring(0, value.IndexOf("*/") + 2);
                AddBlockCommentContent(value, ref tokens, i);
                tokens.Add(CreateToken("*/", i));
                ((List<Token?>)tokens).AddRange(LexicalAnalyzer.Tokenize(splittedComment[1], i));
            }
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
            if (!String.IsNullOrEmpty(comment) && comment.Length > 4)
            {
                tokens.Add(new Token()
                {
                    Lexeme = Lexeme.blockcmt,
                    Value = comment,
                    Location = i,
                    IsValid = true
                });
            }
        }

        public static string CheckMultiLineBlockComments(string value, ref IList<Token?> tokens, int i)
        {
            string[] splittedComment;
            if (value.StartsWith("/*"))
            {
                tokens.Add(CreateToken("/*", i));
                return value;
            }
            else if (value.Contains("/*"))
            {
                splittedComment = value.Split("/*");
                ((List<Token?>)tokens).AddRange(LexicalAnalyzer.Tokenize(splittedComment[0], i));
                tokens.Add(CreateToken("/*", i));
                value = String.Join("/*", splittedComment);
                value = value.Substring(value.IndexOf("/*"), value.Length - splittedComment[0].Length);
                return value;
            }
            return value;
        }

        public static void CheckMultiLineBlockCommentsEndAndAdd(string value, ref IList<Token?> tokens, int i)
        {
            string[] splittedComment;
            int count = i - value.Split("\\n").Length + 1;
            if (value.EndsWith("*/"))
            {
                AddBlockCommentContent(value, ref tokens, count);
                tokens.Add(CreateToken("*/", i));
                return;
            }
            else if (value.Contains("*/"))
            {
                splittedComment = value.Split("*/");
                value = String.Join("*/", splittedComment);
                value = value.Substring(0, value.IndexOf("*/") + 2);
                AddBlockCommentContent(value, ref tokens, count);
                tokens.Add(CreateToken("*/", i));
                ((List<Token?>)tokens).AddRange(LexicalAnalyzer.Tokenize(splittedComment[1], i));
            }
        }
    }       
}
