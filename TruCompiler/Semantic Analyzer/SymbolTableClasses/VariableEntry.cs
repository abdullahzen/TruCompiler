using System;
using System.Collections.Generic;
using System.Text;

namespace TruCompiler.Semantic_Analyzer.SymbolTableClasses
{
    public class VariableEntry : Entry
    {
        public string Visibility { get; set; }
        public SymbolTable ClassType { get; set; }
        public string ClassName { get; set; }
        public VariableEntry(string visibility, string kind, string type, string name, List<int> dims, string className, SymbolTable classType) : base(kind, type, name, null)
        {
            Dims = dims;
            Visibility = visibility;
            ClassType = classType;
            if (Kind == "local" || Kind == "variable")
            {
                if (visibility != null && visibility != "Variable" && visibility != "") 
                {
                    ClassName = className;
                    Tag = "member_" + ClassName + "_var_" + Name;
                } else
                {
                    Tag = "var_" + Name;
                }
            }
            if (ClassType != null && (Dims == null || Dims.Count == 0))
            {
                Size = ClassType.Size;
            }
            else if (ClassType != null && Dims != null && Dims.Count > 0)
            {
                Size = ClassType.Size;
                Size = GetSize();
            } else
            {
                Size = GetSize();
            }
        }

        public VariableEntry(string kind, string type, string name, List<int> dims, SymbolTable classType) : base(kind, type, name, null)
        {
            Dims = dims;
            ClassType = classType;
            if (Kind == "local" || Kind == "variable")
            {
                Tag = "var_" + Name;
            }
            if (Kind == "litval")
            {
                Tag = "lit_" + Name;
            }
            if (Kind == "parameter")
            {
                Tag = "param_" + Name;
            }
            if (ClassType != null && (Dims == null || Dims.Count == 0))
            {
                Size = ClassType.Size;
            }
            else if (ClassType != null && Dims != null && Dims.Count > 0)
            {
                Size = ClassType.Size;
                Size = GetSize();
            } else
            {
                Size = GetSize();
            }
        }

        public VariableEntry(string kind, string type, string name, List<int> dims, SymbolTable classType, string functionName) : base(kind, type, name, null)
        {
            Dims = dims;
            ClassType = classType;
            if (Kind == "local" || Kind == "variable")
            {
                Tag = "var_" + Name;
            }
            if (Kind == "litval")
            {
                Tag = "lit_" + Name;
            }
            if (Kind == "parameter")
            {
                Tag = functionName + "_param_" + Name;
            }
            if (ClassType != null && (Dims == null || Dims.Count == 0))
            {
                Size = ClassType.Size;
            }
            else if (ClassType != null && Dims != null && Dims.Count > 0)
            {
                Size = ClassType.Size;
                Size = GetSize();
            }
            else
            {
                Size = GetSize();
            }
        }

        public override string ToString()
        {
            return
                String.Format("{0,-12}", "| " + Visibility) +
                String.Format("{0,-12}", "| " + Kind) +
                String.Format("{0,-12}", "| " + Name) +
                String.Format("{0,-22}", "| " + Tag) +
                String.Format("{0,-12}", "| " + Type) +
                String.Format("{0,-8}", "| " + Size) +
                String.Format("{0,-8}", "| " + Offset) +
                String.Format("{0,-8}", "| " + Notes) +
                (ClassType != null ? String.Format("{0,-6}", "| linked to " + ClassType.Name) : "")
                + "|";
        }

        public int GetSize()
        {
            int size = Size;
            if (Dims != null && Dims.Count > 0)
            {
                Dims.ForEach(d => { if (d != 0) { size *= d; } });
            }
            return size;
        }
    }
}
