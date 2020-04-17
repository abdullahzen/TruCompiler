using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;
using TruCompiler.Nodes;
using TruCompiler.Semantic_Analyzer.SymbolTableClasses;

namespace TruCompiler.Semantic_Analyzer
{
    public class SymbolTableVisitor : Visitor<Token>
    {
		public static int TempNum { get; set; }
		public string Output { get; set; }

		public SymbolTableVisitor()
		{
		}

		public SymbolTableVisitor(string output)
		{
			if (!String.IsNullOrEmpty(output))
			{
				Output = output;
			}
		}

		public static string GetNewTempName()
		{
			TempNum++;
			return "t" + TempNum.ToString();
		}

		public override void visit(Node<Token> node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
		}

		public override void visit(ProgNode node)
		{
			node.SymbolTable = new SymbolTable(0, "global", null);
			
			foreach(Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
			if (!String.IsNullOrEmpty(Output))
			{
				Driver.WriteToFile(Output, node.SymbolTable.ToString());
			}
		}


		public override void visit(ClassListNode node)
		{
			foreach(Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}

		}

		public override void visit(ClassNode node)
		{
			string className = node.Name.IdValue;
			SymbolTable localClassTable = new SymbolTable(1, className, node.SymbolTable);
			List<SymbolTable> inheritedClasses = new List<SymbolTable>();
			if (node.InheritanceList != null)
			{
				foreach(IdNode c in node.InheritanceList.Classes)
				{
					List<ClassNode> classList = new List<ClassNode>();
					((ClassListNode)node.Parent).Classes.ForEach(classNd => classList.Add(classNd));

					if (classList.FindAll(nd => nd.Name.IdValue == c.IdValue).Count > 1)
					{
						Driver.SemanticErrors += String.Format("\nSemantic Error : Duplicate class definition was found in the same global scope at line {0} for class {1} at line {2}", classList.FindAll(nd => nd.Name.IdValue == c.IdValue).Find(n => n != node).Value.Line, node.Name.IdValue, node.Value.Line);
						return;
					} else
					{
						classList.Remove(node);
					}
					ClassNode classNode = classList.Find(cn => cn.Name.IdValue == c.IdValue);
					//check for base class definition
					if (classNode == null || classNode.Entry == null)
					{
						Driver.SemanticErrors += String.Format("\nSemantic Error : Base class {0} not found for the subclass {1} at line {2}", c.IdValue, node.Name.IdValue, node.Value.Line);
						return;
					} else if (classNode != null) 
					{
						inheritedClasses.Add(classNode.SymbolTable);
					}
				}
				if (node.HasCircularInheritance())
				{
					Driver.SemanticErrors += String.Format("\nSemantic Error : Circular inheritance found on subclass {0} at line {1}", node.Name.IdValue, node.Value.Line);
					return;
				}
			}

			node.Entry = new ClassEntry(className, inheritedClasses, localClassTable);
			if (node.SymbolTable.SearchName(className) != null)
			{
				Driver.SemanticErrors += String.Format("\nSemantic Error : Double class definitions found for class {0} at line {1}", node.Name.IdValue, node.Value.Line);
			} else
			{
				node.SymbolTable.addEntry(node.Entry);
				node.SymbolTable = localClassTable;
			}

			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
		}

		public override void visit(ClassMembersNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
		}

		public override void visit(MemberNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
		}

		public override void visit(FuncDeclNode node)
		{
			string visibility = "";

			if (node.Parent.Children.Count > 1)
			{
				visibility = node.Parent[0].Value.Value;
			}

			string type = "";
			if (!node.IsVoid())
			{
				type = node.ReturnType.Value.Value;
			}
			string name = node.Name.IdValue;
			SymbolTable localFunctionTable = new SymbolTable(2, name, node.SymbolTable);
			List<ParamNode> paramsList = new List<ParamNode>();
			foreach (ParamNode param in node.FParams.Params)
			{
				paramsList.Add(param);
			}

			//Check for duplicate functions and aallow overloaded
			foreach (Entry e in node.SymbolTable.SymList)
			{
				if (e.Kind == "function" && e.Name == name)
				{
					if (paramsList.Count == ((FunctionEntry)e).Params.Count)
					{
						Driver.SemanticErrors += String.Format("\nSemantic Error : Duplicate Function {0} found at line {1}", name, node.Value.Line);
						return;
					} else
					{
						Driver.SemanticErrors += String.Format("\nSemantic (Warning) : Overloaded Function {0} found at line {1}", name, node.Value.Line);
					}
				}
			}

			node.Entry = new FunctionEntry(visibility, type, name, paramsList, localFunctionTable);
			
			node.SymbolTable.addEntry(node.Entry);
			node.SymbolTable = localFunctionTable;


			//Check for shadowed members
			ClassEntry memberClass = (ClassEntry)node.SymbolTable.SearchName(node.SymbolTable.UpperTable.Name);
			if (memberClass != null)
			{
				foreach(SymbolTable classSymbol in memberClass.InheritedClasses)
				{
					foreach(Entry e in classSymbol.SymList)
					{
						if (e.Kind == "function" && e.Name == name)
						{
							Driver.SemanticErrors += String.Format("\nSemantic (Warning) : Function {0} in class {1} at line {2} is shadowing the same member from inherited class {3}", name, memberClass.Name, node.Value.Line, classSymbol.Name);
						}
					}
				}
			}

		}

		public override void visit(VariableDeclNode node)
		{
			string visibility = "";

			if (node.Parent.Children.Count > 1)
			{
				visibility = node.Parent[0].Value.Value;
			}

			string type = node.Type.Value.Value;
			SymbolTable classType = null;
			ClassNode classNode = null;
			if (node.Type.Value.Lexeme == Lexeme.id)
			{
				Node<Token> temp = node;
				while (temp.Parent != null)
				{
					temp = temp.Parent;
				}
				ClassListNode classList = ((ProgNode)temp[0]).ClassList;
				if (classList != null && classList.Classes.Count > 0)
				{
					classNode = classList.Classes.Find(c => c.Name.IdValue == type);
					if (classNode != null && classNode.Entry != null)
					{
						classType = classNode.Entry.SubTable;
					}
					else
					{
						Driver.SemanticErrors += String.Format("\nSemantic Error : Variable {0} at line {1} has invalid type {2}", node.Name.IdValue, node.Name.Value.Line, type);
						return;
					}
				}
				else
				{
					Driver.SemanticErrors += String.Format("\nSemantic Error : Variable {0} at line {1} has invalid type {2}", node.Name.IdValue, node.Name.Value.Line, type);
					return;
				}
			}
			
			string name = node.Name.IdValue;
			//Check for duplicate vairables
			foreach(Entry e in node.SymbolTable.SymList)
			{
				if (e.Kind == "variable" && e.Name == name)
				{
					Driver.SemanticErrors += String.Format("\nSemantic Error : Duplicate Variable {0} found at line {1}", name, node.Value.Line);
					return;
				}
			}

			string varType = "variable";
			if (node.Parent.Value.Value == "local") {
				varType = "local";
			}

			List<int> dims = new List<int>();
			if (node.ArraySize != null)
			{
				foreach(ArraySizeNode arr in node.ArraySize)
				{
					if (arr.ArraySizeValue != null)
					{
						dims.Add(arr.ArraySizeValue);
					}
				}
			}
			
			node.Entry = new VariableEntry(visibility, varType, type, name, dims, node.SymbolTable.Name, classType);
			
			node.SymbolTable.addEntry(node.Entry);

			//Check for shadowed members
			ClassEntry memberClass = (ClassEntry)node.SymbolTable.SearchName(node.SymbolTable.UpperTable.Name);
			if (memberClass != null)
			{
				foreach (SymbolTable classSymbol in memberClass.InheritedClasses)
				{
					foreach (Entry e in classSymbol.SymList)
					{
						if (e.Kind == "variable" && e.Name == name)
						{
							Driver.SemanticErrors += String.Format("\nSemantic (Warning) : Variable {0} in class {1} at line {2} is shadowing the same member from inherited class {3}", name, memberClass.Name, node.Value.Line, classSymbol.Name);
						}
					}
				}
			}
		}

		public override void visit(FuncDefsNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
		}

		public override void visit(FuncDefNode node)
		{
			FuncHeadNode head = node.FunctionHead;
			SymbolTable localFunctionTable = null;
			string type = "";
			string name = "";
			name = head.FunctionName.IdValue;
			if (!node.IsVoid())
			{
				type = node.GetReturnType().Value.Value;
			}

			bool freeFunction = true;
			if (head.ClassName != null && head.ClassName.IdValue != "")
			{
				freeFunction = false;
				ClassListNode classList = ((ProgNode)node.Parent.Parent).ClassList;
				if (classList != null && classList.Classes.Count > 0)
				{
					ClassNode classNode = classList.Classes.Find(c => c.Name.IdValue == head.ClassName.IdValue);
					if (classNode != null && classNode.Entry != null)
					{
						node.Entry = classNode.Entry.SubTable.SymList.Find(e => e.Name == name);
						localFunctionTable = node.Entry.SubTable;
					} else
					{
						Driver.SemanticErrors += String.Format("\nSemantic Error : Function {0} at line {1} is not declared in corresponding class {2}", head.FunctionName.IdValue, head.FunctionName.Value.Line, head.ClassName.IdValue);
						return;
					}
				}
				else
				{
					Driver.SemanticErrors += String.Format("\nSemantic Error : Function {0} at line {1} is not declared in corresponding class {2}", head.FunctionName.IdValue, head.FunctionName.Value.Line, head.ClassName.IdValue);
					return;
				}
			}
			if (localFunctionTable == null)
			{

				localFunctionTable = new SymbolTable(1, name, node.SymbolTable);
				List<ParamNode> paramsList = new List<ParamNode>();
				if (head != null && head.FParams != null && head.FParams.Params != null)
				{
					foreach (ParamNode param in head.FParams.Params)
					{
						paramsList.Add(param);
					}
				}

				//Check for duplicate functions and aallow overloaded
				foreach (Entry e in node.SymbolTable.SymList)
				{
					if (e.Kind == "function" && e.Name == name)
					{
						if (paramsList.Count == ((FunctionEntry)e).Params.Count)
						{
							Driver.SemanticErrors += String.Format("\nSemantic Error : Duplicate Function {0} found at line {1}", name, node.Value.Line);
							return;
						}
						else
						{
							Driver.SemanticErrors += String.Format("\nSemantic (Warning) : Overloaded Function {0} found at line {1}", name, node.Value.Line);
						}
					}
				}

				node.Entry = new FunctionEntry(type, name, paramsList, localFunctionTable);
				node.SymbolTable.addEntry(node.Entry);

			}
			node.SymbolTable = localFunctionTable;
			node.FunctionHead.SymbolTable = node.SymbolTable;
			node.FunctionBody.SymbolTable = node.SymbolTable;
			node.FunctionBody.accept(this);
		}


		public override void visit(FuncBodyNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
		}

		public override void visit(AddOpNode node)
		{
			foreach(Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}

			String tempvarname = GetNewTempName();
			node.TempVarName = tempvarname;
			string type = GetType((ArithExprNode)node.Left);
			node.Entry = new VariableEntry("tempvar", type, node.TempVarName, null, null);
			node.SymbolTable.addEntry(node.Entry);
		}

		public override void visit(MultOpNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
			String tempvarname = GetNewTempName();
			node.TempVarName = tempvarname;
			string type = GetType((ArithExprNode)node.Left);

			node.Entry = new VariableEntry("tempvar", type, node.TempVarName, null, null);
			node.SymbolTable.addEntry(node.Entry);
		}

		public override void visit(NumNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
			String tempvarname = GetNewTempName();
			node.TempVarName = tempvarname;
			string type = node.Type;
			node.Entry = new VariableEntry("litval", type, node.TempVarName, null, null);
			if (node.Type == "integer")
			{
				node.Entry.Notes = node.IntValue.ToString();
			} else
			{
				node.Entry.Notes = node.FloatValue.ToString();
			}
			node.SymbolTable.addEntry(node.Entry);
		}

		public override void visit(AssignStatementNode node)
		{
			node.Type = GetVariableType(node.Left, node);
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
		}

		public override void visit(ArithExprNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
			GetType(node);
		}

		public override void visit(FunctionCallNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
			node.Type = GetFunctionCallType(node, node);
			String tempvarname = GetNewTempName();
			node.TempVarName = tempvarname;
			string type = node.Type;

			node.Entry = new VariableEntry("retval", type, node.TempVarName, null, null);
			node.SymbolTable.addEntry(node.Entry);
		}

		public override void visit(ReturnStatementNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
			ArithExprNode arith = node.Expression.ArithExpr;
			if (arith != null)
			{
				node.Type = GetType(arith);
				//String tempvarname = GetNewTempName();
				//node.TempVarName = tempvarname;
				//string type = node.Type;
				//node.Entry = new VariableEntry("retval", type, node.TempVarName, null);
				//node.SymbolTable.addEntry(node.Entry);
			}
		}

		public override void visit(WriteStatementNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
			ArithExprNode arith = node.Expression.ArithExpr;
			if (arith != null)
			{
				node.Type = GetType(arith);
				//String tempvarname = GetNewTempName();
				//node.TempVarName = tempvarname;
				string type = node.Type;
				//node.Entry = new VariableEntry("retval", type, node.TempVarName, null);
				//node.SymbolTable.addEntry(node.Entry);
			}
		}

		public override void visit(RelExprNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
			ArithExprNode arith1 = node.LeftArithExpr;
			ArithExprNode arith2 = node.RightArithExpr;

			if (arith1 != null && arith2 != null)
			{
				node.Type = GetType(arith1);
				GetType(arith2);
			}
			String tempvarname = "rel_" + GetNewTempName();
			node.TempVarName = tempvarname;
			node.Entry = new VariableEntry("rel_tempvar", "integer", node.TempVarName, null, null);
			node.SymbolTable.addEntry(node.Entry);
		}

		public override void visit(VariableNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
			node.Entry = node.SymbolTable.SearchName(node.Name);
		}

		public override void visit(MainNode node)
		{
			FuncBodyNode mainFunction = node.FuncBody;
			string type = "void";
			string name = "main";
			SymbolTable localFunctionTable = new SymbolTable(1, name, node.SymbolTable);
			List<ParamNode> paramsList = new List<ParamNode>();
		
			node.Entry = new FunctionEntry(type, name, paramsList, localFunctionTable);

			node.SymbolTable.addEntry(node.Entry);
			node.SymbolTable = localFunctionTable;
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
		}


		public override void visit(IdNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
			node.TempVarName = node.IdValue;
		}

		public override void visit(ReadStatementNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
		}

		public override void visit(WhileStatementNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
		}

		public override void visit(IfStatementNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
		}


		public static string GetType(ArithExprNode node)
		{
			string type = "";
			if (node.Children.Count > 0)
			{
				switch (node[0].Value.Lexeme)
				{
					case Lexeme.keyword:
						switch (node[0].Value.Value)
						{
							case "Signed":
								type = ((NumNode)((SignNode)node[0]).Factor).Type;
								node.Type = type;
								break;
							case "FunctionCall":
								type = GetFunctionCallType((FunctionCallNode)node[0], node);
								break;
							case "Variable":
								VariableNode variable = (VariableNode)node[0];
								type = GetVariableType(variable, node);
								break;
						}
						break;
					case Lexeme.floatnum:
						node[0].Type = "float";
						type = node[0].Type;
						break;
					case Lexeme.intnum:
						node[0].Type = "integer";
						type = node[0].Type;
						break;
				}
			}
			return type;
		}

		public static string GetVariableType(VariableNode variable, Node<Token> node)
		{
			string type = "";
			if (variable.Children.Count == 1)
			{
				Entry entry = node.SymbolTable.SearchName(variable.Name);
				if (entry != null)
				{
					type = entry.Type;
					node.Type = type;
				}
				else
				{
					Driver.SemanticErrors += String.Format("\nSemantic Error : Variable {0} at line {1} is not defined in the current scope", variable.Name, node.Value.Line);
					type = "invalidType";
				}
			}
			else if (variable.Children.Count > 1)
			{
				if (variable.Children.FindLast(c => true).Value.Value == "ArraySizeValue")
				{
					List<Node<Token>> subvars = new List<Node<Token>>();
					variable.Children.ForEach(c => subvars.Add(c));
					subvars.RemoveAt(subvars.Count - 1);
					string firstVar = ((IdNode)variable[0]).IdValue;
					Entry entry = node.SymbolTable.SearchName(firstVar);
					if (entry != null)
					{
						int count = subvars.Count;
						int index = 1;
						VariableEntry tempEntry = (VariableEntry)entry;
						while (count != 0)
						{
							if (tempEntry.ClassType != null)
							{
								tempEntry = (VariableEntry)tempEntry.ClassType.SearchName(((IdNode)subvars[index]).IdValue);
							}
							if (tempEntry == null)
							{
								Driver.SemanticErrors += String.Format("\nSemantic Error : {0} is not a member of the object {1} at line {2}", ((IdNode)subvars[index]).IdValue, firstVar, node.Value.Line);
								type = "invalidType";
								break;
							}
							count--;
							index++;
						}
						if (tempEntry != null)
						{
							type = tempEntry.Type;
							node.Type = type;
						}
					}
					else
					{
						Driver.SemanticErrors += String.Format("\nSemantic Error : Variable {0} at line {1} is not defined in the current scope", firstVar, node.Value.Line);
						type = "invalidType";
					}
				}
				else
				{
					string firstVar = ((IdNode)variable[0]).IdValue;
					Entry entry = node.SymbolTable.SearchName(firstVar);
					if (entry != null)
					{
						int count = variable.Children.Count;
						int index = 1;
						VariableEntry tempEntry = (VariableEntry)entry;
						while (count != 0)
						{
							if (tempEntry.ClassType != null)
							{
								tempEntry = (VariableEntry)tempEntry.ClassType.SearchName(((IdNode)variable[index]).IdValue);
							}
							if (tempEntry == null)
							{
								Driver.SemanticErrors += String.Format("\nSemantic Error : {0} is not a member of the object {1} at line {2}", ((IdNode)variable[index]).IdValue, firstVar, node.Value.Line);
								type = "invalidType";
								break;
							}
							count--;
							index++;
						}
						if (tempEntry != null)
						{
							type = tempEntry.Type;
							node.Type = type;
						}
					}
					else
					{
						Driver.SemanticErrors += String.Format("\nSemantic Error : Variable {0} at line {1} is not defined in the current scope", firstVar, node.Value.Line);
						type = "invalidType";
					}
				}
			}
			return type;
		}

		public static string GetFunctionCallType(FunctionCallNode functionCall, Node<Token> node)
		{
			string type = "";
			Node<Token> temp = node;
			while (temp.Parent != null)
			{
				temp = temp.Parent;
			}
			FuncDefsNode funcs = ((ProgNode)temp[0]).FunctionDefinitions;

			if (funcs != null && funcs.Children.Count > 0)
			{
				FuncDefNode func = (FuncDefNode)funcs.Children.Find(f => (f.Entry != null) ? f.Entry.Name == functionCall.Name.IdValue : false);
				
				if (func != null && func.Entry != null)
				{
					type = func.Entry.Type;
					node.Type = type;
				}
				else
				{
					Driver.SemanticErrors += String.Format("\nSemantic Error : Call to undeclared Function {0} at line {1}", functionCall.Name.IdValue, functionCall.Name.Value.Line);
					type = "invalidType";
				}
			}
			else
			{
				Driver.SemanticErrors += String.Format("\nSemantic Error : Call to undeclared Function {0} at line {1}", functionCall.Name.IdValue, functionCall.Name.Value.Line);
				type = "invalidType";
			}
			return type;
		}

	}
}
