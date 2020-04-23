using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;
using TruCompiler.Nodes;
using TruCompiler.Semantic_Analyzer.SymbolTableClasses;

namespace TruCompiler.Semantic_Analyzer
{
    public class SymbolTableFirstRunVisitor : Visitor<Token>
	{

		public SymbolTableFirstRunVisitor()
		{
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

			node.Entry = new ClassEntry(className, localClassTable);
			
			node.SymbolTable.addEntry(node.Entry);
			node.SymbolTable = localClassTable;

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
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
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
						if (node.Entry != null)
						{
							localFunctionTable = node.Entry.SubTable;
						} else
						{
							Driver.SemanticErrors += String.Format("\nSemantic Error : Function {0} at line {1} is not declared in corresponding class {2}", head.FunctionName.IdValue, head.FunctionName.Value.Line, head.ClassName.IdValue);
							return;
						}
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
		}

		public override void visit(MultOpNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
		}

		public override void visit(NumNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
		}

		public override void visit(AssignStatementNode node)
		{
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
		}

		public override void visit(FunctionCallNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
		}

		public override void visit(ExprNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
		}
		public override void visit(ReturnStatementNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
		}

		public override void visit(WriteStatementNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
		}

		public override void visit(RelExprNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
		}

		public override void visit(VariableNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.SymbolTable = node.SymbolTable;
				child.accept(this);
			}
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
	}
}
