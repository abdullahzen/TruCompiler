using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TruCompiler;
using TruCompiler.CodeGeneration;
using TruCompiler.FileManagement;

namespace TruCompilerTests.CodeGenerationStackBased
{
    [TestClass]
    public class TestClassMembersClassFunctionCalls
    {
        public string[] InputFiles { get; set; }
        public string OutputPath { get; set; }
        public string expectedFolder = "..\\..\\..\\Expected\\Test 9\\";
        public string FileName { get; set; }
        public IFile FileWriter { get; set; }
        public MoonExecutor moon { get; set; }
        [TestInitialize]
        public void TestInitialize()
        {
            InputFiles = new string[1];
            InputFiles = new string[1]; InputFiles[0] = "..\\..\\..\\Input\\Test 9\\polynomial.src";
            OutputPath = ".\\Test_9_Results\\";
            FileName = "polynomial.src";
        }


        [TestMethod]
        public void ComplexArithmeticAndReadAndWriteTest()
        {
            FileWriter = new StubbedFileWriter();
            if (!Directory.Exists(OutputPath))
            {
                Directory.CreateDirectory(OutputPath);
            }
            Driver d = new Driver(FileWriter, InputFiles, OutputPath);
            d.Compile();
            string generatedCode = "";

            OutputPath = @".\Test_9_Results\\";
            string codeFile = OutputPath + FileName + "_stack.m";
            generatedCode = FileWriter.Read(codeFile);
            File.WriteAllText(codeFile, generatedCode);

            moon = new MoonExecutor(codeFile, true);
            moon.Execute();

            Assert.AreEqual(13, moon.UsefulOutput.Count);
            Assert.AreEqual("1", moon.UsefulOutput[0]);
            Assert.AreEqual("2", moon.UsefulOutput[1]);
            Assert.AreEqual("4", moon.UsefulOutput[2]);
            Assert.AreEqual("6", moon.UsefulOutput[3]);
            Assert.AreEqual("8", moon.UsefulOutput[4]);
            Assert.AreEqual("10", moon.UsefulOutput[5]);
            Assert.AreEqual("12", moon.UsefulOutput[6]);
            Assert.AreEqual("14", moon.UsefulOutput[7]);
            Assert.AreEqual("16", moon.UsefulOutput[8]);
            Assert.AreEqual("18", moon.UsefulOutput[9]);
            Assert.AreEqual("20", moon.UsefulOutput[10]);
            Assert.AreEqual("16", moon.UsefulOutput[11]);
            Assert.AreEqual("16", moon.UsefulOutput[12]);

        }

    }
}
