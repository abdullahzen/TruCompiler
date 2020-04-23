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
    public class TestComplexArithmeticWithWriteAndComputeAllSizes
    {
        public string[] InputFiles { get; set; }
        public string OutputPath { get; set; }
        public string expectedFolder = "..\\..\\..\\Expected\\Test 6\\";
        public string FileName { get; set; }
        public IFile FileWriter { get; set; }
        public MoonExecutor moon { get; set; }
        [TestInitialize]
        public void TestInitialize()
        {
            InputFiles = new string[1];
            InputFiles = new string[1]; InputFiles[0] = "..\\..\\..\\Input\\Test 6\\polynomial.src";
            OutputPath = ".\\Test_6_Results\\";
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

            OutputPath = @".\Test_6_Results\\";
            string codeFile = OutputPath + FileName + "_stack.m";
            generatedCode = FileWriter.Read(codeFile);
            File.WriteAllText(codeFile, generatedCode);

            moon = new MoonExecutor(codeFile, true);
            moon.Execute();

            Assert.AreEqual(5, moon.UsefulOutput.Count);
            Assert.AreEqual("-3", moon.UsefulOutput[0]);
            Assert.AreEqual("99", moon.UsefulOutput[1]);
            Assert.AreEqual("79", moon.UsefulOutput[2]);
            Assert.AreEqual("890", moon.UsefulOutput[3]);
            Assert.AreEqual("8", moon.UsefulOutput[4]);

        }

    }
}
