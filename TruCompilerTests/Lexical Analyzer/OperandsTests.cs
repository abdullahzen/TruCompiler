using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Lexical_Analyzer;
using static TruCompiler.Lexical_Analyzer.LexicalAnalyzer;

namespace TruCompilerTests.Lexical_Analyzer
{
    [TestClass]
    public class OperandsTests
    {
        IList<Token?> tokens;

        [TestInitialize]
        public void TestInitialize()
        {
            tokens = new List<Token?>();
        }

        [TestMethod]
        public void TestKeywordEq()
        {
            tokens = LexicalAnalyzer.Tokenize("==");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.eq, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual("==", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
        }

        [TestMethod]
        public void TestKeywordNotEq()
        {
            tokens = LexicalAnalyzer.Tokenize("<>");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.noteq, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual("<>", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
        }

        [TestMethod]
        public void TestKeywordIf()
        {
            tokens = LexicalAnalyzer.Tokenize("==");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.eq, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual("==", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
        }
        [TestMethod]
        public void TestKeywordLt()
        {
            tokens = LexicalAnalyzer.Tokenize("<");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.lt, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual("<", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
        }

        [TestMethod]
        public void TestKeywordGt()
        {
            tokens = LexicalAnalyzer.Tokenize(">");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.gt, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(">", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
        }

        [TestMethod]
        public void TestKeywordLeq()
        {
            tokens = LexicalAnalyzer.Tokenize("<=");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.leq, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual("<=", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
        }

        [TestMethod]
        public void TestKeywordGeq()
        {
            tokens = LexicalAnalyzer.Tokenize(">=");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.geq, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(">=", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
        }
        [TestMethod]
        public void TestKeywordPlus()
        {
            tokens = LexicalAnalyzer.Tokenize("+");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.plus, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual("+", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
        }

        [TestMethod]
        public void TestKeywordMinus()
        {
            tokens = LexicalAnalyzer.Tokenize("-");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.minus, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual("-", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
        }

        [TestMethod]
        public void TestKeywordMult()
        {
            tokens = LexicalAnalyzer.Tokenize("*");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.mult, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual("*", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
        }

        [TestMethod]
        public void TestKeywordDiv()
        {
            tokens = LexicalAnalyzer.Tokenize("/");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.div, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual("/", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
        }

        [TestMethod]
        public void TestKeywordEq2()
        {
            tokens = LexicalAnalyzer.Tokenize("=");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.eq, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual("=", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
        }

        [TestMethod]
        public void TestKeywordOpenPar()
        {
            tokens = LexicalAnalyzer.Tokenize("(");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.openpar, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual("(", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
        }

        [TestMethod]
        public void TestKeywordClosePar()
        {
            tokens = LexicalAnalyzer.Tokenize(")");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.closepar, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(")", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
        }

        [TestMethod]
        public void TestKeywordOpencbr()
        {
            tokens = LexicalAnalyzer.Tokenize("{");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.opencbr, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual("{", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
        }
        [TestMethod]
        public void TestKeywordClosecbr()
        {
            tokens = LexicalAnalyzer.Tokenize("}");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.closecbr, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual("}", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
        }

        [TestMethod]
        public void TestKeywordOpensqbr()
        {
            tokens = LexicalAnalyzer.Tokenize("[");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.opensqbr, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual("[", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
        }

        [TestMethod]
        public void TestKeywordClosesqbr()
        {
            tokens = LexicalAnalyzer.Tokenize("]");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.closesqbr, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual("]", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
        }

        [TestMethod]
        public void TestKeywordOpencmt()
        {
            tokens = LexicalAnalyzer.Tokenize("/*");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.opencmt, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual("/*", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
        }

        [TestMethod]
        public void TestKeywordClosecmt()
        {
            tokens = LexicalAnalyzer.Tokenize("*/");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.closecmt, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual("*/", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
        }

        [TestMethod]
        public void TestKeywordInlinecmt()
        {
            tokens = LexicalAnalyzer.Tokenize("//");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.inlinecmt, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual("//", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
        }
    }
}
