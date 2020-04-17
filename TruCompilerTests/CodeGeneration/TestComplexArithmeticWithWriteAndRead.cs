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
    public class TestComplexArithmeticWithWriteAndRead
    {
        public string[] InputFiles { get; set; }
        public string OutputPath { get; set; }
        public string expectedFolder = "..\\..\\..\\Expected\\Test 5\\";
        public string FileName { get; set; }
        public IFile FileWriter { get; set; }
        public MoonExecutor moon { get; set; }
        [TestInitialize]
        public void TestInitialize()
        {
            InputFiles = new string[1];
            InputFiles[0] = "..\\..\\..\\Input\\Test 5\\polynomial4.src";
            OutputPath = ".\\Test_5_Results\\";
            FileName = "polynomial4.src";
            FileWriter = new StubbedFileWriter();
        }

        [TestMethod]
        public void ComplexArithAndWriteTest()
        {
            if (!Directory.Exists(OutputPath))
            {
                Directory.CreateDirectory(OutputPath);
            }
            Driver d = new Driver(FileWriter, InputFiles, OutputPath);
            d.Compile();
            string generatedCode = "";

            OutputPath = @".\Test_5_Results\\";
            string codeFile = OutputPath + FileName + ".m";
            generatedCode = FileWriter.Read(codeFile);
            File.WriteAllText(codeFile, generatedCode);

            moon = new MoonExecutor(codeFile);
            moon.Execute();

            Assert.AreEqual(1, moon.UsefulOutput.Count);
            Assert.AreEqual("-3", moon.UsefulOutput[0]);
        }

        /*[TestMethod]
        public void ComplexArithmeticAndReadAndWriteTest()
        {
            InputFiles[0] = "..\\..\\..\\Input\\Test 5\\polynomial5.src";
            FileName = "polynomial5.src";
            if (!Directory.Exists(OutputPath))
            {
                Directory.CreateDirectory(OutputPath);
            }
            Driver d = new Driver(FileWriter, InputFiles, OutputPath);
            d.Compile();
            string generatedCode = "";

            OutputPath = @".\Test_5_Results\\";
            string codeFile = OutputPath + FileName + ".m";
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

        }*/

    }
}
