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

		public SymbolTableVisitor()
		{
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
				child.accept(this);
			}
		}

		public override void visit(ProgNode node)
		{
			foreach(Node<Token> child in node.Children)
			{
				child.accept(this);
			}
		}


		public override void visit(ClassListNode node)
		{
			foreach(Node<Token> child in node.Children)
			{
				child.accept(this);
			}

		}

		public override void visit(ClassNode node)
		{
			string className = node.Name.IdValue;
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
					node.SymbolTable.UpperTable.SymList.Remove(node.Entry);
					Driver.SemanticErrors += String.Format("\nSemantic Error : Circular inheritance found on subclass {0} at line {1}", node.Name.IdValue, node.Value.Line);
					return;
				}
			}

			((ClassEntry)node.Entry).InheritedClasses = inheritedClasses;
			SymbolTable upper = node.SymbolTable.UpperTable;
			List<Entry> originalList = new List<Entry>();
			upper.SymList.ForEach(e => { originalList.Add(e); });
			upper.SymList.Remove(node.Entry);
			if (upper.SearchName(className) != null)
			{
				Driver.SemanticErrors += String.Format("\nSemantic Error : Double class definitions found for class {0} at line {1}", node.Name.IdValue, node.Value.Line);
			}
			upper.SymList = originalList;

			foreach (Node<Token> child in node.Children)
			{
				child.accept(this);
			}
		}

		public override void visit(ClassMembersNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.accept(this);
			}
		}

		public override void visit(MemberNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
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
			List<ParamNode> paramsList = new List<ParamNode>();
			foreach (ParamNode param in node.FParams.Params)
			{
				paramsList.Add(param);
			}

			//Check for duplicate functions and aallow overloaded
			List<Entry> originalList = node.SymbolTable.UpperTable.SymList;
			List<Entry> upperList = new List<Entry>();
			originalList.ForEach(e => { upperList.Add(e); });
			upperList.Remove(node.Entry);
			foreach (Entry e in upperList)
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
			node.Name.Entry = node.Entry;
			node.SymbolTable.addFirstEntry(node.Entry);

			//Check for shadowed members
			ClassEntry memberClass = (ClassEntry)node.SymbolTable.SearchName(node.SymbolTable.UpperTable.Name);
			if (memberClass != null)
			{
				if (memberClass.InheritedClasses != null)
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
		}

		public override void visit(FuncDefsNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.accept(this);
			}
		}

		public override void visit(FuncDefNode node)
		{
			FuncHeadNode head = node.FunctionHead;
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
					if (classNode == null || classNode.Entry == null)
					{
						Driver.SemanticErrors += String.Format("\nSemantic Error : Function {0} at line {1} is not declared in corresponding class {2}", head.FunctionName.IdValue, head.FunctionName.Value.Line, head.ClassName.IdValue);
						return;
					} else
					{
						List<String> paramsTypes = new List<String>();
						head.FParams.Params.ForEach(p => paramsTypes.Add(p.Type.Type));
						FunctionEntry linkedFuncEntry = (FunctionEntry)node.SymbolTable.SearchFunctionNameAndParams(name, head.FParams.Params.Count, paramsTypes, head.ReturnType.Type);
						if (linkedFuncEntry != null)
						{
							node.Entry = linkedFuncEntry;
							
						} else
						{
							Driver.SemanticErrors += String.Format("\nSemantic Error : Function {0} at line {1} is not declared in corresponding class {2}", head.FunctionName.IdValue, head.FunctionName.Value.Line, head.ClassName.IdValue);
							return;
						}
					}
				}
				else
				{
					Driver.SemanticErrors += String.Format("\nSemantic Error : Function {0} at line {1} is not declared in corresponding class {2}", head.FunctionName.IdValue, head.FunctionName.Value.Line, head.ClassName.IdValue);
					return;
				}
			}
			if (freeFunction)
			{
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
			}
			if (node.Entry != null)
			{
				head.FParams.Params = ((FunctionEntry)node.Entry).Params;
				node.FunctionHead.SymbolTable = node.SymbolTable;
				node.FunctionBody.SymbolTable = node.SymbolTable;
				node.FunctionBody.accept(this);
			} else
			{

			}
		}


		public override void visit(FuncBodyNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.accept(this);
			}
		}

		public override void visit(AddOpNode node)
		{
			foreach(Node<Token> child in node.Children)
			{
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

		public override void visit(ExprNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.accept(this);
			}
			node.Type = node[0].Type;
		}


		public override void visit(AssignStatementNode node)
		{
			node.Type = GetVariableType(node.Left, node);
			foreach (Node<Token> child in node.Children)
			{
				child.accept(this);
			}
		}

		public override void visit(ArithExprNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.accept(this);
			}
			GetType(node);
			if (node.Children.Count > 0 && node[0].Value.Value == "Signed")
			{
				node[0][1].Entry.Notes = node[0][0].Value.Value + node[0][1].Entry.Notes;
				node.Entry = node[0][1].Entry;
			}
		}

		public override void visit(FunctionCallNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.accept(this);
			}
			SymbolTable symbolTable = node.SymbolTable;
			string baseClass = "";
			if (node.Children.Count > 2)
			{
				for(int i = 0; i < node.Children.Count - 2; i++)
				{
					Entry e = symbolTable.SearchName(node[i].Value.Value);
					if (e != null)
					{
						baseClass = e.Type;
					} else
					{
						Driver.SemanticErrors += String.Format("\nSemantic Error: variable or member {0} not defined at line {1}", node[i].Value.Value, node[i].Value.Line);
						return;
					}
					symbolTable = symbolTable.SearchNameTable(node[i].Value.Value);
				}
			} 

			node.Type = GetFunctionCallType(node, node, baseClass);

			if (String.IsNullOrEmpty(node.Type))
			{
				Driver.SemanticErrors += String.Format("\nSemantic Error: Function {0} not defined at line {1}", node.Name.IdValue, node.Name.Value.Line);
				return;
			}

			String tempvarname = "retval_" + GetNewTempName();
			node.TempVarName = tempvarname;
			string type = node.Type;

			node.Entry = new VariableEntry("retval", type, node.TempVarName, null, null);
			node.SymbolTable.addEntry(node.Entry);
			if (symbolTable != null)
			{
				node.SymbolTable = symbolTable;
			}
		}

		public override void visit(ReturnStatementNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.accept(this);
			}
			ArithExprNode arith = node.Expression.ArithExpr;
			if (arith != null)
			{
				node.Type = GetType(arith);
				/*String tempvarname = "ret_val" + GetNewTempName();
				node.TempVarName = tempvarname;
				string type = node.Type;
				node.Entry = new VariableEntry("ret_val", type, node.TempVarName, null, null);
				node.SymbolTable.addEntry(node.Entry);*/
			}
		}

		public override void visit(WriteStatementNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
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
				child.accept(this);
			}

			node.Entry = node.SymbolTable.SearchName(node.Name);
			if (node.Entry == null || node.Children.Count > 1)
			{
				var temp = node[0];
				var tempEntry = node.SymbolTable.SearchName(((IdNode)node[0]).IdValue);
				node[0].Entry = tempEntry;
				for(int i = 1; i < node.Children.Count; i++)
				{
					if (node[i].Value.Value != "ArraySizeValue")
					{
						if (tempEntry == null)
						{
							Driver.SemanticErrors += String.Format("Semantic Error: Variable {0} reference is null at line {1}.", ((IdNode)temp).IdValue , temp.Value.Line);
							node.Entry = null;
							Entry removedEntry = null;
							foreach(Entry entry in node.SymbolTable.UpperTable.SymList)
							{
								if (entry.SubTable != null && entry.SubTable == node.SymbolTable)
								{
									removedEntry = entry;
									break;
								}
							}
							if (removedEntry != null)
							{
								node.SymbolTable.UpperTable.SymList.Remove(removedEntry);
								Node<Token> tempNode = node;
								while (tempNode.Value.Value != "Function")
								{
									tempNode = tempNode.Parent;
								}
								tempNode.Entry = null;
								tempNode.SymbolTable = tempNode.SymbolTable.UpperTable;
							}
							break;
						}
						if (((VariableEntry)tempEntry).ClassType != null)
						{
							node.Entry = ((VariableEntry)tempEntry).ClassType.SearchName(((IdNode)node[i]).IdValue);
							node[i].Entry = node.Entry;
						}
					} else
					{
						node.Entry = tempEntry;
						break;
					}
					temp = node[i];
					tempEntry = node.Entry;
				}
			}
		}

		public override void visit(MainNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.accept(this);
			}
		}


		public override void visit(IdNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.accept(this);
			}
			node.TempVarName = node.IdValue;
		}

		public override void visit(ReadStatementNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.accept(this);
			}
		}

		public override void visit(WhileStatementNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.accept(this);
			}
		}

		public override void visit(IfStatementNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
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
								SymbolTable symbolTable = node.SymbolTable;
								string baseClass = "";
								if (node[0].Children.Count > 2)
								{
									for (int i = 0; i < node[0].Children.Count - 2; i++)
									{
										Entry e = symbolTable.SearchName(node[0][i].Value.Value);
										if (e != null)
										{
											baseClass = e.Type;
										}
										else
										{
											Driver.SemanticErrors += String.Format("\nSemantic Error: variable or member {0} not defined at line {1}", node[0][i].Value.Value, node[0][i].Value.Line);
											type = "invalidType";
										}
										symbolTable = symbolTable.SearchNameTable(node[0][i].Value.Value);
									}
								}
								if (type != "invalidType")
								{
									type = GetFunctionCallType((FunctionCallNode)node[0], node, baseClass);
									node.Type = type;
									node[0].Type = type;
								}

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
					variable.Type = type;
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
							variable.Type = type;
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
							variable.Type = type;
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

		public static string GetFunctionCallType(FunctionCallNode functionCall, Node<Token> node, string baseClass)
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
				FuncDefNode func = (FuncDefNode)funcs.Children.Find(f => IsFunctionEqual((FuncDefNode)f, functionCall, baseClass));

				if (func != null && func.Entry != null)
				{
					type = func.Entry.Type;
					node.Type = type;
					functionCall.Type = type;
				}
				else if (!String.IsNullOrEmpty(baseClass)) {
					if (node.SymbolTable.SearchName(baseClass) != null)
					{
						foreach (SymbolTable c in ((ClassEntry)node.SymbolTable.SearchName(baseClass)).InheritedClasses)
						{
							type = GetFunctionCallType(functionCall, node, c.Name);
							if (type != "invalidType" && !String.IsNullOrEmpty(type))
							{
								node.Type = type;
								functionCall.Type = type;
								break;
							}
						}
					} else
					{
						type = "invalidType";
					}
					
					if (type == "invalidType")
					{
						Driver.SemanticErrors += String.Format("\nSemantic Error : Call to undeclared Function {0} at line {1}", functionCall.Name.IdValue, functionCall.Name.Value.Line);
					}
				} else
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

		public static bool IsFunctionEqual(FuncDefNode f, FunctionCallNode functionCall, string baseClass)
		{
			if (f.Entry != null)
			{
				if (f.Entry.Name != functionCall.Name.IdValue)
				{
					return false;
				}
				else if (((FunctionEntry)f.Entry).Params.Count != functionCall.AParams.Children.Count)
				{
					return false;
				}
				else if (!String.IsNullOrEmpty(baseClass) && f.FunctionHead.ClassName.IdValue != baseClass)
				{
					return false;
				}
				return true;
			}
			return false;
		}
	}
}
