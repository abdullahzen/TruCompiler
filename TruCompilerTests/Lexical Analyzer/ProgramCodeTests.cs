using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TruCompiler.Lexical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompilerTests.Lexical_Analyzer
{
    [TestClass]
    public class ProgramCodeTests
    {

        IList<Token> tokens;

        [TestInitialize]
        public void TestInitialize()
        {
            tokens = new List<Token>();
        }

        // Test data: if(abc==1){}
        // Regular single line of code without spaces        
        [TestMethod]
        public void TestIfStatement()
        {
            tokens = LexicalAnalyzer.Tokenize("if(abc==1){}");
            Assert.AreEqual(8, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].Lexeme);
            Assert.AreEqual(Lexeme.openpar, tokens[1].Lexeme);
            Assert.AreEqual(Lexeme.id, tokens[2].Lexeme);
            Assert.AreEqual(Lexeme.eqeq, tokens[3].Lexeme);
            Assert.AreEqual(Lexeme.intnum, tokens[4].Lexeme);
            Assert.AreEqual(Lexeme.closepar, tokens[5].Lexeme);
            Assert.AreEqual(Lexeme.opencbr, tokens[6].Lexeme);
            Assert.AreEqual(Lexeme.closecbr, tokens[7].Lexeme);
            Assert.AreEqual(1, tokens[0].Line);
            Assert.AreEqual(1, tokens[1].Line);
            Assert.AreEqual(1, tokens[2].Line);
            Assert.AreEqual(1, tokens[3].Line);
            Assert.AreEqual(1, tokens[4].Line);
            Assert.AreEqual(1, tokens[5].Line);
            Assert.AreEqual(1, tokens[6].Line);
            Assert.AreEqual(1, tokens[7].Line);
            Assert.IsTrue(tokens[0].IsValid);
            Assert.IsTrue(tokens[1].IsValid);
            Assert.IsTrue(tokens[2].IsValid);
            Assert.IsTrue(tokens[3].IsValid);
            Assert.IsTrue(tokens[4].IsValid);
            Assert.IsTrue(tokens[5].IsValid);
            Assert.IsTrue(tokens[6].IsValid);
            Assert.IsTrue(tokens[7].IsValid);
            Assert.AreEqual("if", tokens[0].Value);
            Assert.AreEqual("(", tokens[1].Value);
            Assert.AreEqual("abc", tokens[2].Value);
            Assert.AreEqual("==", tokens[3].Value);
            Assert.AreEqual("1", tokens[4].Value);
            Assert.AreEqual(")", tokens[5].Value);
            Assert.AreEqual("{", tokens[6].Value);
            Assert.AreEqual("}", tokens[7].Value);
        }

        // Test data: <>::<if==>x
        // Regular line of code without spaces
        [TestMethod]
        public void TestNoSpaces()
        {
            tokens = LexicalAnalyzer.Tokenize("<>::<if==>x");
            Assert.AreEqual(7, tokens.Count);
            Assert.AreEqual(Lexeme.noteq, tokens[0].Lexeme);
            Assert.AreEqual(Lexeme.coloncolon, tokens[1].Lexeme);
            Assert.AreEqual(Lexeme.lt, tokens[2].Lexeme);
            Assert.AreEqual(Lexeme.keyword, tokens[3].Lexeme);
            Assert.AreEqual(Lexeme.eqeq, tokens[4].Lexeme);
            Assert.AreEqual(Lexeme.gt, tokens[5].Lexeme);
            Assert.AreEqual(Lexeme.id, tokens[6].Lexeme);
            Assert.AreEqual(1, tokens[0].Line);
            Assert.AreEqual(1, tokens[1].Line);
            Assert.AreEqual(1, tokens[2].Line);
            Assert.AreEqual(1, tokens[3].Line);
            Assert.AreEqual(1, tokens[4].Line);
            Assert.AreEqual(1, tokens[5].Line);
            Assert.AreEqual(1, tokens[6].Line);
            Assert.IsTrue(tokens[0].IsValid);
            Assert.IsTrue(tokens[1].IsValid);
            Assert.IsTrue(tokens[2].IsValid);
            Assert.IsTrue(tokens[3].IsValid);
            Assert.IsTrue(tokens[4].IsValid);
            Assert.IsTrue(tokens[5].IsValid);
            Assert.IsTrue(tokens[6].IsValid);
            Assert.AreEqual("<>", tokens[0].Value);
            Assert.AreEqual("::", tokens[1].Value);
            Assert.AreEqual("<", tokens[2].Value);
            Assert.AreEqual("if", tokens[3].Value);
            Assert.AreEqual("==", tokens[4].Value);
            Assert.AreEqual(">", tokens[5].Value);
            Assert.AreEqual("x", tokens[6].Value);
        }

        // Test data: <>::<if==>_x
        // Regular line of code without spaces with illegal id that should be rejected
        [TestMethod]
        public void TestNoSpacesIllegalId()
        {
            tokens = LexicalAnalyzer.Tokenize("<>::<if==>_x");
            Assert.AreEqual(7, tokens.Count);
            Assert.AreEqual(Lexeme.noteq, tokens[0].Lexeme);
            Assert.AreEqual(Lexeme.coloncolon, tokens[1].Lexeme);
            Assert.AreEqual(Lexeme.lt, tokens[2].Lexeme);
            Assert.AreEqual(Lexeme.keyword, tokens[3].Lexeme);
            Assert.AreEqual(Lexeme.eqeq, tokens[4].Lexeme);
            Assert.AreEqual(Lexeme.gt, tokens[5].Lexeme);
            Assert.AreEqual(Lexeme.id, tokens[6].Lexeme);
            Assert.AreEqual(1, tokens[0].Line);
            Assert.AreEqual(1, tokens[1].Line);
            Assert.AreEqual(1, tokens[2].Line);
            Assert.AreEqual(1, tokens[3].Line);
            Assert.AreEqual(1, tokens[4].Line);
            Assert.AreEqual(1, tokens[5].Line);
            Assert.AreEqual(1, tokens[6].Line);
            Assert.IsTrue(tokens[0].IsValid);
            Assert.IsTrue(tokens[1].IsValid);
            Assert.IsTrue(tokens[2].IsValid);
            Assert.IsTrue(tokens[3].IsValid);
            Assert.IsTrue(tokens[4].IsValid);
            Assert.IsTrue(tokens[5].IsValid);
            Assert.IsFalse(tokens[6].IsValid);
            Assert.AreEqual("<>", tokens[0].Value);
            Assert.AreEqual("::", tokens[1].Value);
            Assert.AreEqual("<", tokens[2].Value);
            Assert.AreEqual("if", tokens[3].Value);
            Assert.AreEqual("==", tokens[4].Value);
            Assert.AreEqual(">", tokens[5].Value);
            Assert.AreEqual("_x", tokens[6].Value);
        }
    }

}
