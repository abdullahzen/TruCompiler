﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Lexical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompilerTests.Lexical_Analyzer
{
    [TestClass]
    public class InlineCommentsTests
    {
        IList<Token> tokens;
        string line;

        [TestInitialize]
        public void TestInitialize()
        {
            tokens = new List<Token>();
            line = "//";
        }

        // Test data: //this is a comment inline
        // Regular inline comment should be identified as a comment
        [TestMethod]
        public void TestWholeLineInlineCommentUsingTokenizeFunction()
        {
            tokens = LexicalAnalyzer.Tokenize(line + "this is a comment inline");
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(Lexeme.inlinecmt, tokens[0].Lexeme);
            Assert.AreEqual(Lexeme.inlinecmt, tokens[1].Lexeme);
            Assert.AreEqual(1, tokens[0].Line);
            Assert.AreEqual(1, tokens[1].Line);
            Assert.IsTrue(tokens[0].IsValid);
            Assert.IsTrue(tokens[1].IsValid);
            Assert.AreEqual("//", tokens[0].Value);
            Assert.AreEqual(line+"this is a comment inline", tokens[1].Value);
        }

        // Test data: if = //this is a comment inline
        // Having code before the inline comment still keeps the code recognizable
        [TestMethod]
        public void TestInlineCommentAfterCodeUsingTokenizeFunction()
        {
            tokens = LexicalAnalyzer.Tokenize("if = " + line + "this is a comment inline");
            Assert.AreEqual(4, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].Lexeme);
            Assert.AreEqual(Lexeme.eq, tokens[1].Lexeme);
            Assert.AreEqual(Lexeme.inlinecmt, tokens[2].Lexeme);
            Assert.AreEqual(Lexeme.inlinecmt, tokens[3].Lexeme);
            Assert.AreEqual(1, tokens[0].Line);
            Assert.AreEqual(1, tokens[1].Line);
            Assert.AreEqual(1, tokens[2].Line);
            Assert.AreEqual(1, tokens[3].Line);
            Assert.IsTrue(tokens[0].IsValid);
            Assert.IsTrue(tokens[1].IsValid);
            Assert.IsTrue(tokens[2].IsValid);
            Assert.IsTrue(tokens[3].IsValid);
            Assert.AreEqual("if", tokens[0].Value);
            Assert.AreEqual("=", tokens[1].Value);
            Assert.AreEqual("//", tokens[2].Value);
            Assert.IsTrue("//this is a comment inline".Equals(tokens[3].Value));
        }

        // Test data: //this\tis a comment inlineif = 
        // Having inline comment before code makes code commented
        [TestMethod]
        public void TestInlineCommentBeforeCodeUsingTokenizeFunctionShouldBeCommented()
        {
            tokens = LexicalAnalyzer.Tokenize(line + "this\tis a comment inline" + "if = ");
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(Lexeme.inlinecmt, tokens[0].Lexeme);
            Assert.AreEqual(Lexeme.inlinecmt, tokens[1].Lexeme);
            Assert.AreEqual(1, tokens[0].Line);
            Assert.AreEqual(1, tokens[1].Line);
            Assert.IsTrue(tokens[0].IsValid);
            Assert.IsTrue(tokens[1].IsValid);
            Assert.AreEqual("//", tokens[0].Value);
            Assert.AreEqual("//this\tis a comment inlineif = ", tokens[1].Value);
        }

        // Test data: //this\tis a\tcomment\tinlineif = \t
        // All tabs are recognized properly and kept in their places in the comment
        [TestMethod]
        public void TestInlineCommentBeforeCodeUsingTokenizeFunctionWithTabsInMiddleAndEnd()
        {
            tokens = LexicalAnalyzer.Tokenize(line + "this\tis a\tcomment\tinline" + "if = \t");
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(Lexeme.inlinecmt, tokens[0].Lexeme);
            Assert.AreEqual(Lexeme.inlinecmt, tokens[1].Lexeme);
            Assert.AreEqual(1, tokens[0].Line);
            Assert.AreEqual(1, tokens[1].Line);
            Assert.IsTrue(tokens[0].IsValid);
            Assert.IsTrue(tokens[1].IsValid);
            Assert.AreEqual("//", tokens[0].Value);
            Assert.AreEqual(line + "this\tis a\tcomment\tinline" + "if = \t", tokens[1].Value);
        }

        // Test data: if//comment\t = \t 
        // if is recognized before the comment start
        [TestMethod]
        public void TestInlineCommentSymbolInMiddleOfWordThatHasCodeAndCommentUsingTokenizeFunction()
        {
            tokens = LexicalAnalyzer.Tokenize("if" + line + "comment\t = \t");
            Assert.AreEqual(3, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].Lexeme);
            Assert.AreEqual(Lexeme.inlinecmt, tokens[1].Lexeme);
            Assert.AreEqual(Lexeme.inlinecmt, tokens[2].Lexeme);
            Assert.AreEqual(1, tokens[0].Line);
            Assert.AreEqual(1, tokens[1].Line);
            Assert.AreEqual(1, tokens[2].Line);
            Assert.IsTrue(tokens[0].IsValid);
            Assert.IsTrue(tokens[1].IsValid);
            Assert.IsTrue(tokens[2].IsValid);
            Assert.AreEqual("if", tokens[0].Value);
            Assert.AreEqual("//", tokens[1].Value);
            Assert.AreEqual(line + "comment\t = \t", tokens[2].Value);
        }

        // Test data: if // comment\t = \t 
        // if is recognized before the comment start
        [TestMethod]
        public void TestInlineCommentSymbolBetweenCodeAndCommentWithASapceInCommentUsingTokenizeFunction()
        {
            tokens = LexicalAnalyzer.Tokenize("if " + line + " comment\t = \t");
            Assert.AreEqual(3, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].Lexeme);
            Assert.AreEqual(Lexeme.inlinecmt, tokens[1].Lexeme);
            Assert.AreEqual(Lexeme.inlinecmt, tokens[2].Lexeme);
            Assert.AreEqual(1, tokens[0].Line);
            Assert.AreEqual(1, tokens[1].Line);
            Assert.AreEqual(1, tokens[2].Line);
            Assert.IsTrue(tokens[0].IsValid);
            Assert.IsTrue(tokens[1].IsValid);
            Assert.IsTrue(tokens[2].IsValid);
            Assert.AreEqual("if", tokens[0].Value);
            Assert.AreEqual("//", tokens[1].Value);
            Assert.AreEqual(line + " comment\t = \t", tokens[2].Value);
        }

        // Test data: if // comm//ent\t = \t 
        // if is recognized before the comment start
        [TestMethod]
        public void TestInlineCommentSymbolBetweenCodeAndCommentWithASapceInCommentWithMultipleCommentSymbolsUsingTokenizeFunction()
        {
            tokens = LexicalAnalyzer.Tokenize("if " + line + " comm//ent\t = \t");
            Assert.AreEqual(3, tokens.Count);
            Assert.AreEqual(Lexeme.keyword, tokens[0].Lexeme);
            Assert.AreEqual(Lexeme.inlinecmt, tokens[1].Lexeme);
            Assert.AreEqual(Lexeme.inlinecmt, tokens[2].Lexeme);
            Assert.AreEqual(1, tokens[0].Line);
            Assert.AreEqual(1, tokens[1].Line);
            Assert.AreEqual(1, tokens[2].Line);
            Assert.IsTrue(tokens[0].IsValid);
            Assert.IsTrue(tokens[1].IsValid);
            Assert.IsTrue(tokens[2].IsValid);
            Assert.AreEqual("if", tokens[0].Value);
            Assert.AreEqual("//", tokens[1].Value);
            Assert.AreEqual(line + " comm//ent\t = \t", tokens[2].Value);
        }

    }
}
