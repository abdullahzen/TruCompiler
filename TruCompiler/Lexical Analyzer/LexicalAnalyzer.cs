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
                        //handle multi line block comments
                        if (lines[k].Contains("/*") && !lines[k].Contains("*/"))
                        {
                            bool commented = true;
                            string blockcomment = "";
                            do
                            {
                                i = k + 1;
                                if (lines[k].Contains("*/"))
                                {
                                    commented = false;
                                    CheckMultiLineBlockCommentsEndAndAdd(blockcomment + lines[k], ref tokens, i);
                                }
                                blockcomment += CheckMultiLineBlockComments(lines[k], ref tokens, i);
                                blockcomment += "\\n";
                                k++;
                            } while (commented && k < lines.Length);
                        } else
                        {
                            ((List<Token?>)tokens).AddRange(Tokenize(lines[k], i));
                        }
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
                //handle inline comments
                if (line.Contains("//"))
                {
                    CheckInlineComments(line, ref tokens, i);
                    return tokens;
                }

                //handle single line block comments
                if (line.Contains("/*") && line.Contains("*/"))
                {
                    CheckLineBlockComments(line, ref tokens, i);
                    return tokens;
                }

                //handle any other tokens
                string[] values = line.Split();

                for (int j = 0; j < values.Length; j++)
                {
                    if (!String.IsNullOrEmpty(values[j]))
                    {
                        //regular tokens
                        tokens.Add(CreateToken(values[j], i, ref tokens));   
                    }
                }
            }
            return tokens;
        }
    }
}
