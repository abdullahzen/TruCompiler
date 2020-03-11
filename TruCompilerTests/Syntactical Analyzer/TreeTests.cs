using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TruCompiler.Lexical_Analyzer;
using TruCompiler.Sentactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompilerTests.Syntactical_Analyzer
{
    [TestClass]
    public class TreeTests
    {
        TreeNode<Token> syntaxTree;


        [TestInitialize]
        public void TestInitialize()
        {
            syntaxTree = new TreeNode<Token>(new Token(Lexeme.keyword, "testNode"));
        }

        [TestMethod]
        public void TestAddChildAsToken()
        {
            Token testToken = new Token(Lexeme.id);
            syntaxTree.AddChild(testToken);
            Assert.IsTrue(syntaxTree.Children.Count > 0);
            Assert.IsTrue(syntaxTree.Children[0].Value.Equals(testToken));
        }

        [TestMethod]
        public void TestAddChildAsTreeNode()
        {
            TreeNode<Token> testToken = new TreeNode<Token>(new Token(Lexeme.id));
            syntaxTree.AddChild(testToken);
            Assert.IsTrue(syntaxTree.Children.Count > 0);
            Assert.IsTrue(syntaxTree.Children.Contains(testToken));
        }
    }
}
