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
    public class TokenScannerTests
    {
        List<Token> tokens;
        TokenScanner tokenScanner;
        Token token1;
        Token token2;
        Token token3;
        Token token4;


        [TestInitialize]
        public void TestInitialize()
        {
            tokens = new List<Token>();
            token1 = new Token(Lexeme.keyword, "token1");
            token2 = new Token(Lexeme.keyword, "token2");
            token3 = new Token(Lexeme.keyword, "token3");
            token4 = new Token(Lexeme.keyword, "token4");
            tokens.Add(token1);
            tokens.Add(token2);
            tokens.Add(token3);
            tokens.Add(token4);
            tokenScanner = new TokenScanner(tokens);
        }

        [TestMethod]
        public void TestPeek()
        {
            Assert.IsTrue(tokenScanner.Peek().Equals(token1));
        }

        [TestMethod]
        public void TestNextToken()
        {
            Assert.IsTrue(tokenScanner.NextToken().Equals(token1));
            Assert.IsTrue(tokenScanner.Current.Equals(token1));
        }

        [TestMethod]
        public void TestTraverseWithScanner()
        {
            Assert.IsTrue(tokenScanner.NextToken().Equals(token1));
            tokenScanner.NextToken();
            tokenScanner.NextToken();
            Assert.IsTrue(tokenScanner.Current.Equals(token3));
            Assert.IsTrue(tokenScanner.Peek().Equals(token4));
        }

        [TestMethod]
        public void TestHasNextIsTrue()
        {
            Assert.IsTrue(tokenScanner.hasNext());
            tokenScanner.NextToken();
            tokenScanner.NextToken();
            Assert.IsTrue(tokenScanner.hasNext());
        }

        [TestMethod]
        public void TestHasNextIsFalse()
        {
            tokenScanner.NextToken();
            tokenScanner.NextToken();
            tokenScanner.NextToken();
            tokenScanner.NextToken();
            Assert.IsFalse(tokenScanner.hasNext());
        }
    }
}
