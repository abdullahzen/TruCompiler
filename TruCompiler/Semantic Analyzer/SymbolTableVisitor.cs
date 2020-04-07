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
		public int TempNum { get; set; }
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

		public string GetNewTempName()
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
			node.Entry = new FunctionEntry(visibility, type, name, paramsList, localFunctionTable);
			
			node.SymbolTable.addEntry(node.Entry);
			node.SymbolTable = localFunctionTable;
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
					ClassNode classNode = classList.Classes.Find(c => c.Name.IdValue == type);
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
			node.Entry = new VariableEntry(visibility, "variable", type, name, null);
			if (classType != null)
			{
				((VariableEntry)node.Entry).ClassType = classType;
			}
			node.SymbolTable.addEntry(node.Entry);
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
				foreach (ParamNode param in head.FParams.Params)
				{
					paramsList.Add(param);
				}
				node.Entry = new FunctionEntry(type, name, paramsList, localFunctionTable);
				localFunctionTable.addEntry(node.Entry);
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

			String tempvarname = this.GetNewTempName();
			node.TempVarName = tempvarname;
			string type = GetType((ArithExprNode)node.Left);
			node.Entry = new VariableEntry("tempvar", type, node.TempVarName, null);
			node.SymbolTable.addEntry(node.Entry);
		}

		public override void visit(MultOpNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
			String tempvarname = this.GetNewTempName();
			node.TempVarName = tempvarname;
			string type = GetType((ArithExprNode)node.Left);

			node.Entry = new VariableEntry("tempvar", type, node.TempVarName, null);
			node.SymbolTable.addEntry(node.Entry);
		}

		public override void visit(NumNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
			String tempvarname = this.GetNewTempName();
			node.TempVarName = tempvarname;
			string type = node.Type;
			node.Entry = new VariableEntry("litval", type, node.TempVarName, null);
			node.SymbolTable.addEntry(node.Entry);
		}

		public override void visit(AssignStatementNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
		}

		/*public override void visit(MainNode node)
		{

		}*/


		public string GetType(ArithExprNode node)
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
								type = GetType(((ArithExprNode)((SignNode)node[0]).Factor));
								node.Type = type;
								break;
							case "FunctionCall":
								FunctionCallNode functionCall = (FunctionCallNode)node[0];
								Node<Token> temp = node;
								while(temp.Parent != null)
								{
									temp = temp.Parent;
								}
								FuncDefsNode funcs = ((ProgNode)temp[0]).FunctionDefinitions;

								if (funcs != null && funcs.Children.Count > 0)
								{
									FuncDefNode func = (FuncDefNode)funcs.Children.Find(f => f.Entry.Name == functionCall.Name.IdValue);
									if (func != null && func.Entry != null)
									{
										type = func.Entry.Type;
										node.Type = type;
									} else
									{
										Driver.SemanticErrors += String.Format("\nSemantic Error : Call to non-existing Function {0} at line {1}", functionCall.Name.IdValue, functionCall.Name.Value.Line);
										type = "invalidType";
									}
								} else
								{
									Driver.SemanticErrors += String.Format("\nSemantic Error : Call to non-existing Function {0} at line {1}", functionCall.Name.IdValue, functionCall.Name.Value.Line);
									type = "invalidType";
								}
								break;
							case "Variable":
								VariableNode variable = (VariableNode)node[0];
								if (variable.Children.Count == 1)
								{
									Entry entry = node.SymbolTable.SearchName(variable.Name);
									if (entry != null)
									{
										type = entry.Type;
										node.Type = type;
									}else
									{
										Driver.SemanticErrors += String.Format("\nSemantic Error : Variable {0} at line {1} is not defined in the current scope", variable.Name, variable.Value.Line);
										type = "invalidType";
									}
								} else if (variable.Children.Count > 1)
								{
									if (variable.Children.FindLast(c => true).Value.Value == "ArraySizeValue")
									{

									} else
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
											}

											type = tempEntry.Type;
											node.Type = type;
										}
										else
										{
											Driver.SemanticErrors += String.Format("\nSemantic Error : Variable {0} at line {1} is not defined in the current scope", firstVar, variable.Value.Line);
											type = "invalidType";
										}
									}
								}
								break;
						}
						break;
					case Lexeme.floatnum:
						node[0].Type = "float";
						type = node[0].Type;
						break;
					case Lexeme.intnum:
						node[0].Type = "int";
						type = node[0].Type;
						break;
				}
			}
			return type;
		}


		/*
				public void visit(StatBlockNode p_node)
				{
					// propagate accepting the same visitor to all the children
					// this effectively achieves Depth-First AST Traversal
					for (Node child : p_node.getChildren())
					{
						child.m_symtab = p_node.m_symtab;
						child.accept(this);
					}
				};

				public void visit(ProgramBlockNode p_node)
				{
					SymTab localtable = new SymTab(1, "program", p_node.m_symtab);
					p_node.m_symtabentry = new FuncEntry("void", "program", new Vector<VarEntry>(), localtable);
					p_node.m_symtab.addEntry(p_node.m_symtabentry);
					p_node.m_symtab = localtable;
					// propagate accepting the same visitor to all the children
					// this effectively achieves Depth-First AST Traversal
					for (Node child : p_node.getChildren())
					{
						child.m_symtab = p_node.m_symtab;
						child.accept(this);
					}
				};



				

				public void visit(VarDeclNode p_node)
				{
					
				}

			

				public void visit(DimListNode p_node)
				{
					// propagate accepting the same visitor to all the children
					// this effectively achieves Depth-First AST Traversal
					for (Node child : p_node.getChildren())
					{
						child.m_symtab = p_node.m_symtab;
						child.accept(this);
					}
				};

				public void visit(FuncDefListNode p_node)
				{
					// propagate accepting the same visitor to all the children
					// this effectively achieves Depth-First AST Traversal
					for (Node child : p_node.getChildren())
					{
						child.m_symtab = p_node.m_symtab;
						child.accept(this);
					}
				};

				public void visit(IdNode p_node)
				{
					// propagate accepting the same visitor to all the children
					// this effectively achieves Depth-First AST Traversal
					for (Node child : p_node.getChildren())
					{
						child.m_symtab = p_node.m_symtab;
						child.accept(this);
					}
					p_node.m_moonVarName = p_node.m_data;
				};

				public void visit(Node p_node)
				{
					// propagate accepting the same visitor to all the children
					// this effectively achieves Depth-First AST Traversal
					for (Node child : p_node.getChildren())
					{
						child.m_symtab = p_node.m_symtab;
						child.accept(this);
					}
				};

				public void visit(PutStatNode p_node)
				{
					// propagate accepting the same visitor to all the children
					// this effectively achieves Depth-First AST Traversal
					for (Node child : p_node.getChildren())
					{
						child.m_symtab = p_node.m_symtab;
						child.accept(this);
					}
				};

				public void visit(TypeNode p_node)
				{
					// propagate accepting the same visitor to all the children
					// this effectively achieves Depth-First AST Traversal
					for (Node child : p_node.getChildren())
					{
						child.m_symtab = p_node.m_symtab;
						child.accept(this);
					}
				};
				public void visit(ParamListNode p_node)
				{
					// propagate accepting the same visitor to all the children
					// this effectively achieves Depth-First AST Traversal
					for (Node child : p_node.getChildren())
					{
						child.m_symtab = p_node.m_symtab;
						child.accept(this);
					}
				}

				public void visit(DimNode p_node)
				{
					// propagate accepting the same visitor to all the children
					// this effectively achieves Depth-First AST Traversal
					for (Node child : p_node.getChildren())
					{
						child.m_symtab = p_node.m_symtab;
						child.accept(this);
					}
				};

				public void visit(FuncCallNode p_node)
				{
					// propagate accepting the same visitor to all the children
					// this effectively achieves Depth-First AST Traversal
					for (Node child : p_node.getChildren())
						child.accept(this);
					String tempvarname = this.getNewTempVarName();
					p_node.m_moonVarName = tempvarname;
					String vartype = p_node.getType();
					p_node.m_symtabentry = new VarEntry("retval", vartype, p_node.m_moonVarName, new Vector<Integer>());
					p_node.m_symtab.addEntry(p_node.m_symtabentry);
				};

				public void visit(ReturnStatNode p_node)
				{
					// propagate accepting the same visitor to all the children
					// this effectively achieves Depth-First AST Traversal
					for (Node child : p_node.getChildren())
						child.accept(this);
				};*/
	}
}
