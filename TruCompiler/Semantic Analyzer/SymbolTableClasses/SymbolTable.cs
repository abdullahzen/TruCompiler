using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace TruCompiler.Semantic_Analyzer.SymbolTableClasses
{
    public class SymbolTable
    {
        public string Name { get; set; }
        public List<Entry> SymList { get; set; }
        public int Size { get; set; }
        public int TableLevel { get; set; }
        public SymbolTable UpperTable { get; set; }
        public int Offset { get; set; }

        public bool CircularInheritance { get; set; }


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
        private SymbolTable()
        {

        }

        public void addEntry(Entry entry)
        {
            SymList.Add(entry);
            if (entry.Kind != "function")
            {
                Size += entry.Size;
            }
        }

        public void addFirstEntry(Entry entry)
        {
            List<Entry> temp = new List<Entry>();
            temp.Add(entry);
            SymList.ForEach(e => { temp.Add(e); });
            Size += entry.Size;
            SymList = temp;
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

        public SymbolTable SearchNameTable(string name)
        {
            SymbolTable result = null;
            bool found = false;
            foreach (Entry ent in SymList)
            {
                if (ent.Name == name)
                {
                    result = this;
                    found = true;
                }
            }
            if (!found)
            {
                if (UpperTable != null)
                {
                    result = UpperTable.SearchNameTable(name);
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

        public Entry SearchFunctionNameAndParams(string name, int paramsCount, List<String> paramsTypes, string functionType)
        {
            Entry result = null;
            bool found = false;
            foreach (Entry ent in SymList)
            {
                try
                {
                    var t = (FunctionEntry)ent;
                    if (ent.Name == name && t.Params.Count == paramsCount && t.Type == functionType)
                    {
                        for(int i = 0; i < t.Params.Count; i++)
                        {
                            if (paramsTypes[i] == t.Params[i].Type.Type)
                            {
                                result = ent;
                                found = true;
                            } else
                            {
                                result = null;
                                found = false;
                            }
                        }
                        
                    }
                } catch (Exception)
                {
                    continue;
                }
            }
            if (!found)
            {
                if (UpperTable != null)
                {
                    result = UpperTable.SearchFunctionNameInUpper(name);
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
                    result = UpperTable.SearchFunctionNameInUpper(name);
                }
            }
            return result;
        }

        public SymbolTable ShallowCopy()
        {
            return (SymbolTable)this.MemberwiseClone();
        }

        public override string ToString()
        {
            string result = "";
            string lineSpacing = "";
            for (int i = 0; i < TableLevel; i++)
            {
                lineSpacing += "|    ";
            }

            result += "\n" + lineSpacing + "===============================================================================================\n";
            result += lineSpacing + String.Format("{0,-27}", "| table: " + Name) + String.Format("{0,-24}", " scope size: " + Size) + "|" + String.Format("{0,-42}", " scope offset: " + Offset) + "|\n";
            result += lineSpacing + "===============================================================================================\n";
            if (Name != "global")
            {
                result += String.Format("{0,-5}", "| ") +
                 String.Format("{0,-12}", "| " + "VISIBILITY") +
                 String.Format("{0,-12}", "| " + "KIND") +
                 String.Format("{0,-12}", "| " + "NAME") +
                 String.Format("{0,-22}", "| " + "TAG") +
                 String.Format("{0,-12}", "| " + "TYPE") +
                 String.Format("{0,-8}", "| " + "SIZE") +
                 String.Format("{0,-8}", "| " + "OFFSET") +
                 String.Format("{0,-8}", "| " + "NOTES") + "|\n";
            }
            
            for (int i = 0; i < SymList.Count; i++)
            {
                result += lineSpacing + SymList[i].ToString() + '\n';
            }

            result += lineSpacing + "===============================================================================================\n|";
            return result;
        }

    }
}
