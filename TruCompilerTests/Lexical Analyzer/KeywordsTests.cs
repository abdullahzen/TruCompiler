using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TruCompiler.Lexical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompilerTests.Lexical_Analyzer
{
    [TestClass]
    public class KeywordsTests
    {

        IList<Token?> tokens;

        [TestInitialize]
        public void TestInitialize()
        {
            tokens = new List<Token?>();
        }

        [TestMethod]
        public void TestKeywordIf()
        {
            tokens = LexicalAnalyzer.Tokenize("if");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.AreEqual("if", tokens[0].GetValueOrDefault().Value);
        }

        [TestMethod]
        public void TestKeywordThen()
        {
            tokens = LexicalAnalyzer.Tokenize("then");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.AreEqual("then", tokens[0].GetValueOrDefault().Value);
        }

        [TestMethod]
        public void TestKeywordElse()
        {
            tokens = LexicalAnalyzer.Tokenize("else");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.AreEqual("else", tokens[0].GetValueOrDefault().Value);
        }

        [TestMethod]
        public void TestKeywordWhile()
        {
            tokens = LexicalAnalyzer.Tokenize("while");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.AreEqual("while", tokens[0].GetValueOrDefault().Value);
        }

        [TestMethod]
        public void TestKeywordClass()
        {
            tokens = LexicalAnalyzer.Tokenize("class");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.AreEqual("class", tokens[0].GetValueOrDefault().Value);
        }

        [TestMethod]
        public void TestKeywordInteger()
        {
            tokens = LexicalAnalyzer.Tokenize("integer");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.AreEqual("integer", tokens[0].GetValueOrDefault().Value);
        }

        [TestMethod]
        public void TestKeywordFloat()
        {
            tokens = LexicalAnalyzer.Tokenize("float");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.AreEqual("float", tokens[0].GetValueOrDefault().Value);
        }

        [TestMethod]
        public void TestKeywordDo()
        {
            tokens = LexicalAnalyzer.Tokenize("do");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.AreEqual("do", tokens[0].GetValueOrDefault().Value);
        }

        [TestMethod]
        public void TestKeywordEnd()
        {
            tokens = LexicalAnalyzer.Tokenize("end");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.AreEqual("end", tokens[0].GetValueOrDefault().Value);
        }

        [TestMethod]
        public void TestKeywordPublic()
        {
            tokens = LexicalAnalyzer.Tokenize("public");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.AreEqual("public", tokens[0].GetValueOrDefault().Value);
        }

        [TestMethod]
        public void TestKeywordPrivate()
        {
            tokens = LexicalAnalyzer.Tokenize("private");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.AreEqual("private", tokens[0].GetValueOrDefault().Value);
        }

        [TestMethod]
        public void TestKeywordOr()
        {
            tokens = LexicalAnalyzer.Tokenize("or");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.AreEqual("or", tokens[0].GetValueOrDefault().Value);
        }

        [TestMethod]
        public void TestKeywordAnd()
        {
            tokens = LexicalAnalyzer.Tokenize("and");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.AreEqual("and", tokens[0].GetValueOrDefault().Value);
        }

        [TestMethod]
        public void TestKeywordNot()
        {
            tokens = LexicalAnalyzer.Tokenize("not");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.AreEqual("not", tokens[0].GetValueOrDefault().Value);
        }

        [TestMethod]
        public void TestKeywordRead()
        {
            tokens = LexicalAnalyzer.Tokenize("read");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.AreEqual("read", tokens[0].GetValueOrDefault().Value);
        }

        [TestMethod]
        public void TestKeywordWrite()
        {
            tokens = LexicalAnalyzer.Tokenize("write");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.AreEqual("write", tokens[0].GetValueOrDefault().Value);
        }

        [TestMethod]
        public void TestKeywordReturn()
        {
            tokens = LexicalAnalyzer.Tokenize("return");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.AreEqual("return", tokens[0].GetValueOrDefault().Value);
        }

        [TestMethod]
        public void TestKeywordMain()
        {
            tokens = LexicalAnalyzer.Tokenize("main");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.AreEqual("main", tokens[0].GetValueOrDefault().Value);
        }

        [TestMethod]
        public void TestKeywordInherits()
        {
            tokens = LexicalAnalyzer.Tokenize("inherits");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.AreEqual("inherits", tokens[0].GetValueOrDefault().Value);
        }

        [TestMethod]
        public void TestKeywordLocal()
        {
            tokens = LexicalAnalyzer.Tokenize("local");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.AreEqual("local", tokens[0].GetValueOrDefault().Value);
        }
    }
}
