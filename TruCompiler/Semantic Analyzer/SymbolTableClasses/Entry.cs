using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace TruCompiler.Semantic_Analyzer.SymbolTableClasses
{
    public class Entry
    {
        public string Kind { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public int Offset { get; set; }
        public SymbolTable SubTable { get; set; }
        public List<int> Dims { get; set; }
        public string Notes { get; set; }
        public string Tag { get; set; }

        public Entry()
        {

        }

        public Entry(string kind, string type, string name, SymbolTable subTable)
        {
            Kind = kind;
            Type = type;
            Name = name;
            SubTable = subTable;
            if (type.Equals("integer") || type.Equals("float"))
            {
                Size = 4;
            }
            Tag = Name;
        }
    }
}
