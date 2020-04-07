using System;
using System.Collections.Generic;
using System.Text;

namespace TruCompiler.Semantic_Analyzer.SymbolTableClasses
{
    public class ClassEntry : Entry
    {
        public List<SymbolTable> InheritedClasses { get; set; }
        public ClassEntry(string name, SymbolTable subTable) : base("class", name, name, subTable)
        {
            
        }

        public ClassEntry(string name, List<SymbolTable> inheritedClasses, SymbolTable subTable) : base("class", name, name, subTable)
        {
            InheritedClasses = inheritedClasses;
        }

        public override string ToString()
        {
            return String.Format("{0,-12}", "| " + Kind) +
                            String.Format("{0,-12}", "| " + Name) +
                            GetStringOfInheritedClasses() + 
                            "|" +
                            SubTable;
        }

        public string GetStringOfInheritedClasses()
        {
            string result = "";
            if (InheritedClasses != null && InheritedClasses.Count > 0)
            {
                InheritedClasses.ForEach(c =>
                {
                    result += c.Name + ",";
                });
                result = result.TrimEnd(',');
                result = String.Format("{0,-28}", "| inherits " + result);
            }
            return result;
        }
    }
}
