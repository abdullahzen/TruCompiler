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
                SubTable.addEntry(new VariableEntry("parameter", p.Type.Type, p.Name.IdValue, null));
            }
        }

        public FunctionEntry(string visibility, string type, string name, List<ParamNode> paramaters, SymbolTable table) : base("function", type, name, table)
        {
            Params = paramaters;
            Visibility = visibility;
            foreach(ParamNode p in Params)
            {
                SubTable.addEntry(new VariableEntry("parameter", p.Type.Type, p.Name.IdValue, null));
            }
        }

        public override string ToString()
        {
            return
                String.Format("{0,-12}", "| " + Visibility) +
                String.Format("{0,-12}", "| " + Kind) +
                String.Format("{0,-12}", "| " + Name) +
                GetParamsString() +
                String.Format("{0,-14}", "| " + Type) +
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

            result = String.Format("{0,-12}", "| (" + result + ")  ");
            return result;
        }
    }
}
