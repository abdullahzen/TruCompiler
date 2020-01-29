using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Lexical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompilerTests.Lexical_Analyzer
{
    [TestClass]
    public class IntegerTests
    {
        IList<Token?> tokens;
        DynamicLexValidator dynamicLexValidator;
        

        [TestInitialize]
        public void TestInitialize()
        {
            tokens = new List<Token?>();
            dynamicLexValidator = new DynamicLexValidator();
        }

        // Test data: 0
        // A single zero should be accepted as an intnum
        [TestMethod]
        public void TestASingleZeroIntUsingTokenizeFunction()
        {
            tokens = LexicalAnalyzer.Tokenize("0");

            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.intnum, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.AreEqual("0", tokens[0].GetValueOrDefault().Value);
        }

        // Test data: 0123
        // Numbers starting with 0 are not accepted
        [TestMethod]
        public void TestNumberStartWithZeroIntUsingTokenizeFunction()
        {
            tokens = LexicalAnalyzer.Tokenize("0123");

            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.intnum, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.AreEqual("0123", tokens[0].GetValueOrDefault().Value);

            Assert.IsFalse(tokens[0].GetValueOrDefault().IsValid);
        }

        // Test data: 12384333
        // Regular integer number should be accepted as intnum
        [TestMethod]
        public void TestRegularInt()
        {
            tokens = LexicalAnalyzer.Tokenize("12384333");

            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.intnum, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.AreEqual("12384333", tokens[0].GetValueOrDefault().Value);
        }

        // Test data: 1238e4333
        // Invalid number with string in it should be considered invalid id
        [TestMethod]
        public void TestInvalidNumberWithCharInTheMiddle()
        {
            tokens = LexicalAnalyzer.Tokenize("1238e4333");

            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.id, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsFalse(tokens[0].GetValueOrDefault().IsValid);
            Assert.AreEqual("1238e4333", tokens[0].GetValueOrDefault().Value);
        }

        // Test data: 102030490
        // Regular integer that should be accepted
        [TestMethod]
        public void TestValidIntegerWithZeros()
        {
            tokens = LexicalAnalyzer.Tokenize("102030490");

            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.intnum, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.AreEqual("102030490", tokens[0].GetValueOrDefault().Value);
        }

        // Test data: 102030490
        // Regular integer that should be accepted
        [TestMethod]
        public void TestValidIntegerWithZerosUsingDynamicValidator()
        {
            Assert.IsTrue(dynamicLexValidator.Validate("102030490", "Integer"));
        }

        // Test data: 0
        // A single zero should be accepted as an intnum
        [TestMethod]
        public void TestASingleZeroIntUsingDeynamicValidator()
        {
            Assert.IsTrue(dynamicLexValidator.Validate("0", "Integer"));
        }

        // Test data: 0123
        // Numbers starting with 0 are not accepted
        [TestMethod]
        public void TestNumberStartWithZeroIntUsingDynamicValidator()
        {
            Assert.IsFalse(dynamicLexValidator.Validate("0123", "Integer"));
        }

        // Test data: 12384333
        // Regular integer number should be accepted as intnum
        [TestMethod]
        public void TestRegularIntUsingDynamicValidator()
        {
            Assert.IsTrue(dynamicLexValidator.Validate("12384333", "Integer"));
        }

        // Test data: 1238e4333
        // Invalid number with string in it should be considered invalid id
        [TestMethod]
        public void TestInvalidNumberWithCharInTheMiddleUsingDynamicValidator()
        {
            Assert.IsFalse(dynamicLexValidator.Validate("1238e4333", "Integer"));
        }

    }
}
