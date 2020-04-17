using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TruCompiler.CodeGeneration;
using TruCompiler.FileManagement;
using TruCompiler.Lexical_Analyzer;
using TruCompiler.Nodes;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler
{
    /// <summary>
    /// Driver class that is responsible for the main logic of TruCompiler.
    /// The below class includes calling and processing the lexical analyzer
    /// </summary>
    public class Driver
    {
        public string[] InputFiles { get; private set; }
        public string OutputPath { get; private set; }
        public static string[] ASTResult { get; set; }
        public static int ASTIndex { get; set; }
        public static string SemanticErrors { get; set; }
        public static IFile FileWriter { get; set; }
        public static Dictionary<string, string> GeneratedCode {get; set;}
        public static HashSet<string> AddedProcedures { get; set; }
        public static HashSet<string> AddedFunctions { get; set; }
        public Driver(string[] inputFiles, string outputPath)
        {
            this.Initialize(inputFiles, outputPath);
        }

        public Driver(IFile fileWriter, string[] inputFiles, string outputPath)
        {
            this.Initialize(inputFiles, outputPath);
            FileWriter = fileWriter;
        }
        
        public void Initialize(string[] inputFiles, string outputPath)
        {
            InputFiles = inputFiles;
            OutputPath = outputPath;
            FileWriter = new FileWriter();
            GeneratedCode = new Dictionary<string, string>();
            GeneratedCode.Add("Functions", "%- Functions -%\n");
            GeneratedCode.Add("Program", "%- Program -%\n");
            GeneratedCode.Add("Data", "%- Data -%\n");
            GeneratedCode.Add("Procedures", "%- Procedures -%\n");
            AddedProcedures = new HashSet<string>();
            AddedFunctions = new HashSet<string>();
        }
        public void Compile()
        {
            try
            {
                foreach (string file in InputFiles)
                {
                    //Generate Tokens through the lexical analyzer
                    IDictionary<string, IList<Token>> tokens = new Dictionary<string, IList<Token>>();
                    Node<Token> syntaxTree = null;
                    if (File.Exists(file))
                    {
                        tokens.Add(file, LexicalAnalyzer.Tokenize(File.ReadAllLines(file)));

                        //Output to file by default the file location is in the same location as the source file
                        string outlextokens = Tokens.ToString(tokens[file].Where<Token>(t => t != null && t.IsValid));
                        string outlexerrors = Tokens.ToString(tokens[file].Where<Token>(t => t != null && !t.IsValid));

                        if (String.IsNullOrEmpty(OutputPath))
                        {
                            OutputPath = file.Substring(0, file.LastIndexOf("\\"));
                        }

                        if (Directory.Exists(OutputPath))
                        {
                            string outlextokensFile = OutputPath + file.Substring(file.LastIndexOf("\\"), file.LastIndexOf(".") - OutputPath.Length) + ".outlextokens";
                            string outlexerrorsFile = OutputPath + file.Substring(file.LastIndexOf("\\"), file.LastIndexOf(".") - OutputPath.Length) + ".outlexerrors";

                            WriteToFile(outlextokensFile, outlextokens);
                            WriteToFile(outlexerrorsFile, outlexerrors);
                        }   
                    }
                    //Generate Abstract syntax tree through the syntactical analyzer
                    List<Token> nonNullableTokens;
                    foreach(string key in tokens.Keys) {
                        nonNullableTokens = new List<Token>();
                        nonNullableTokens = tokens[key].Where(t => t != null 
                        && t.Lexeme != Lexeme.inlinecmt
                        && t.Lexeme != Lexeme.blockcmt
                        && t.Lexeme != Lexeme.closecmt
                        && t.Lexeme != Lexeme.opencmt).ToList();
                        TokenScanner tokenScanner = new TokenScanner(nonNullableTokens);
                        syntaxTree = SyntacticalAnalyzer.AnalyzeSyntax(tokenScanner);
                    }
                    Node<Token> newSyntaxTree = new Node<Token>();
                    SyntacticalAnalyzer.CleanEmptyChildren(syntaxTree, ref newSyntaxTree);
                    string result = "digraph name {\n";
                    int index = 0;
                    string[] arr = SyntacticalAnalyzer.GenerateDiGraph(newSyntaxTree, ref index);
                    result += arr[0];
                    result += arr[1];
                    result += "}";

                    //string derivation = Tokens.ToString(syntaxTree.Flatten().ToList());

                    if (String.IsNullOrEmpty(OutputPath))
                    {
                        OutputPath = file.Substring(0, file.LastIndexOf("\\"));
                    }

                    if (Directory.Exists(OutputPath))
                    {
                        string outastFile = OutputPath + file.Substring(file.LastIndexOf("\\"), file.LastIndexOf(".") - OutputPath.Length) + ".outast";
                        //string outderivation = OutputPath + file.Substring(file.LastIndexOf("\\"), file.LastIndexOf(".") - OutputPath.Length) + ".outderivation";
                        
                        WriteToFile(outastFile, result);
                        //WriteToFile(outderivation, derivation);
                    }

                    //TODO: ADD OUTDERIVATIONERRORS AND OUTDERIVATION BEFORE FINAL PROJECT

                    //Semantic Analyzer
                    StartNode startNode = new StartNode(null, newSyntaxTree);
                    ASTVisitor aSTVisitor = new ASTVisitor();
                    string r = "digraph name {\n";
                    ASTIndex = 0;
                    ASTResult = new string[2];
                    startNode.accept(aSTVisitor);
                    r += ASTResult[0];
                    r += ASTResult[1];
                    r += "}";

                    if (String.IsNullOrEmpty(OutputPath))
                    {
                        OutputPath = file.Substring(0, file.LastIndexOf("\\"));
                    }

                    if (Directory.Exists(OutputPath))
                    {
                        string outastFile = OutputPath + file.Substring(file.LastIndexOf("\\"), file.LastIndexOf(".") - OutputPath.Length) + "_new" + ".outast";

                        WriteToFile(outastFile, r);
                    }

                    //Generate symbol table from AST first run
                    if (String.IsNullOrEmpty(OutputPath))
                    {
                        OutputPath = file.Substring(0, file.LastIndexOf("\\"));
                    }
                    string symTablePath = "";
                    if (Directory.Exists(OutputPath))
                    {
                        symTablePath = OutputPath + file.Substring(file.LastIndexOf("\\"), file.LastIndexOf(".") - OutputPath.Length) + ".outsymboltable";
                    }

                    SemanticErrors = "";
                    SymbolTableVisitor symbolTableVisitor = new SymbolTableVisitor(symTablePath);
                    startNode.accept(symbolTableVisitor);
                    TypeCheckingVisitor typeCheckingVisitor = new TypeCheckingVisitor();
                    startNode.accept(typeCheckingVisitor);

                    
                    if (Directory.Exists(OutputPath))
                    {
                        string outsemanticerros = OutputPath + file.Substring(file.LastIndexOf("\\"), file.LastIndexOf(".") - OutputPath.Length) + ".outsemanticerrors";

                        WriteToFile(outsemanticerros, SemanticErrors);
                    }

                    //CodeGeneration
                    CodeGenerationVisitor codeGenVisitor = new CodeGenerationVisitor();
                    startNode.accept(codeGenVisitor);

                    string generatedCodeFile = "";
                    foreach(var key in GeneratedCode.Keys)
                    {
                        generatedCodeFile += GeneratedCode[key] + "\n";
                    }
                    generatedCodeFile = generatedCodeFile.Trim('\n');
                    if (Directory.Exists(OutputPath))
                    {
                        string outCodeGen = OutputPath + file.Substring(file.LastIndexOf("\\"), file.LastIndexOf(".") - OutputPath.Length) + ".m";

                        WriteToFile(outCodeGen, generatedCodeFile);
                    }



                }
            } catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
        public static void WriteToFile(string filePath, string input)
        {
            FileWriter.Write(filePath, input);
        }
    }
}

