using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Lexical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompilerTests.Lexical_Analyzer
{
    [TestClass]
    public class FloatTests
    {
        IList<Token> tokens;
        DynamicLexValidator dynamicLexValidator;


        [TestInitialize]
        public void TestInitialize()
        {
            tokens = new List<Token>();
            dynamicLexValidator = new DynamicLexValidator();
        }

        // Test data: 0.0
        // A zero float should be accepted!!!
        [TestMethod]
        public void TestASingleZeroFloatUsingTokenizeFunction()
        {
            tokens = LexicalAnalyzer.Tokenize("0.0");

            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.floatnum, tokens[0].Lexeme);
            Assert.AreEqual(1, tokens[0].Line);
            Assert.AreEqual("0.0", tokens[0].Value);

            Assert.IsTrue(tokens[0].IsValid);
        }

        // Test data: 0.56
        // A good float that should be accepted
        [TestMethod]
        public void TestValidFloarWithZeroOnlyOnLeftHandSideUsingTokenizeFunction()
        {
            tokens = LexicalAnalyzer.Tokenize("0.56");

            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.floatnum, tokens[0].Lexeme);
            Assert.AreEqual(1, tokens[0].Line);
            Assert.AreEqual("0.56", tokens[0].Value);

            Assert.IsTrue(tokens[0].IsValid);
        }

        // Test data: 0.560
        // A non good float that shouldn't be accepted
        [TestMethod]
        public void TestValidFloarWithZeroOnlyOnLeftHandSideAndTrailingUsingTokenizeFunction()
        {
            tokens = LexicalAnalyzer.Tokenize("0.560");

            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.floatnum, tokens[0].Lexeme);
            Assert.AreEqual(1, tokens[0].Line);
            Assert.AreEqual("0.560", tokens[0].Value);

            Assert.IsFalse(tokens[0].IsValid);
        }

        // Test data: 0123.23
        // Floats starting with 0 are not accepted
        [TestMethod]
        public void TestNumberStartWithZeroFloatUsingTokenizeFunction()
        {
            tokens = LexicalAnalyzer.Tokenize("0123.23");

            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.floatnum, tokens[0].Lexeme);
            Assert.AreEqual(1, tokens[0].Line);
            Assert.AreEqual("0123.23", tokens[0].Value);

            Assert.IsFalse(tokens[0].IsValid);
        }

        // Test data: 1238.4333
        // Regular Float number should be accepted as floatnum
        [TestMethod]
        public void TestRegularFloatWithDot()
        {
            tokens = LexicalAnalyzer.Tokenize("1238.4333");

            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.floatnum, tokens[0].Lexeme);
            Assert.AreEqual(1, tokens[0].Line);
            Assert.IsTrue(tokens[0].IsValid);
            Assert.AreEqual("1238.4333", tokens[0].Value);
        }

        // Test data: 1238.43.33
        // Invalid float should NOT be accepted 
        [TestMethod]
        public void TestInvalidFloatWithTwoDots()
        {
            tokens = LexicalAnalyzer.Tokenize("1238.43.33");

            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.floatnum, tokens[0].Lexeme);
            Assert.AreEqual(1, tokens[0].Line);
            Assert.AreEqual("1238.43.33", tokens[0].Value);

            Assert.IsFalse(tokens[0].IsValid);
        }

        // Test data: 12384.333e+98
        // Regular float with exponential + should be accepted as floatnum
        [TestMethod]
        public void TestRegularFloatWithExponentialPlus()
        {
            tokens = LexicalAnalyzer.Tokenize("12384.333e+98");

            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.floatnum, tokens[0].Lexeme);
            Assert.AreEqual(1, tokens[0].Line);
            Assert.IsTrue(tokens[0].IsValid);
            Assert.AreEqual("12384.333e+98", tokens[0].Value);
        }

        // Test data: 94084.023e-98
        // Regular float with exponential + should be accepted as floatnum
        [TestMethod]
        public void TestRegularFloatWithExponentialMinus()
        {
            tokens = LexicalAnalyzer.Tokenize("94084.023e-98");

            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.floatnum, tokens[0].Lexeme);
            Assert.AreEqual(1, tokens[0].Line);
            Assert.IsTrue(tokens[0].IsValid);
            Assert.AreEqual("94084.023e-98", tokens[0].Value);
        }

        // Test data: 45.3e543
        // Invalid float with only exponential and no sign
        [TestMethod]
        public void TestInvalidFloatWithOnlyExponential()
        {
            tokens = LexicalAnalyzer.Tokenize("45.3e543");

            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.floatnum, tokens[0].Lexeme);
            Assert.AreEqual(1, tokens[0].Line);
            Assert.AreEqual("45.3e543", tokens[0].Value);

            Assert.IsFalse(tokens[0].IsValid);
        }

        // Test data: 234.3450
        // Invalid float with trailing zero shouldn't be accepted
        [TestMethod]
        public void TestInvalidFloatWithTrailingZero()
        {
            tokens = LexicalAnalyzer.Tokenize("234.3450");

            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.floatnum, tokens[0].Lexeme);
            Assert.AreEqual(1, tokens[0].Line);
            Assert.AreEqual("234.3450", tokens[0].Value);

            Assert.IsFalse(tokens[0].IsValid);
        }

        // Test data: 234.3450e+94
        // Invalid float exponential with trailing zero shouldn't be accepted
        [TestMethod]
        public void TestInvalidFloatWithExponentialTrailingZero()
        {
            tokens = LexicalAnalyzer.Tokenize("234.3450e+94");

            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.floatnum, tokens[0].Lexeme);
            Assert.AreEqual(1, tokens[0].Line);
            Assert.AreEqual("234.3450e+94", tokens[0].Value);

            Assert.IsFalse(tokens[0].IsValid);
        }

        // Test data: 0.345e+0
        // Valid float exponential with zero only
        [TestMethod]
        public void TestValidFloatWithExponentialZeroOnly()
        {
            tokens = LexicalAnalyzer.Tokenize("0.345e+0");

            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.floatnum, tokens[0].Lexeme);
            Assert.AreEqual(1, tokens[0].Line);
            Assert.AreEqual("0.345e+0", tokens[0].Value);

            Assert.IsTrue(tokens[0].IsValid);
        }

        // Test data: 234.345e+094
        // Invalid float exponential with invalid exponential
        [TestMethod]
        public void TestValidFloatWithInvalidExponential()
        {
            tokens = LexicalAnalyzer.Tokenize("234.345e+094");

            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(Lexeme.floatnum, tokens[0].Lexeme);
            Assert.AreEqual(1, tokens[0].Line);
            Assert.AreEqual("234.345e+094", tokens[0].Value);

            Assert.IsFalse(tokens[0].IsValid);
        }

        // Test data: 0.0
        // A zero float should be accepted
        [TestMethod]
        public void TestASingleZeroFloatUsingDynamicValidator()
        {
            Assert.IsTrue(dynamicLexValidator.Validate("0.0", "Float"));
        }

        // Test data: 0.56
        // A good float that should be accepted
        [TestMethod]
        public void TestValidFloarWithZeroOnlyOnLeftHandSideUsingDynamicValidator()
        {
            Assert.IsTrue(dynamicLexValidator.Validate("0.56", "Float"));
        }

        // Test data: 0.560
        // A non good float that shouldn't be accepted
        [TestMethod]
        public void TestValidFloarWithZeroOnlyOnLeftHandSideAndTrailingUsingDynamicValidaton()
        {
            Assert.IsFalse(dynamicLexValidator.Validate("0.560", "Float"));
        }

        // Test data: 0123.23
        // Floats starting with 0 are not accepted
        [TestMethod]
        public void TestNumberStartWithZeroFloatUsingDynamicValidator()
        {
            Assert.IsFalse(dynamicLexValidator.Validate("0123.23", "Float"));
        }

        // Test data: 1238.4333
        // Regular Float number should be accepted as floatnum
        [TestMethod]
        public void TestRegularFloatWithDotUsingDynamicValidator()
        {
            Assert.IsTrue(dynamicLexValidator.Validate("1238.4333", "Float"));
        }

        // Test data: 1238.43.33
        // Invalid float should NOT be accepted 
        [TestMethod]
        public void TestInvalidFloatWithTwoDotsUsingDynamicValidator()
        {
            Assert.IsFalse(dynamicLexValidator.Validate("1238.43.33", "Float"));
        }

        // Test data: 12384.333e+98
        // Regular float with exponential + should be accepted as floatnum
        [TestMethod]
        public void TestRegularFloatWithExponentialPlusUsingDynamicValidator()
        {
            Assert.IsTrue(dynamicLexValidator.Validate("12384.333e+98", "Float"));
        }

        // Test data: 94084.023e-98
        // Regular float with exponential + should be accepted as floatnum
        [TestMethod]
        public void TestRegularFloatWithExponentialMinusUsingDynamicValidator()
        {
            Assert.IsTrue(dynamicLexValidator.Validate("94084.023e-98", "Float"));
        }

        // Test data: 45.3e543
        // Invalid float with only exponential and no sign
        [TestMethod]
        public void TestInvalidFloatWithOnlyExponentialUsingDynamicValidator()
        {
            Assert.IsFalse(dynamicLexValidator.Validate("45.3e543", "Float"));
        }

        // Test data: 234.3450
        // Invalid float with trailing zero shouldn't be accepted
        [TestMethod]
        public void TestInvalidFloatWithTrailingZeroUsingDynamicValidator()
        {
            Assert.IsFalse(dynamicLexValidator.Validate("234.3450", "Float"));
        }

        // Test data: 234.3450e+94
        // Invalid float exponential with trailing zero shouldn't be accepted
        [TestMethod]
        public void TestInvalidFloatWithExponentialTrailingZeroUsingDynamicValidator()
        {
            Assert.IsFalse(dynamicLexValidator.Validate("234.3450e+94", "Float"));
        }

        // Test data: 0.345e+0
        // Valid float exponential with zero only
        [TestMethod]
        public void TestValidFloatWithExponentialZeroOnlyUsingDynamicValidator()
        {
            Assert.IsTrue(dynamicLexValidator.Validate("0.345e+0", "Float"));
        }

        // Test data: 234.345e+094
        // Invalid float exponential with invalid exponential
        [TestMethod]
        public void TestValidFloatWithInvalidExponentialUsingDynamicValidator()
        {
            Assert.IsFalse(dynamicLexValidator.Validate("234.345e+094", "Float"));
        }
    }
}
