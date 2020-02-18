using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TruCompiler.Lexical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompilerTests.Lexical_Analyzer
{
    [TestClass]
    public class PunctuationsTests
    {
        IList<Token> tokens;

        [TestInitialize]
        public void TestInitialize()
        {
            tokens = new List<Token>();
        }

        [TestMethod]
        public void TestKeywordSemi()
        {
            tokens = LexicalAnalyzer.Tokenize(";");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.semi, tokens[0].Lexeme);
            Assert.AreEqual(";", tokens[0].Value);
            Assert.AreEqual(1, tokens[0].Line);
            Assert.IsTrue(tokens[0].IsValid);
        }

        [TestMethod]
        public void TestKeywordComma()
        {
            tokens = LexicalAnalyzer.Tokenize(",");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.comma, tokens[0].Lexeme);
            Assert.AreEqual(",", tokens[0].Value);
            Assert.AreEqual(1, tokens[0].Line);
            Assert.IsTrue(tokens[0].IsValid);
        }

        [TestMethod]
        public void TestKeywordDot()
        {
            tokens = LexicalAnalyzer.Tokenize(".");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.dot, tokens[0].Lexeme);
            Assert.AreEqual(".", tokens[0].Value);
            Assert.AreEqual(1, tokens[0].Line);
            Assert.IsTrue(tokens[0].IsValid);
        }

        [TestMethod]
        public void TestKeywordColon()
        {
            tokens = LexicalAnalyzer.Tokenize(":");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.colon, tokens[0].Lexeme);
            Assert.AreEqual(":", tokens[0].Value);
            Assert.AreEqual(1, tokens[0].Line);
            Assert.IsTrue(tokens[0].IsValid);
        }

        [TestMethod]
        public void TestKeywordColonColon()
        {
            tokens = LexicalAnalyzer.Tokenize("::");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.coloncolon, tokens[0].Lexeme);
            Assert.AreEqual("::", tokens[0].Value);
            Assert.AreEqual(1, tokens[0].Line);
            Assert.IsTrue(tokens[0].IsValid);
        }
    }
}
