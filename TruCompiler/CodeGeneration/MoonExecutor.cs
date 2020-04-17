using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace TruCompiler.CodeGeneration
{

    public class MoonExecutor
    {
        public string InputFile { get; set; }
        public List<string> Output { get; set; }
        public List<string> UsefulOutput { get; set; }
        public const string MOON_EXE = ".\\CodeGeneration\\lib\\moon.exe";
        
        public MoonExecutor(string inputFile)
        {
            InputFile = inputFile;
            Output = new List<string>();
            UsefulOutput = new List<string>();
        }

        public void Execute()
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo()
            {
                CreateNoWindow = true,
                FileName = "C:\\Windows\\System32\\cmd.exe",
                RedirectStandardOutput = true,
                Arguments = "/c " + MOON_EXE + " " + InputFile,
                WorkingDirectory = Directory.GetCurrentDirectory()
            };
            p.Start();
            p.WaitForExit();
            StreamReader r = p.StandardOutput;
            while (!r.EndOfStream)
            {
                Output.Add(r.ReadLine());
            }

            int count = 0;
            foreach(var line in Output)
            {
                if (count == 0 || count == Output.Count -1 || String.IsNullOrEmpty(line))
                {
                    count++;
                    continue;
                } else
                {
                    UsefulOutput.Add(line);
                }
                count++;
            }
            
        }
    }
}
