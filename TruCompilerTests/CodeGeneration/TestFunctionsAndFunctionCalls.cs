using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TruCompiler;
using TruCompiler.CodeGeneration;
using TruCompiler.FileManagement;

namespace TruCompilerTests.CodeGeneration
{
    [TestClass]
    public class TestFunctionsAndFunctionCalls
    {
        public string[] InputFiles { get; set; }
        public string OutputPath { get; set; }
        public string expectedFolder = "..\\..\\..\\Expected\\Test 8\\";
        public string FileName { get; set; }
        public IFile FileWriter { get; set; }
        public MoonExecutor moon { get; set; }
        [TestInitialize]
        public void TestInitialize()
        {
            InputFiles = new string[1];
            InputFiles[0] = "..\\..\\..\\Input\\Test 8\\polynomial.src";
            OutputPath = ".\\Test_8_Results\\";
            FileName = "polynomial.src";
            FileWriter = new StubbedFileWriter();
        }


        [TestMethod]
        public void ComplexArithmeticAndReadAndWriteTest()
        {
            if (!Directory.Exists(OutputPath))
            {
                Directory.CreateDirectory(OutputPath);
            }
            Driver d = new Driver(FileWriter, InputFiles, OutputPath);
            d.Compile();
            string generatedCode = "";

            OutputPath = @".\Test_8_Results\\";
            string codeFile = OutputPath + FileName + ".m";
            generatedCode = FileWriter.Read(codeFile);
            File.WriteAllText(codeFile, generatedCode);

            moon = new MoonExecutor(codeFile, true);
            moon.Execute();

            Assert.AreEqual(1, moon.UsefulOutput.Count);
            Assert.AreEqual("6", moon.UsefulOutput[0]);

        }

    }
}
