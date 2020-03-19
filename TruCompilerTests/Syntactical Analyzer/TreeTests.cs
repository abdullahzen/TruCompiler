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
    public class TreeTests
    {
        Node<Token> syntaxTree;


        [TestInitialize]
        public void TestInitialize()
        {
            syntaxTree = new Node<Token>(new Token(Lexeme.keyword, "testNode"));
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
            Node<Token> testToken = new Node<Token>(new Token(Lexeme.id));
            syntaxTree.AddChild(testToken);
            Assert.IsTrue(syntaxTree.Children.Count > 0);
            Assert.IsTrue(syntaxTree.Children.Contains(testToken));
        }
    }
}
