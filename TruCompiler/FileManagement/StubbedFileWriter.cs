using System;
using System.Collections.Generic;
using System.Text;

namespace TruCompiler.FileManagement
{
    public class StubbedFileWriter : IFile
    {
        public Dictionary<string, string> FileContent { get; set; }

        public StubbedFileWriter()
        {
            FileContent = new Dictionary<string, string>();
        }
        public string Read(string ReadingPath)
        {
            FileContent.TryGetValue(ReadingPath, out string result);
            return result;
        }

        public void Write(string OutputPath, string content)
        {
            FileContent.Add(OutputPath, content);
        }
    }
}
