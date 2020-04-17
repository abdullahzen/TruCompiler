using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TruCompiler.Nodes;
using TruCompiler.Semantic_Analyzer.SymbolTableClasses;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.CodeGeneration
{
    public class CodeGenerationHelper
    {
        public static Dictionary<string, bool> AvailableRegisters = new Dictionary<string, bool>
        { 
            { "r2",true }, 
            { "r3",true }, 
            { "r4",true }, 
            { "r5",true }, 
            { "r6",true }, 
            { "r7",true }, 
            { "r8",true }, 
            { "r9",true }, 
            { "r10",true }, 
            { "r11",true }, 
            { "r12",true }, 
            { "r13",true }, 
            { "r14",true }, 
        };
        public static string ReserveData(Entry entry)
        {
            return String.Format("{0,-8} res {1}\n", entry.Tag, entry.Size);
        }

        public static string StoreLiteralValue(Entry entry)
        {
            string r = GetNextAvailableRegister();
            int.TryParse(entry.Notes, out int val);
            string result =  String.Format("%- Storing literal value {2} for later use -%\n" +
                "{0,-8}addi {1},R0,{2}\n" + 
                "{3,-8}sw {4}(R0),{1}\n\n", 
                "", r, val.ToString(), 
                "", entry.Tag);
            AvailableRegisters[r] = true;
            return result;
        }

        public static string AssignSides(Entry left, Entry right)
        {
            string r = GetNextAvailableRegister();
            while (String.IsNullOrEmpty(left.Tag))
            {
                left = left.SubTable.SymList[0];
            }
            while (String.IsNullOrEmpty(right.Tag))
            {
                right = right.SubTable.SymList[0];
            }

            string result = String.Format("%- Assigning a variable -%\n" +
                "{0,-8}lw {1},{2}(R0)\n" +
                "{3,-8}sw {4}(R0),{1}\n\n",
                "", r, right.Tag,
                "", left.Tag);
            AvailableRegisters[r] = true;
            return result;
        }

        public static string AddOp(Entry left, Entry right, string op, Entry temp)
        {
            string r2 = GetNextAvailableRegister();
            string r3 = GetNextAvailableRegister();
            string operation = op == "+" ? "add" : "sub";
            string result = String.Format("%- Arithmetic operation ({0}) -%\n" +
                "{1,-8}lw {2},{3}(R0)\n" +
                "{4,-8}lw {5},{6}(R0)\n" +
                "{7,-8}{8} {2},{2},{5}\n" +
                "{9,-8}sw {10}(R0),{2}\n\n", 
                op,
                "", r2, left.Tag,
                "", r3, right.Tag,
                "", operation,
                "", temp.Tag);
            AvailableRegisters[r2] = true;
            AvailableRegisters[r3] = true;
            return result;
        }

        public static string RelExpr(Entry left, Entry right, string op, Entry temp)
        {
            string r2 = GetNextAvailableRegister();
            string r3 = GetNextAvailableRegister();
            string operation = "";
            switch (op)
            {
                case "and":
                    operation = "and";
                    break;
                case "or":
                    operation = "or";
                    break;
                case "not":
                    operation = "not";
                    break;
                case "==":
                    operation = "ceq";
                    break;
                case "<>":
                    operation = "cne";
                    break;
                case "<":
                    operation = "clt";
                    break;
                case "<=":
                    operation = "cle";
                    break;
                case ">":
                    operation = "cgt";
                    break;
                case ">=":
                    operation = "cge";
                    break;
            }

            string result = String.Format("%- Relational Expression operation ({0}) -%\n" +
                "{1,-8}lw {2},{3}(R0)\n" +
                "{4,-8}lw {5},{6}(R0)\n" +
                "{7,-8}{8} {2},{2},{5}\n" +
                "{9,-8}sw {10}(R0),{2}\n\n",
                op,
                "", r2, left.Tag,
                "", r3, right.Tag,
                "", operation,
                "", temp.Tag);
            AvailableRegisters[r2] = true;
            AvailableRegisters[r3] = true;
            return result;
        }

        public static string BranchIf(Entry tempRelExpr, string endthen)
        {
            string r2 = GetNextAvailableRegister();
            string result = String.Format("%- If condition check and branch to then or else -%\n" +
                "{0,-8}lw {1},{2}(R0)\n" +
                "{3,-8}bz {1},{4}\n\n",
                "", r2, tempRelExpr.Tag,
                "", endthen);
            AvailableRegisters[r2] = true;
            return result;
        }

        public static string BranchWhile(Entry tempRelExpr, string endwhile)
        {
            string r2 = GetNextAvailableRegister();
            string result = String.Format("%- While condition check if true continue in loop else jump to end -%\n" +
                "{0,-8}lw {1},{2}(R0)\n" +
                "{3,-8}bz {1},{4}\n\n",
                "", r2, tempRelExpr.Tag,
                "", endwhile);
            AvailableRegisters[r2] = true;
            return result;
        }
        
        public static string ReturnFrom(Entry returnValue)
        {
            string result = String.Format("%- Returning from function -%\n" +
                "{0,-8}lw R1,{1}(R0)\n",
                "", returnValue.Tag);

            return result;
        }
        
        public static string FunctionCall(List<ExprNode> expresions, string name, Entry returnVal, FunctionCallNode node)
        {
            string r = GetNextAvailableRegister();
            FunctionEntry linkedFuncEntry = null;
            string result = "";
            Node<Token> temp = node;
            while (temp.Parent != null)
            {
                temp = temp.Parent;
            }
            FuncDefsNode funcs = ((ProgNode)temp[0]).FunctionDefinitions;

            if (funcs != null && funcs.Children.Count > 0)
            {
                FuncDefNode func = (FuncDefNode)funcs.Children.Find(f => (f.Entry != null) ? f.Entry.Name == node.Name.IdValue : false);

                if (func != null && func.Entry != null)
                {
                    linkedFuncEntry = (FunctionEntry)func.Entry;
                }
            }

            for (int i = 0; i < expresions.Count; i++)
            {
                result += "%- Passing function params -%\n";
                if (expresions[i].Children.Count > 0)
                {
                    Node<Token> entry = expresions[i];
                    while(entry.Entry == null)
                    {
                        if (entry.Children.Count > 0)
                        {
                            entry = entry[0];
                        } else
                        {
                            break;
                        }
                    }

                    result += String.Format(
                            "{0,-8}lw {1},{2}(R0)\n" +
                            "{3,-8}sw {4}(R0),{1}\n\n",
                            "", r, entry.Entry.Tag,
                            "", linkedFuncEntry.Params[i].Entry.Tag);
                }
                
            }
            result += String.Format("%- Calling function -%\n" +
                "{0,-8}jl R15,{1}\n" + 
                "{2,-8}sw {3}(R0),R1\n\n",
                "", linkedFuncEntry.Tag,
                "", returnVal.Tag);
            AvailableRegisters[r] = true;
            return result;
        }

        public static string MultOp(Entry left, Entry right, string op, Entry temp)
        {
            string r2 = GetNextAvailableRegister();
            string r3 = GetNextAvailableRegister();
            string operation = op == "*" ? "mul" : "div";
            string result = String.Format("%- Arithmetic operation ({0}) -%\n" +
                "{1,-8}lw {2},{3}(R0)\n" +
                "{4,-8}lw {5},{6}(R0)\n" +
                "{7,-8}{8} {2},{2},{5}\n" +
                "{9,-8}sw {10}(R0),{2}\n\n",
                op,
                "", r2, left.Tag,
                "", r3, right.Tag,
                "", operation,
                "", temp.Tag);
            AvailableRegisters[r2] = true;
            AvailableRegisters[r3] = true;
            return result;
        }


        public static string WriteToConsole(Entry entry)
        {
            string procedure = "";
            if (entry.Type == "integer")
            {
                AddPutIntProcedure();
                procedure = "putint";
            }
            AddNewLineProcedure();
            string result = String.Format("%- Writing to console -%\n" +
                "{0,-8}lw R1,{1}(R0)\n" +
                "{2,-8}jl R15,{3}\n" +
                "{4,-8}jl R15,newlineWin\n\n",
                "", entry.Tag,
                "", procedure, 
                "");
            return result;
        }

        public static string ReadFromConsole(Entry entry)
        {
            string procedure = "";
            if (entry.Type == "integer")
            {
                AddReadIntProcedure();
                procedure = "getint";
            }
            string result = String.Format("%- Reading from console (key input) -%\n" +
                "{0,-8}jl R15,{1}\n" +
                "{2,-8}sw {3}(R0),R1\n\n",
                "", procedure,
                "", entry.Tag);
            return result;
        }

        public static string GetNextAvailableRegister()
        {
            foreach(var key in AvailableRegisters.Keys)
            {
                if (AvailableRegisters[key])
                {
                    AvailableRegisters[key] = false;
                    return key;
                }
            }
            return "";
        }

        public static void AddPutIntProcedure()
        {
            if (!Driver.AddedProcedures.Contains("PutInt"))
            {
                Driver.GeneratedCode["Procedures"] += File.ReadAllText(".\\CodeGeneration\\lib\\PutInt.m") + "\n";
                Driver.AddedProcedures.Add("PutInt");
            }
        }

        public static void AddReadIntProcedure()
        {
            if (!Driver.AddedProcedures.Contains("GetInt"))
            {
                Driver.GeneratedCode["Procedures"] += File.ReadAllText(".\\CodeGeneration\\lib\\GetInt.m") + "\n";
                Driver.AddedProcedures.Add("GetInt");
            }
        }

        public static void AddNewLineProcedure()
        {
            if (!Driver.AddedProcedures.Contains("newline"))
            {
                Driver.GeneratedCode["Procedures"] += File.ReadAllText(".\\CodeGeneration\\lib\\newline.m") + "\n";
                Driver.AddedProcedures.Add("newline");
            }
        }
    }
}
