using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Lexical_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Semantic_Analyzer
{
    public class ASTVisitor : Visitor<Token>
    {
        public override void visit(Node<Token> node)
        {
            int parent = Driver.ASTIndex;
            if (node.Parent == null)
            {
                Driver.ASTResult[0] += String.Format("{0}[label=\"{1}\"]\n", Driver.ASTIndex, SyntacticalAnalyzer.GetValueFromNode(node.Value));
                Driver.ASTResult[1] += String.Format("{0}->{1}\n", Driver.ASTIndex, Driver.ASTIndex + 1);
            }
            foreach (var child in node.Children)
            {
                Driver.ASTIndex++;
                if (child.Value != null)
                {
                    Driver.ASTResult[0] += String.Format("{0}[label=\"{1}\"]\n", Driver.ASTIndex, SyntacticalAnalyzer.GetValueFromNode(child.Value));
                    Driver.ASTResult[1] += String.Format("{0}->{1}\n", parent, Driver.ASTIndex);
                }
                if (child.Children.Count > 0)
                {
                    child.accept(this);
                }
            }
        }
    }
}
