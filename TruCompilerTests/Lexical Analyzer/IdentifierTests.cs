using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Lexical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompilerTests.Lexical_Analyzer
{
    [TestClass]
    public class IdentifierTests
    {
        IList<Token?> tokens;
        DynamicLexValidator dynamicLexValidator;


        [TestInitialize]
        public void TestInitialize()
        {
            tokens = new List<Token?>();
            dynamicLexValidator = new DynamicLexValidator();
        }

        // Test data: validID
        // A valid id with only letters [a-z][A-Z] should be accepted
        [TestMethod]
        public void TestValidIdentifierUsingTokenizeFunction()
        {
            tokens = LexicalAnalyzer.Tokenize("validID");

            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.id, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.AreEqual("validID", tokens[0].GetValueOrDefault().Value);

            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
        }

        // Test data: validID123_1valid
        // Should be accepted
        [TestMethod]
        public void TestValidIdentifierWithNumbersAndSymbolUsingTokenizeFunction()
        {
            tokens = LexicalAnalyzer.Tokenize("validID123_1valid");

            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.id, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.AreEqual("validID123_1valid", tokens[0].GetValueOrDefault().Value);

            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
        }

        // Test data: VALI_12l2idID123_1va3lid4_
        // Should be accepted
        [TestMethod]
        public void TestValidIdentifierExtremeNameUsingTokenizeFunction()
        {
            tokens = LexicalAnalyzer.Tokenize("VALI_12l2idID123_1va3lid4_");

            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.id, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.AreEqual("VALI_12l2idID123_1va3lid4_", tokens[0].GetValueOrDefault().Value);

            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
        }

        // Test data: _1VALI_12l2idID123_1va3lid4_
        // Should not be accepted because starting with _
        [TestMethod]
        public void TestInValidIdentifierExtremeNameUsingTokenizeFunction()
        {
            tokens = LexicalAnalyzer.Tokenize("_1VALI_12l2idID123_1va3lid4_");

            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.id, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.AreEqual("_1VALI_12l2idID123_1va3lid4_", tokens[0].GetValueOrDefault().Value);

            Assert.IsFalse(tokens[0].GetValueOrDefault().IsValid);
        }

        // Test data: 1VALI_12l2idID123_1va3lid4_
        // Should not be accepted because starting with a number
        [TestMethod]
        public void TestInValidIdentifierStartingWithNumUsingTokenizeFunction()
        {
            tokens = LexicalAnalyzer.Tokenize("1VALI_12l2idID123_1va3lid4_");

            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.id, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.AreEqual("1VALI_12l2idID123_1va3lid4_", tokens[0].GetValueOrDefault().Value);

            Assert.IsFalse(tokens[0].GetValueOrDefault().IsValid);
        }

        // Test data: @VA$LI_12l2%idID^123_1va3lid4_
        // Should not be accepted because illegal characters in language
        [TestMethod]
        public void TestInValidIdentifierWithIllegalCharactersUsingTokenizeFunction()
        {
            tokens = LexicalAnalyzer.Tokenize("@VA$LI_12l2%idID^123_1va3lid4_");

            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.id, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.AreEqual("@VA$LI_12l2%idID^123_1va3lid4_", tokens[0].GetValueOrDefault().Value);

            Assert.IsFalse(tokens[0].GetValueOrDefault().IsValid);
        }

        // Test data: VA$LI_12l2%idID^123_1va3lid4_
        // Should not be accepted because illegal characters in language
        [TestMethod]
        public void TestInValidIdentifierValidStartWithIllegalCharactersUsingTokenizeFunction()
        {
            tokens = LexicalAnalyzer.Tokenize("VA$LI_12l2%idID^123_1va3lid4_");

            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.id, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Location);
            Assert.AreEqual("VA$LI_12l2%idID^123_1va3lid4_", tokens[0].GetValueOrDefault().Value);

            Assert.IsFalse(tokens[0].GetValueOrDefault().IsValid);
        }

        // Test data: validID
        // A valid id with only letters [a-z][A-Z] should be accepted
        [TestMethod]
        public void TestValidIdentifierUsingDynamicValidator()
        {
            Assert.IsTrue(dynamicLexValidator.Validate("validID", "Identifier"));
        }

        // Test data: validID123_1valid
        // Should be accepted
        [TestMethod]
        public void TestValidIdentifierWithNumbersAndSymbolUsingDynamicValidator()
        {
            Assert.IsTrue(dynamicLexValidator.Validate("validID123_1valid", "Identifier"));
        }

        // Test data: VALI_12l2idID123_1va3lid4_
        // Should be accepted
        [TestMethod]
        public void TestValidIdentifierExtremeNameUsingDynamicValidator()
        {
            Assert.IsTrue(dynamicLexValidator.Validate("VALI_12l2idID123_1va3lid4_", "Identifier"));
        }

        // Test data: _1VALI_12l2idID123_1va3lid4_
        // Should not be accepted because starting with _
        [TestMethod]
        public void TestInValidIdentifierExtremeNameUsingDynamicValidator()
        {
            Assert.IsFalse(dynamicLexValidator.Validate("_1VALI_12l2idID123_1va3lid4_", "Identifier"));
        }

        // Test data: 1VALI_12l2idID123_1va3lid4_
        // Should not be accepted because starting with a number
        [TestMethod]
        public void TestInValidIdentifierStartingWithNumUsingDynamicValidator()
        {
            Assert.IsFalse(dynamicLexValidator.Validate("1VALI_12l2idID123_1va3lid4_", "Identifier"));
        }

        // Test data: @VA$LI_12l2%idID^123_1va3lid4_
        // Should not be accepted because illegal characters in language
        [TestMethod]
        public void TestInValidIdentifierWithIllegalCharactersUsingDynamicValidator()
        {
            Assert.IsFalse(dynamicLexValidator.Validate("@VA$LI_12l2%idID^123_1va3lid4_", "Identifier"));
        }

        // Test data: VA$LI_12l2%idID^123_1va3lid4_
        // Should not be accepted because illegal characters in language
        [TestMethod]
        public void TestInValidIdentifierValidStartWithIllegalCharactersUsingDynamicValidator()
        {
            Assert.IsFalse(dynamicLexValidator.Validate("VA$LI_12l2%idID^123_1va3lid4_", "Identifier"));
        }


    }
}
