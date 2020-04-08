using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;
using TruCompiler.Nodes;
using TruCompiler.Semantic_Analyzer.SymbolTableClasses;

namespace TruCompiler.Semantic_Analyzer
{
	public class TypeCheckingVisitor : Visitor<Token>
	{

		public TypeCheckingVisitor()
		{
		}

		public override void visit(AssignStatementNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.accept(this);
			}
			
			String leftType = node.Left.Type;
			String rightType = node.Right.Type;
			if (String.IsNullOrEmpty(leftType))
			{
				node.Left.Type = GetVariableType(node.Left, node);
				leftType = node.Left.Type;
			}
			if (String.IsNullOrEmpty(rightType))
			{
				if (node.Right.ArithExpr != null)
				{
					node.Right.Type = GetType(node.Right.ArithExpr);
					rightType = node.Right.Type;
				}
			}

			if (leftType == rightType)
				node.Type = leftType;
			else
			{
				Driver.SemanticErrors += String.Format("\nSemantic Error : AssignmentStatment error left expression and right expression are not the same type at line {0}", node.Value.Line);
				return;
			}
		}

		public override void visit(RelExprNode node)
		{
			foreach (Node<Token> child in node.Children)
			{
				child.accept(this);
			}
			String leftType = node.LeftArithExpr.Type;
			String rightType = node.RightArithExpr.Type;
			//if (String.IsNullOrEmpty(leftType))
			//{
				node.LeftArithExpr.Type = GetType(node.LeftArithExpr);
				leftType = node.LeftArithExpr.Type;
			//}
			/*if (String.IsNullOrEmpty(rightType))
			{*/
				if (node.RightArithExpr != null)
				{
					node.RightArithExpr.Type = GetType(node.RightArithExpr);
					rightType = node.RightArithExpr.Type;
				}
			//}

			if (leftType == rightType)
				node.Type = leftType;
			else
			{
				Driver.SemanticErrors += String.Format("\nSemantic Error : RelExpr error left expression and right expression are not the same type at line {0}", node.Value.Line);
				return;
			}
		}

		public override void visit(AddOpNode node)
		{
			foreach (Node<Token> child in node.Children)
			{

				child.accept(this);
			}

			String leftType = node.Left.Type;
			String rightType = node.Right.Type;
			/*if (String.IsNullOrEmpty(leftType))
			{*/
				node.Left.Type = GetType((ArithExprNode)node.Left);
				leftType = node.Left.Type;
			//}
			//if (String.IsNullOrEmpty(rightType))
			//{
				if (node.Right != null)
				{
					node.Right.Type = GetType((ArithExprNode)node.Right);
					rightType = node.Right.Type;
				}
			//}

			if (leftType == rightType)
				node.Type = leftType;
			else
			{
				Driver.SemanticErrors += String.Format("\nSemantic Error : AddOp error left expression and right expression are not the same type at line {0}", node.Value.Line);
				return;
			}

		}

		public override void visit(MultOpNode node)
		{
			foreach (Node<Token> child in node.Children)
			{

				child.accept(this);
			}

			String leftType = node.Left.Type;
			String rightType = node.Right.Type;
			/*if (String.IsNullOrEmpty(leftType))
			{*/
				node.Left.Type = GetType((ArithExprNode)node.Left);
				leftType = node.Left.Type;
			/*}
			if (String.IsNullOrEmpty(rightType))
			{*/
				if (node.Right != null)
				{
					node.Right.Type = GetType((ArithExprNode)node.Right);
					rightType = node.Right.Type;
				}
			//}

			if (leftType == rightType)
				node.Type = leftType;
			else
			{
				Driver.SemanticErrors += String.Format("\nSemantic Error : MultOp error left expression and right expression are not the same type at line {0}", node.Value.Line);
				return;
			}
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
			foreach (Node<Token> child in node.Children)
			{

				child.accept(this);
			}
		}


		public override void visit(ClassListNode node)
		{
			foreach (Node<Token> child in node.Children)
			{

				child.accept(this);
			}

		}

		public override void visit(ClassNode node)
		{
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
			foreach (Node<Token> child in node.Children)
			{

				child.accept(this);
			}
		}

		public override void visit(VariableDeclNode node)
		{
			foreach (Node<Token> child in node.Children)
			{

				child.accept(this);
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
			foreach (Node<Token> child in node.Children)
			{

				child.accept(this);
			}
		}


		public override void visit(FuncBodyNode node)
		{
			foreach (Node<Token> child in node.Children)
			{

				child.accept(this);
			}
		}
		

		public override void visit(NumNode node)
		{
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
		}

		public override void visit(FunctionCallNode node)
		{
			foreach (Node<Token> child in node.Children)
			{

				child.accept(this);
			}
		}

		public override void visit(ReturnStatementNode node)
		{
			foreach (Node<Token> child in node.Children)
			{

				child.accept(this);
			}
		}

		public override void visit(WriteStatementNode node)
		{
			foreach (Node<Token> child in node.Children)
			{

				child.accept(this);
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
					////Driver.SemanticErrors += String.Format("\nSemantic Error : Variable {0} at line {1} is not defined in the current scope", variable.Name, node.Value.Line);
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
								////Driver.SemanticErrors += String.Format("\nSemantic Error : {0} is not a member of the object {1} at line {2}", ((IdNode)subvars[index]).IdValue, firstVar, node.Value.Line);
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
						//Driver.SemanticErrors += String.Format("\nSemantic Error : Variable {0} at line {1} is not defined in the current scope", firstVar, node.Value.Line);
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
								//Driver.SemanticErrors += String.Format("\nSemantic Error : {0} is not a member of the object {1} at line {2}", ((IdNode)variable[index]).IdValue, firstVar, node.Value.Line);
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
						//Driver.SemanticErrors += String.Format("\nSemantic Error : Variable {0} at line {1} is not defined in the current scope", firstVar, node.Value.Line);
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
					//Driver.SemanticErrors += String.Format("\nSemantic Error : Call to undeclared Function {0} at line {1}", functionCall.Name.IdValue, functionCall.Name.Value.Line);
					type = "invalidType";
				}
			}
			else
			{
				//Driver.SemanticErrors += String.Format("\nSemantic Error : Call to undeclared Function {0} at line {1}", functionCall.Name.IdValue, functionCall.Name.Value.Line);
				type = "invalidType";
			}
			return type;
		}
	}
}
