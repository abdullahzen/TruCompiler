using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Lexical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompilerTests.Lexical_Analyzer
{
    [TestClass]
    public class BlockCommentsTests
    {
        IList<Token?> tokens;
        string[] lines;

        [TestInitialize]
        public void TestInitialize()
        {
            tokens = new List<Token?>();
        }

        // Test data: /* this is a regular single line block comment */
        // Regular single line block comment should be identified as a block comment
        [TestMethod]
        public void TestSingleLineBlockComment()
        {
            tokens = LexicalAnalyzer.Tokenize("/* this is a regular single line block comment */");
            Assert.AreEqual(3, tokens.Count);
            Assert.AreEqual(Lexeme.opencmt, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.blockcmt, tokens[1].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.closecmt, tokens[2].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[1].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[2].GetValueOrDefault().Line);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[1].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[2].GetValueOrDefault().IsValid);
            Assert.AreEqual("/*", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual("/* this is a regular single line block comment */", tokens[1].GetValueOrDefault().Value);
            Assert.AreEqual("*/", tokens[2].GetValueOrDefault().Value);
        }

        // Test data: if = /* this is also a single line block comment */
        // Having code before the comment still keeps the code recognizable
        [TestMethod]
        public void TestSingleBlockCommentAfterCodeUsingTokenizeFunction()
        {
            tokens = LexicalAnalyzer.Tokenize("if = /* this is also a single line block comment */");
            Assert.AreEqual(5, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.eq, tokens[1].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.opencmt, tokens[2].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.blockcmt, tokens[3].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.closecmt, tokens[4].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[1].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[2].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[3].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[4].GetValueOrDefault().Line);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[1].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[2].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[3].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[4].GetValueOrDefault().IsValid);
            Assert.AreEqual("if", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual("=", tokens[1].GetValueOrDefault().Value);
            Assert.AreEqual("/*", tokens[2].GetValueOrDefault().Value);
            Assert.IsTrue("/* this is also a single line block comment */".Equals(tokens[3].GetValueOrDefault().Value));
            Assert.AreEqual("*/", tokens[4].GetValueOrDefault().Value);
        }

        // Test data: /*this\tis a comment inlineif = */
        // Having inline comment before code makes code commented
        [TestMethod]
        public void TestSingleBlockCommentBeforeCodeUsingTokenizeFunctionShouldBeCommented()
        {
            tokens = LexicalAnalyzer.Tokenize("/*this\tis a comment inlineif = */");
            Assert.AreEqual(3, tokens.Count);
            Assert.AreEqual(Lexeme.opencmt, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.blockcmt, tokens[1].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.closecmt, tokens[2].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[1].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[2].GetValueOrDefault().Line);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[1].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[2].GetValueOrDefault().IsValid);
            Assert.AreEqual("/*", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual("/*this\tis a comment inlineif = */", tokens[1].GetValueOrDefault().Value);
            Assert.AreEqual("*/", tokens[2].GetValueOrDefault().Value);
        }

        // Test data: /*this\tis a\tcomment\tinline*/if = 
        // All tabs are recognized properly and kept in their places in the comment
        // Code after comment is recognized
        [TestMethod]
        public void TestSingleBlockCommentBeforeCodeUsingTokenizeFunctionWithTabsInMiddleAndEnd()
        {
            tokens = LexicalAnalyzer.Tokenize("/*this\tis a\tcomment\tinline*/if = ");
            Assert.AreEqual(5, tokens.Count);
            Assert.AreEqual(Lexeme.opencmt, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.blockcmt, tokens[1].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.closecmt, tokens[2].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.keyword, tokens[3].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.eq, tokens[4].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[1].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[2].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[3].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[4].GetValueOrDefault().Line);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[1].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[2].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[3].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[4].GetValueOrDefault().IsValid);
            Assert.AreEqual("/*", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual("/*this\tis a\tcomment\tinline*/", tokens[1].GetValueOrDefault().Value);
            Assert.AreEqual("*/", tokens[2].GetValueOrDefault().Value);
            Assert.AreEqual("if", tokens[3].GetValueOrDefault().Value);
            Assert.AreEqual("=", tokens[4].GetValueOrDefault().Value);
        }

        // Test data: if/*comment\t = \t*/
        // if is recognized before the comment start
        [TestMethod]
        public void TestSingleBlockCommentSymbolInMiddleOfWordThatHasCodeAndCommentUsingTokenizeFunction()
        {
            tokens = LexicalAnalyzer.Tokenize("if/*comment\t = \t*/");
            Assert.AreEqual(4, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.opencmt, tokens[1].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.blockcmt, tokens[2].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.closecmt, tokens[3].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[1].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[2].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[3].GetValueOrDefault().Line);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[1].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[2].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[3].GetValueOrDefault().IsValid);
            Assert.AreEqual("if", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual("/*", tokens[1].GetValueOrDefault().Value);
            Assert.AreEqual("/*comment\t = \t*/", tokens[2].GetValueOrDefault().Value);
            Assert.AreEqual("*/", tokens[3].GetValueOrDefault().Value);
        }

        // Test data: if /* comment\t = \t */
        // if is recognized before the comment start
        [TestMethod]
        public void TestSingleBlockCommentSymbolBetweenCodeAndCommentWithASapceInCommentUsingTokenizeFunction()
        {
            tokens = LexicalAnalyzer.Tokenize("if /* comment\t = \t */");
            Assert.AreEqual(4, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.opencmt, tokens[1].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.blockcmt, tokens[2].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.closecmt, tokens[3].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[1].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[2].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[3].GetValueOrDefault().Line);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[1].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[2].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[3].GetValueOrDefault().IsValid);
            Assert.AreEqual("if", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual("/*", tokens[1].GetValueOrDefault().Value);
            Assert.AreEqual("/* comment\t = \t */", tokens[2].GetValueOrDefault().Value);
            Assert.AreEqual("*/", tokens[3].GetValueOrDefault().Value);
        }

        // Test data: if /* comment\t = \t */= while do
        // if is recognized before the comment start
        [TestMethod]
        public void TestSingleBlockCommentBetweenCodeAndCommentWithASapceInCommentWithMultipleCommentSymbolsUsingTokenizeFunction()
        {
            tokens = LexicalAnalyzer.Tokenize("if /* comm/*ent\t = \t */= while do");
            Assert.AreEqual(7, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.opencmt, tokens[1].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.blockcmt, tokens[2].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.closecmt, tokens[3].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.eq, tokens[4].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.keyword, tokens[5].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.keyword, tokens[6].GetValueOrDefault().Lexeme);


            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[1].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[2].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[3].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[4].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[5].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[6].GetValueOrDefault().Line);

            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[1].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[2].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[3].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[4].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[5].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[6].GetValueOrDefault().IsValid);

            Assert.AreEqual("if", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual("/*", tokens[1].GetValueOrDefault().Value);
            Assert.AreEqual("/* comm/*ent\t = \t */", tokens[2].GetValueOrDefault().Value);
            Assert.AreEqual("*/", tokens[3].GetValueOrDefault().Value);
            Assert.AreEqual("=", tokens[4].GetValueOrDefault().Value);
            Assert.AreEqual("while", tokens[5].GetValueOrDefault().Value);
            Assert.AreEqual("do", tokens[6].GetValueOrDefault().Value);
        }

        // Test data: /* this is a multiple
        // line block co
        // mment 
        // */
        // Regular multi line block comment should be identified as a block comment
        [TestMethod]
        public void TestMultiLineBlockComment()
        {
            lines = new string[] { "/* this is a multiple", "line block co", "mment", "*/" };
            tokens = LexicalAnalyzer.Tokenize(lines);
            Assert.AreEqual(3, tokens.Count);
            Assert.AreEqual(Lexeme.opencmt, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.blockcmt, tokens[1].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.closecmt, tokens[2].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[1].GetValueOrDefault().Line);
            Assert.AreEqual(4, tokens[2].GetValueOrDefault().Line);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[1].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[2].GetValueOrDefault().IsValid);
            Assert.AreEqual("/*", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual("/* this is a multiple\\nline block co\\nmment\\n*/", tokens[1].GetValueOrDefault().Value);
            Assert.AreEqual("*/", tokens[2].GetValueOrDefault().Value);
        }

        // Test data: 
        // if = /*thi
        // s is also a multi line block comment*/
        // Having code before the comment still keeps the code recognizable
        [TestMethod]
        public void TestMultiBlockCommentAfterCodeUsingTokenizeFunction()
        {
            lines = new string[] { "if = /*thi", "s is also a multi line block comment*/"};
            tokens = LexicalAnalyzer.Tokenize(lines);
            Assert.AreEqual(5, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.eq, tokens[1].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.opencmt, tokens[2].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.blockcmt, tokens[3].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.closecmt, tokens[4].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[1].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[2].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[3].GetValueOrDefault().Line);
            Assert.AreEqual(2, tokens[4].GetValueOrDefault().Line);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[1].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[2].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[3].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[4].GetValueOrDefault().IsValid);
            Assert.AreEqual("if", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual("=", tokens[1].GetValueOrDefault().Value);
            Assert.AreEqual("/*", tokens[2].GetValueOrDefault().Value);
            Assert.IsTrue("/*thi\\ns is also a multi line block comment*/".Equals(tokens[3].GetValueOrDefault().Value));
            Assert.AreEqual("*/", tokens[4].GetValueOrDefault().Value);
        }

        // Test data: /*this\tis a comment 
        // inlineif = */
        // Having inline comment before code makes code commented
        [TestMethod]
        public void TestMultiBlockCommentBeforeCodeUsingTokenizeFunctionShouldBeCommented()
        {
            lines = new string[] { "/*this\tis a comment", "inlineif = */"};
            tokens = LexicalAnalyzer.Tokenize(lines);
            Assert.AreEqual(3, tokens.Count);
            Assert.AreEqual(Lexeme.opencmt, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.blockcmt, tokens[1].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.closecmt, tokens[2].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[1].GetValueOrDefault().Line);
            Assert.AreEqual(2, tokens[2].GetValueOrDefault().Line);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[1].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[2].GetValueOrDefault().IsValid);
            Assert.AreEqual("/*", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual("/*this\tis a comment\\ninlineif = */", tokens[1].GetValueOrDefault().Value);
            Assert.AreEqual("*/", tokens[2].GetValueOrDefault().Value);
        }

        // Test data: /*this\tis a\tcom
        // ment\tinl
        // ine*/if = 
        // All tabs are recognized properly and kept in their places in the comment
        // Code after comment is recognized
        [TestMethod]
        public void TestMultiBlockCommentBeforeCodeUsingTokenizeFunctionWithTabsInMiddleAndEnd()
        {
            lines = new string[] { "/*this\tis a\tcom","ment\tinl","ine*/if =" };
            tokens = LexicalAnalyzer.Tokenize(lines);
            Assert.AreEqual(5, tokens.Count);
            Assert.AreEqual(Lexeme.opencmt, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.blockcmt, tokens[1].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.closecmt, tokens[2].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.keyword, tokens[3].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.eq, tokens[4].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[1].GetValueOrDefault().Line);
            Assert.AreEqual(3, tokens[2].GetValueOrDefault().Line);
            Assert.AreEqual(3, tokens[3].GetValueOrDefault().Line);
            Assert.AreEqual(3, tokens[4].GetValueOrDefault().Line);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[1].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[2].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[3].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[4].GetValueOrDefault().IsValid);
            Assert.AreEqual("/*", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual("/*this\tis a\tcom\\nment\tinl\\nine*/", tokens[1].GetValueOrDefault().Value);
            Assert.AreEqual("*/", tokens[2].GetValueOrDefault().Value);
            Assert.AreEqual("if", tokens[3].GetValueOrDefault().Value);
            Assert.AreEqual("=", tokens[4].GetValueOrDefault().Value);
        }

        // Test data: if/*co/*mmen
        // t\t = \t*/
        // if is recognized before the comment start
        [TestMethod]
        public void TestMultiBlockCommentWithMultipleOpenCommentSymbolsAndCodeBefore()
        {
            lines = new string[] { "if/*co/*mmen", "t\t =\t*/" };
            tokens = LexicalAnalyzer.Tokenize(lines);
            Assert.AreEqual(4, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.opencmt, tokens[1].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.blockcmt, tokens[2].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.closecmt, tokens[3].GetValueOrDefault().Lexeme);
            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[1].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[2].GetValueOrDefault().Line);
            Assert.AreEqual(2, tokens[3].GetValueOrDefault().Line);
            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[1].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[2].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[3].GetValueOrDefault().IsValid);
            Assert.AreEqual("if", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual("/*", tokens[1].GetValueOrDefault().Value);
            Assert.AreEqual("/*co/*mmen\\nt\t =\t*/", tokens[2].GetValueOrDefault().Value);
            Assert.AreEqual("*/", tokens[3].GetValueOrDefault().Value);
        }
       

        // Test data: if /* co/*mm
        // ent\t 
        // = \t */= while do
        // if is recognized before the comment start and - while do after it
        [TestMethod]
        public void TestMultiBlockCommentBetweenCodeAndCommentWithASapceInCommentWithMultipleCommentSymbolsUsingTokenizeFunction()
        {
            lines = new string[] { "if /* co/*mm", "ent\t", "= \t */= while do" };

            tokens = LexicalAnalyzer.Tokenize(lines);
            Assert.AreEqual(7, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.opencmt, tokens[1].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.blockcmt, tokens[2].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.closecmt, tokens[3].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.eq, tokens[4].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.keyword, tokens[5].GetValueOrDefault().Lexeme);
            Assert.AreEqual(Lexeme.keyword, tokens[6].GetValueOrDefault().Lexeme);


            Assert.AreEqual(1, tokens[0].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[1].GetValueOrDefault().Line);
            Assert.AreEqual(1, tokens[2].GetValueOrDefault().Line);
            Assert.AreEqual(3, tokens[3].GetValueOrDefault().Line);
            Assert.AreEqual(3, tokens[4].GetValueOrDefault().Line);
            Assert.AreEqual(3, tokens[5].GetValueOrDefault().Line);
            Assert.AreEqual(3, tokens[6].GetValueOrDefault().Line);

            Assert.IsTrue(tokens[0].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[1].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[2].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[3].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[4].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[5].GetValueOrDefault().IsValid);
            Assert.IsTrue(tokens[6].GetValueOrDefault().IsValid);

            Assert.AreEqual("if", tokens[0].GetValueOrDefault().Value);
            Assert.AreEqual("/*", tokens[1].GetValueOrDefault().Value);
            Assert.AreEqual("/* co/*mm\\nent\t\\n= \t */", tokens[2].GetValueOrDefault().Value);
            Assert.AreEqual("*/", tokens[3].GetValueOrDefault().Value);
            Assert.AreEqual("=", tokens[4].GetValueOrDefault().Value);
            Assert.AreEqual("while", tokens[5].GetValueOrDefault().Value);
            Assert.AreEqual("do", tokens[6].GetValueOrDefault().Value);
        }

    }
}
