using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TruCompiler;
using TruCompiler.FileManagement;

namespace TruCompilerTests.SemanticAnalyzer
{
    [TestClass]
    public class TestBaseClassDefinition
    {
        public string[] InputFiles { get; set; }
        public string OutputPath { get; set; }
        public string expectedFolder = "..\\..\\..\\Expected\\Test 1\\";
        public string FileName { get; set; }
        public IFile FileWriter { get; set; }
        [TestInitialize]
        public void TestInitialize()
        {
            InputFiles = new string[1];
            InputFiles = new string[1]; InputFiles[0] = "..\\..\\..\\Input\\Test 1\\polynomial.src";
            OutputPath = ".\\Test_1_Results\\";
            FileName = "polynomial.src";
        }

        [TestMethod]
        public void BaseClassNotFoundSemanticTest()
        {
            FileWriter = new StubbedFileWriter();
            if (!Directory.Exists(OutputPath))
            {
                Directory.CreateDirectory(OutputPath);
            }
            Driver d = new Driver(FileWriter, InputFiles, OutputPath);
            d.Compile();
            string symtableResult = "";
            string symtableExpected = "";
            string errorsResult = "";
            string errorsExpected = "";

            OutputPath = @".\Test_1_Results\\";
            symtableResult = FileWriter.Read(OutputPath + FileName + ".outsymboltable");

            errorsResult = FileWriter.Read(OutputPath + FileName + ".outsemanticerrors");

            FileName = FileName.Split('.')[0];
            using (StreamReader reader = new StreamReader(expectedFolder + FileName + ".outsymboltable"))
            {
                symtableExpected = reader.ReadToEnd();
            }
            using (StreamReader reader = new StreamReader(expectedFolder + FileName + ".outsemanticerrors"))
            {
                errorsExpected = reader.ReadToEnd();
            }
            Assert.AreEqual(symtableExpected, symtableResult);
            Assert.AreEqual(errorsExpected, errorsResult);
        }

        [TestMethod]
        public void BaseClassFoundSemanticWithNoErrorsTest()
        {
            FileWriter = new StubbedFileWriter();
            InputFiles = new string[1]; InputFiles[0] = "..\\..\\..\\Input\\Test 1\\polynomial_noerrors.src";
            FileName = "polynomial_noerrors.src";
            if (!Directory.Exists(OutputPath))
            {
                Directory.CreateDirectory(OutputPath);
            }
            Driver d = new Driver(FileWriter, InputFiles, OutputPath);
            d.Compile();
            string symtableResult = "";
            string symtableExpected = "";
            string errorsResult = "";
            string errorsExpected = "";

            OutputPath = @".\Test_1_Results\\";

            symtableResult = FileWriter.Read(OutputPath + FileName + ".outsymboltable");

            errorsResult = FileWriter.Read(OutputPath + FileName + ".outsemanticerrors");

            FileName = FileName.Split('.')[0];
            using (StreamReader reader = new StreamReader(expectedFolder + FileName + ".outsymboltable"))
            {
                symtableExpected = reader.ReadToEnd();
            }
            using (StreamReader reader = new StreamReader(expectedFolder + FileName + ".outsemanticerrors"))
            {
                errorsExpected = reader.ReadToEnd();
            }
            Assert.AreEqual(symtableExpected, symtableResult);
            Assert.AreEqual(errorsExpected, errorsResult);
        }

    }
}
