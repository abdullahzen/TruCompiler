using System;
using System.Collections.Generic;
using System.Text;

namespace TruCompiler.Semantic_Analyzer.SymbolTableClasses
{
    public class VariableEntry : Entry
    {
        public string Visibility { get; set; }
        public SymbolTable ClassType { get; set; }
        public VariableEntry(string visibility, string kind, string type, string name, List<int> dims) : base(kind, type, name, null)
        {
            Dims = dims;
            Visibility = visibility;
        }

        public VariableEntry(string kind, string type, string name, List<int> dims) : base(kind, type, name, null)
        {
            Dims = dims;
        }

        public override string ToString()
        {
            return
                String.Format("{0,-12}", "| " + Visibility) +
                String.Format("{0,-12}", "| " + Kind) +
                String.Format("{0,-12}", "| " + Name) +
                String.Format("{0,-12}", "| " + Type) +
                /*String.Format("{0,-8}", "| " + Size) +
                String.Format("{0,-2}", "| " + Offset) +*/
                (ClassType != null ? String.Format("{0,-6}", "| linked to " + ClassType.Name) : "")
                + "|";
        }
    }
}
