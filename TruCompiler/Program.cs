﻿using System;
using System.Collections.Generic;

namespace TruCompiler
{
    /// <summary>
    /// Main program class to initialize the passed arguments and run the driver.
    /// </summary>
    class Program
    {
        private Dictionary<string, string[]> parsedArgs = new Dictionary<string, string[]>();
        public Dictionary<string, string[]> ParsedArgs { get => parsedArgs; set => parsedArgs = value; }
        private Program(string[] args)
        {
            try
            {
                for (int i = 0; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        case "-input":
                            ParsedArgs.Add("inputFiles", args[i + 1].Split(','));
                            i++;
                            break;
                       /* case "-output":
                            ParsedArgs.Add("outputPath", args[i + 1].Split(','));
                            i++;
                            break;*/
                    }
                }
            } catch (Exception)
            {
                throw new ArgumentException();
            } finally
            {
                if (ParsedArgs.Count == 0)
                {
                    throw new ArgumentException();
                }
            }
        }


        public static void Main(string[] args)
        {
            try
            {
                new Program(args).Run();
            } catch (ArgumentException e)
            {
                Console.WriteLine("Error occurred when parsing the passed arguemnts.\n\n" +
                    "=====================================================================\n" +
                    "Please follow the following usage:\n" +
                    "dotnet run -input \"filesPathsToCompileSeparatedByCommas\"\r\n"
                    + "=====================================================================\n");
                Console.WriteLine(e.StackTrace);
            }
        }

        private void Run()
        {
            //Call driver with parsed args
            Driver driver = new Driver(ParsedArgs["inputFiles"], "");
            driver.Compile();
        }
    }
}
