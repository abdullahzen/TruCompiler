using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TruCompiler.Lexical_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompilerTests.Syntactical_Analyzer
{
    [TestClass]
    public class RulesTests
    {
        IList<Token> tokens;
        TokenScanner tokenScanner;
        TreeNode<Token> syntaxTree;


        [TestInitialize]
        public void TestInitialize()
        {
            tokens = new List<Token>();
            syntaxTree = null;
        }

        [TestMethod]
        public void TestProgWithMainOnly()
        {
            string input = "main";
            tokens = LexicalAnalyzer.Tokenize(input);
            tokens = tokens.Where(t => t != null
                        && t.Lexeme != Lexeme.inlinecmt
                        && t.Lexeme != Lexeme.blockcmt
                        && t.Lexeme != Lexeme.closecmt
                        && t.Lexeme != Lexeme.opencmt).ToList();
            tokenScanner = new TokenScanner((List<Token>)tokens);
            syntaxTree = SyntacticalAnalyzer.AnalyzeSyntax(tokenScanner);
            Assert.IsNotNull(syntaxTree);
            Assert.IsTrue(syntaxTree.Value.Value == "Start");
            Assert.IsTrue(syntaxTree.Children[0].Value.Value == "Program");
            Assert.IsTrue(syntaxTree.Children[0].Children[0].Value.Value == "main");

        }

        [TestMethod]
        public void TestProgWithClassAndNoMain()
        {
            string input = @"class POLYNOMIAL {
                                public evaluate(float x) : float;
                            };";
            tokens = LexicalAnalyzer.Tokenize(input);
            tokens = tokens.Where(t => t != null
                        && t.Lexeme != Lexeme.inlinecmt
                        && t.Lexeme != Lexeme.blockcmt
                        && t.Lexeme != Lexeme.closecmt
                        && t.Lexeme != Lexeme.opencmt).ToList();
            tokenScanner = new TokenScanner((List<Token>)tokens);
            syntaxTree = SyntacticalAnalyzer.AnalyzeSyntax(tokenScanner);
            Assert.IsNotNull(syntaxTree);
            Assert.IsFalse(syntaxTree.Value.Value == "main");
        }

        [TestMethod]
        public void TestMatchReturnFalseOnNoMain()
        {
            string input = @"class POLYNOMIAL {
                                public evaluate(float x) : float;
                            };";
            tokens = LexicalAnalyzer.Tokenize(input);
            tokens = tokens.Where(t => t != null
                        && t.Lexeme != Lexeme.inlinecmt
                        && t.Lexeme != Lexeme.blockcmt
                        && t.Lexeme != Lexeme.closecmt
                        && t.Lexeme != Lexeme.opencmt).ToList();
            tokenScanner = new TokenScanner((List<Token>)tokens);
            Rules rules = new Rules(tokenScanner);
            while (tokenScanner.hasNext())
            {
                if (tokenScanner.Current != null)
                {
                    Assert.IsFalse(tokenScanner.Current.Value == "main");
                }
                Assert.IsFalse(rules.Match(new Token(Lexeme.keyword, "main")));
                tokenScanner.NextToken();
            }
            Assert.IsNull(syntaxTree);
        }

        [TestMethod]
        public void ClassDeclWillNotAddAnythingToSyntaxTree()
        {
            string input = @"clasjss 1POLdYNOMIAL {
                                public evaluate(float x) : float;
                            };";
            tokens = LexicalAnalyzer.Tokenize(input);
            tokens = tokens.Where(t => t != null
                        && t.Lexeme != Lexeme.inlinecmt
                        && t.Lexeme != Lexeme.blockcmt
                        && t.Lexeme != Lexeme.closecmt
                        && t.Lexeme != Lexeme.opencmt).ToList();
            tokenScanner = new TokenScanner((List<Token>)tokens);
            Rules rules = new Rules(tokenScanner);
            syntaxTree = rules.ClassDecl();
            Assert.IsTrue(syntaxTree.Children.Count == 0);
        }

        [TestMethod]
        public void ClassDeclWithFlawedParams()
        {
            string input = @"class POLdYNOMIAL {
                                public evaluate(float kk kkk x) : float;
                            };";
            tokens = LexicalAnalyzer.Tokenize(input);
            tokens = tokens.Where(t => t != null
                        && t.Lexeme != Lexeme.inlinecmt
                        && t.Lexeme != Lexeme.blockcmt
                        && t.Lexeme != Lexeme.closecmt
                        && t.Lexeme != Lexeme.opencmt).ToList();
            tokenScanner = new TokenScanner((List<Token>)tokens);
            Rules rules = new Rules(tokenScanner);
            syntaxTree = rules.ClassDecl();
            Assert.IsTrue(syntaxTree.Children.Count != 0);
        }

        [TestMethod]
        public void FunctionHeadCorrectSyntax()
        {
            string input = @"LINEAR::evaluate(float x) : float";
            tokens = LexicalAnalyzer.Tokenize(input);
            tokens = tokens.Where(t => t != null
                        && t.Lexeme != Lexeme.inlinecmt
                        && t.Lexeme != Lexeme.blockcmt
                        && t.Lexeme != Lexeme.closecmt
                        && t.Lexeme != Lexeme.opencmt).ToList();
            tokenScanner = new TokenScanner((List<Token>)tokens);
            Rules rules = new Rules(tokenScanner);
            syntaxTree = rules.FuncHead();
            Assert.IsTrue(syntaxTree.Children.Count != 0);            
        }

        
    }
}
