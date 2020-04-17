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
    public class TestSimpleArithmeticAndWrite
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
            InputFiles[0] = "..\\..\\..\\Input\\Test 5\\polynomial.src";
            OutputPath = ".\\Test_5_Results\\";
            FileName = "polynomial.src";
            FileWriter = new StubbedFileWriter();
        }

        [TestMethod]
        public void SimpleAddAndWriteTest()
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
            Assert.AreEqual("15", moon.UsefulOutput[0]);
        }

        [TestMethod]
        public void CompoundAddAndWriteWithAddTest()
        {
            InputFiles[0] = "..\\..\\..\\Input\\Test 5\\polynomial2.src";
            FileName = "polynomial2.src";
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
            Assert.AreEqual("22", moon.UsefulOutput[0]);
        }

        [TestMethod]
        public void CompoundAddAndSubtractAndWriteWithAddSubTest()
        {
            InputFiles[0] = "..\\..\\..\\Input\\Test 5\\polynomial3.src";
            FileName = "polynomial3.src";
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
            Assert.AreEqual("14", moon.UsefulOutput[0]);
        }

    }
}
