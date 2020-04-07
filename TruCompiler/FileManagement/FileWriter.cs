using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TruCompiler.FileManagement
{
    public class FileWriter : IFile
    {
        public StreamWriter streamWriter { get; set; }
        public FileWriter()
        {
        }

        public void Write(string OutputPath, string content)
        {
            using (StreamWriter writer = new StreamWriter(OutputPath, false))
            {
                writer.Write(content);
            }
        }

        public string Read(string ReadingPath)
        {
            return "";
        }
    }
}
