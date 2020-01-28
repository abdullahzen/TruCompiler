using System;
using System.Collections.Generic;
using static TruCompiler.Lexical_Analyzer.Tokens;
using static TruCompiler.Lexical_Analyzer.CommentsAnalyzer;

namespace TruCompiler.Lexical_Analyzer
{
    public class LexicalAnalyzer
    {
        public static IList<Token?> Tokenize(string[] lines)
        {
            IList<Token?> tokens = new List<Token?>();
            if (lines != null && lines.Length != 0)
            {
                //Loop through all the lines and find the tokens.
                for (int k = 0; k < lines.Length; k++)
                {
                    int i = k + 1;
                    if (!String.IsNullOrEmpty(lines[k]))
                    {
                        ((List<Token?>)tokens).AddRange(Tokenize(lines[k], i));
                    }
                }
            }
            return tokens;
        }
        public static IList<Token?> Tokenize(string line, int i = 1)
        {
            IList<Token?> tokens = new List<Token?>();
            if (!String.IsNullOrEmpty(line))
            {
                        
                string[] values = line.Split();
                bool inlinecmt = false;
                string commentContent = "";
                int charCount = 0;
                string value;
                for (int j = 0; j < values.Length; j++)
                {
                    if (!String.IsNullOrEmpty(values[j]))
                    {
                        value = CheckInlineComments(values[j], ref tokens, i, ref inlinecmt);

                        if (!inlinecmt)
                        {
                            //regular tokens
                            tokens.Add(CreateToken(value, i));
                            charCount += value.Length;
                            charCount++;
                        } else
                        {
                            //This is to add back any \t or whitespaces between the words in the comments from the original line
                            if (line[charCount] == ' ' || line[charCount] == '\t')
                            {
                                commentContent += line[charCount].ToString() + value;
                                charCount++;
                            }
                            else
                            {
                                commentContent += value;
                            }
                            charCount += values[j].Length;
                        }
                    }
                    else
                    {
                        if (inlinecmt)
                        {
                            commentContent += line[charCount].ToString();
                            charCount++;
                        }
                    }
                }

                if (inlinecmt && commentContent.Length > 2)
                {
                    AddInlineCommentContent(commentContent, ref tokens, i);
                }
            }
            return tokens;
        }
    }
}
