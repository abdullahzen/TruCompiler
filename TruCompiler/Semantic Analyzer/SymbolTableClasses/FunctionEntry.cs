using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Nodes;

namespace TruCompiler.Semantic_Analyzer.SymbolTableClasses
{
    public class FunctionEntry : Entry
    {
        public List<ParamNode> Params { get; set; }
        public string Visibility { get; set; }

        public FunctionEntry(string type, string name, List<ParamNode> paramaters, SymbolTable table) : base("function", type, name, table)
        {
            Params = paramaters;
            foreach (ParamNode p in Params)
            {
                List<int> dims = new List<int>();
                if (p.ArraySize != null)
                {
                    foreach (ArraySizeNode arr in p.ArraySize)
                    {
                        if (arr.ArraySizeValue != null)
                        {
                            dims.Add(arr.ArraySizeValue);
                        }
                    }
                }
                SymbolTable classType = null;
                if (p.Type.Value.Lexeme == Lexical_Analyzer.Tokens.Lexeme.id)
                {
                    classType = table.SearchName(p.Type.Value.Value).SubTable;
                }
                p.Entry = new VariableEntry("parameter", p.Type.Type, p.Name.IdValue, dims, classType, name);
                SubTable.addEntry(p.Entry);
            }
            if (type == "void")
            {
                Size = 0;
            }
        }

        public FunctionEntry(string visibility, string type, string name, List<ParamNode> paramaters, SymbolTable table) : base("function", type, name, table)
        {
            Params = paramaters;
            Visibility = visibility;
            foreach(ParamNode p in Params)
            {
                List<int> dims = new List<int>();
                if (p.ArraySize != null)
                {
                    foreach (ArraySizeNode arr in p.ArraySize)
                    {
                        if (arr.ArraySizeValue != null)
                        {
                            dims.Add(arr.ArraySizeValue);
                        }
                    }
                }
                SymbolTable classType = null;
                if (p.Type.Value.Lexeme == Lexical_Analyzer.Tokens.Lexeme.id)
                {
                    classType = table.SearchName(p.Type.Value.Value).SubTable;
                }
                p.Entry = new VariableEntry("parameter", p.Type.Type, p.Name.IdValue, dims, classType);
                SubTable.addEntry(p.Entry);
            }
            if (type == "void")
            {
                Size = 0;
            }
        }

        public override string ToString()
        {
            return
                String.Format("{0,-18}", "| " + Visibility) +
                String.Format("{0,-12}", "| " + Kind) +
                String.Format("{0,-4}", "| " + Name) +
                GetParamsString() +
                String.Format("{0,-22}", "| " + Tag) +
                String.Format("{0,-12}", "| " + Type) +
                String.Format("{0,-8}", "| " + Size) +
                String.Format("{0,-8}", "| " + Notes) +
                "|" +
                SubTable;
        }

        public string GetParamsString()
        {
            string result = "";
            if (Params != null && Params.Count > 0)
            {
                Params.ForEach(p =>
                {
                    result += p.Type.Value.Value + ",";
                });
                result = result.TrimEnd(',');
            }

            result = String.Format("{0,-4}", " (" + result + ")  ");
            return result;
        }
    }
}
