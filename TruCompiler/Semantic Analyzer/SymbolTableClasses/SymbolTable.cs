using System;
using System.Collections.Generic;
using System.Text;

namespace TruCompiler.Semantic_Analyzer.SymbolTableClasses
{
    public class SymbolTable
    {
        public string Name { get; set; }
        public List<Entry> SymList { get; set; }
        public int Size { get; set; }
        public int TableLevel { get; set; }
        public SymbolTable UpperTable { get; set; }

        public SymbolTable(int tableLevel, SymbolTable upperTable)
        {
            TableLevel = tableLevel;
            Name = null;
            SymList = new List<Entry>();
            UpperTable = upperTable;
        }

        public SymbolTable(int tableLevel, string name, SymbolTable upperTable)
        {
            TableLevel = tableLevel;
            Name = name;
            SymList = new List<Entry>();
            UpperTable = upperTable;
        }

        public SymbolTable(int tableLevel, string name)
        {
            TableLevel = tableLevel;
            Name = name;
            SymList = new List<Entry>();
        }

        public void addEntry(Entry entry)
        {
            SymList.Add(entry);
        }

        public Entry SearchName(string name)
        {
            Entry result = null;
            bool found = false;
            foreach (Entry ent in SymList)
            {
                if (ent.Name == name)
                {
                    result = ent;
                    found = true;
                }
            }
            if (!found)
            {
                if (UpperTable != null)
                {
                    result = UpperTable.SearchName(name);
                }
            }
            return result;
        }

        public Entry SearchFunctionName(string name)
        {
            Entry result = null;
            bool found = false;
            foreach (Entry ent in SymList)
            {
                if (ent.Name == name)
                {
                    result = ent;
                    found = true;
                }
            }
            return result;
        }

        public Entry SearchFunctionNameInUpper(string name)
        {
            Entry result = null;
            bool found = false;
            foreach (Entry ent in SymList)
            {
                if (ent.Name == name)
                {
                    result = ent;
                    found = true;
                }
            }
            if (!found)
            {
                if (UpperTable != null)
                {
                    result = UpperTable.SearchName(name);
                }
            }
            return result;
        }

        public override string ToString()
        {
            string result = "";
            string lineSpacing = "";
            for (int i = 0; i < TableLevel; i++)
            {
                lineSpacing += "|    ";
            }

            result += "\n" + lineSpacing + "=====================================================\n";
            result += lineSpacing + String.Format("{0,-25}", "| table: " + Name) + /*String.Format("{0,-27}", " scope offset: " + Size)*/ "|\n";
            result += lineSpacing + "=====================================================\n";
            
            for (int i = 0; i < SymList.Count; i++)
            {
                result += lineSpacing + SymList[i].ToString() + '\n';
            }

            result += lineSpacing + "=====================================================";
            return result;
        }
    }
}
