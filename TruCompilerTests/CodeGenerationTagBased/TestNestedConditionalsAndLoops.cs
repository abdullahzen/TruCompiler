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
    public class TestNestedConditionalsAndLoops
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
            InputFiles = new string[1]; InputFiles[0] = "..\\..\\..\\Input\\Test 7\\polynomial.src";
            OutputPath = ".\\Test_7_Results\\";
            FileName = "polynomial.src";
        }


        [TestMethod]
        public void ComplexNestedConditionalsAndLoopsTest()
        {
            FileWriter = new StubbedFileWriter();
            if (!Directory.Exists(OutputPath))
            {
                Directory.CreateDirectory(OutputPath);
            }
            Driver d = new Driver(FileWriter, InputFiles, OutputPath);
            d.Compile();
            string generatedCode = "";

            OutputPath = @".\Test_7_Results\\";
            string codeFile = OutputPath + FileName + "_tag.m";
            generatedCode = FileWriter.Read(codeFile);
            File.WriteAllText(codeFile, generatedCode);

            moon = new MoonExecutor(codeFile, true);
            moon.Execute();

            Assert.AreEqual(45, moon.UsefulOutput.Count);
            Assert.AreEqual("-3", moon.UsefulOutput[0]);
            Assert.AreEqual("31", moon.UsefulOutput[1]);
            Assert.AreEqual("-1", moon.UsefulOutput[2]);
            Assert.AreEqual("240", moon.UsefulOutput[3]);
            Assert.AreEqual("0", moon.UsefulOutput[4]);
            Assert.AreEqual("16", moon.UsefulOutput[5]);
            Assert.AreEqual("0", moon.UsefulOutput[6]);
            Assert.AreEqual("11", moon.UsefulOutput[7]);
            Assert.AreEqual("1111", moon.UsefulOutput[8]);
            Assert.AreEqual("1", moon.UsefulOutput[9]);
            Assert.AreEqual("11", moon.UsefulOutput[10]);
            Assert.AreEqual("1111", moon.UsefulOutput[11]);
            Assert.AreEqual("2", moon.UsefulOutput[12]);
            Assert.AreEqual("11", moon.UsefulOutput[13]);
            Assert.AreEqual("1111", moon.UsefulOutput[14]);
            Assert.AreEqual("3", moon.UsefulOutput[15]);
            Assert.AreEqual("11", moon.UsefulOutput[16]);
            Assert.AreEqual("1111", moon.UsefulOutput[17]);
            Assert.AreEqual("4", moon.UsefulOutput[18]);
            Assert.AreEqual("11", moon.UsefulOutput[19]);
            Assert.AreEqual("1111", moon.UsefulOutput[20]);
            Assert.AreEqual("5", moon.UsefulOutput[21]);
            Assert.AreEqual("11", moon.UsefulOutput[22]);
            Assert.AreEqual("1111", moon.UsefulOutput[23]);
            Assert.AreEqual("6", moon.UsefulOutput[24]);
            Assert.AreEqual("11", moon.UsefulOutput[25]);
            Assert.AreEqual("15", moon.UsefulOutput[26]);
            Assert.AreEqual("14", moon.UsefulOutput[27]);
            Assert.AreEqual("13", moon.UsefulOutput[28]);
            Assert.AreEqual("12", moon.UsefulOutput[29]);
            Assert.AreEqual("11", moon.UsefulOutput[30]);
            Assert.AreEqual("10", moon.UsefulOutput[31]);
            Assert.AreEqual("9", moon.UsefulOutput[32]);
            Assert.AreEqual("8", moon.UsefulOutput[33]);
            Assert.AreEqual("7", moon.UsefulOutput[34]);
            Assert.AreEqual("6", moon.UsefulOutput[35]);
            Assert.AreEqual("5", moon.UsefulOutput[36]);
            Assert.AreEqual("4", moon.UsefulOutput[37]);
            Assert.AreEqual("3", moon.UsefulOutput[38]);
            Assert.AreEqual("2", moon.UsefulOutput[39]);
            Assert.AreEqual("1", moon.UsefulOutput[40]);
            Assert.AreEqual("8", moon.UsefulOutput[41]);
            Assert.AreEqual("4444", moon.UsefulOutput[42]);
            Assert.AreEqual("9", moon.UsefulOutput[43]);
            Assert.AreEqual("4444", moon.UsefulOutput[44]);

        }

    }
}
