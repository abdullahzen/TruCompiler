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
            bool blockcmtMult = false;
            string blockcmtContent = "";
            if (lines != null && lines.Length != 0)
            {
                //Loop through all the lines and find the tokens.
                for (int k = 0; k < lines.Length; k++)
                {
                    int i = k + 1;
                    if (!String.IsNullOrEmpty(lines[k]))
                    {
                        if (lines[k].Contains("/*") && !lines[k].Contains("*/"))
                        {
                            blockcmtMult = true;
                        }
                        ((List<Token?>)tokens).AddRange(Tokenize(lines[k], ref blockcmtContent, i, blockcmtMult));
                        if (lines[k].Contains("*/"))
                        {
                            blockcmtMult = false;
                        }
                        if (blockcmtMult)
                        {
                            blockcmtContent += "\\n";
                        }
                    }
                }
            }
            return tokens;
        }
        public static IList<Token?> Tokenize(string line, int i = 1, bool blockcmtMult = false)
        {
            string blockcmtContent = "";
            
            return Tokenize(line, ref blockcmtContent, i, blockcmtMult);
        }
        public static IList<Token?> Tokenize(string line, ref string blockcmtContent, int i = 1, bool blockcmtMult = false)
        {
            IList<Token?> tokens = new List<Token?>();
            if (!String.IsNullOrEmpty(line))
            {
                        
                string[] values = line.Split();
                bool inlinecmt = false;
                bool blockcmt = false;
                string commentContent = "";
                int charCount = 0;
                
                string value = null;
                for (int j = 0; j < values.Length; j++)
                {
                    if (!String.IsNullOrEmpty(values[j]))
                    {
                        if (!blockcmtMult && !blockcmt)
                        {
                            value = CheckInlineComments(values[j], ref tokens, i, ref inlinecmt);
                            value = CheckBlockComments(value, ref tokens, i, ref blockcmt);
                        }
                        else
                        {
                            value = CheckBlockComments(values[j], ref tokens, i, ref blockcmt);
                        }


                        if (!inlinecmt && !blockcmtMult && !blockcmt)
                        {
                            //regular tokens
                            tokens.Add(CreateToken(value, i));
                            charCount += value.Length;
                            charCount++;
                        } else if (inlinecmt || blockcmtMult || blockcmt)
                        {
                            //This is to add back any \t or whitespaces between the words in the comments from the original line
                            if (line[charCount] == ' ' || line[charCount] == '\t')
                            {
                                if (blockcmtMult)
                                {
                                    blockcmtContent += line[charCount].ToString() + value;
                                } else
                                {
                                    commentContent += line[charCount].ToString() + value;
                                }
                                charCount++;
                            }
                            else
                            {
                                if (blockcmtMult)
                                {
                                    blockcmtContent += value;
                                }
                                else
                                {
                                    commentContent += value;
                                }
                            }
                            charCount += values[j].Length;
                        } 
                    }
                    else
                    {
                        if (inlinecmt || blockcmt)
                        {
                            commentContent += line[charCount].ToString();
                            charCount++;
                        } else if (blockcmtMult)
                        {
                            blockcmtContent += line[charCount].ToString();
                            charCount++;
                        }
                    }
                    CheckBlockCommentsEnd(values[j], ref tokens, i, ref blockcmt, blockcmtMult);
                }
                

                if (inlinecmt && commentContent.Length > 2)
                {
                    AddInlineCommentContent(commentContent, ref tokens, i);
                } else if (!blockcmt && commentContent.Length > 4 && !blockcmtMult)
                {
                    AddBlockCommentContent(commentContent, ref tokens, i);
                }
                else if (blockcmtMult && !blockcmt && line.Contains("*/"))
                {
                    int count = i - blockcmtContent.Split("\\n").Length + 1;
                    AddBlockCommentContent(blockcmtContent, ref tokens, count);
                }

            }
            return tokens;
        }
    }
}
