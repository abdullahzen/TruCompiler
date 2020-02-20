using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Sentactical_Analyzer
{
    public class SyntacticalAnalyzer
    { 
        public static TreeNode<Token> AnalyzeSyntax(TokenScanner tokenScanner)
        {
            Rules rules = new Rules(tokenScanner);
            TreeNode<Token> tree = rules.Start();
            return tree;
        }

        public static string[] GenerateDiGraph(TreeNode<Token> node, ref int index)
        {
            string[] result = new string[]{ "", ""};
            int parent = index;
            if (node.Parent == null)
            {
                result[0] += String.Format("{0}[label=\"{1}\"]\n", index, GetValueFromNode(node.Value));
                result[1] += String.Format("{0}->{1}\n", index, index + 1);
            }
            foreach (var child in node.Children)
            {
                index++;
                result[0] += String.Format("{0}[label=\"{1}\"]\n", index, GetValueFromNode(child.Value));
                result[1] += String.Format("{0}->{1}\n", parent, index);
                if (child.Children.Count > 0)
                {
                    string[] arr = new string[2];
                    arr = GenerateDiGraph(child, ref index);
                    result[0] += arr[0];
                    result[1] += arr[1];
                }
            }
            return result;
        }

        private static string GetValueFromNode(Token token)
        {
            if (token != null)
            {
                if (token.Lexeme == Lexeme.keyword)
                {
                    return token.Value;
                } else if (token.Lexeme == Lexeme.intnum || token.Lexeme == Lexeme.floatnum || token.Lexeme == Lexeme.id)
                {
                    return String.Format("{0}:{1}", token.Lexeme.ToString(), token.Value);
                } else
                {
                    return token.Lexeme.ToString();
                }
            }
            return "Empty";
        }
    }
}
