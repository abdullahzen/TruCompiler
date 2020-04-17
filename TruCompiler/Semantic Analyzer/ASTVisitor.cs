using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Lexical_Analyzer;
using TruCompiler.Nodes;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Semantic_Analyzer
{
    public class ASTVisitor : Visitor<Token>
    {
        public override void visit(Node<Token> node)
        {
            GenerateDiGraph(node);
        }

        public override void visit(ProgNode node)
        {
            GenerateDiGraph(node);

        }

        public override void visit(ClassListNode node)
        {
            GenerateDiGraph(node);
        }

        public override void visit(ClassNode node)
        {
            GenerateDiGraph(node);
        }

        public void GenerateDiGraph(Node<Token> node)
        {
            int parent = Driver.ASTIndex;
            if (node.Parent == null)
            {
                Driver.ASTResult[0] += String.Format("{0}[label=\"{1}\"]\n", Driver.ASTIndex, SyntacticalAnalyzer.GetValueFromNode(node.Value));
                Driver.ASTResult[1] += String.Format("{0}->{1}\n", Driver.ASTIndex, Driver.ASTIndex + 1);
            }
            foreach (var child in node.Children)
            {
                Driver.ASTIndex++;
                if (child.Value != null)
                {
                    Driver.ASTResult[0] += String.Format("{0}[label=\"{1}\"]\n", Driver.ASTIndex, SyntacticalAnalyzer.GetValueFromNode(child.Value));
                    Driver.ASTResult[1] += String.Format("{0}->{1}\n", parent, Driver.ASTIndex);
                }
                if (child.Children.Count > 0)
                {
                    child.accept(this);
                }
            }
        }

        public override void visit(ClassMembersNode node)
        {
            GenerateDiGraph(node);
        }

        public override void visit(MemberNode node)
        {
            GenerateDiGraph(node);
        }

        public override void visit(FuncDeclNode node)
        {
            GenerateDiGraph(node);
        }

        public override void visit(VariableDeclNode node)
        {
            GenerateDiGraph(node);
        }

        public override void visit(FuncDefsNode node)
        {
            GenerateDiGraph(node);
        }

        public override void visit(FuncDefNode node)
        {
            GenerateDiGraph(node);
        }

        public override void visit(IfStatementNode node)
        {
            GenerateDiGraph(node);
        }

        public override void visit(WhileStatementNode node)
        {
            GenerateDiGraph(node);
        }



        public override void visit(AddOpNode node)
        {
            GenerateDiGraph(node);
        }

        public override void visit(FuncBodyNode node)
        {
            GenerateDiGraph(node);
        }

        public override void visit(MultOpNode node)
        {
            GenerateDiGraph(node);
        }

        public override void visit(NumNode node)
        {
            GenerateDiGraph(node);
        }

        public override void visit(AssignStatementNode node)
        {
            GenerateDiGraph(node);
        }

        public override void visit(IdNode node)
        {
            GenerateDiGraph(node);
        }

        public override void visit(ReturnStatementNode node)
        {
            GenerateDiGraph(node);
        }

        public override void visit(WriteStatementNode node)
        {
            GenerateDiGraph(node);
        }

        public override void visit(RelExprNode node)
        {
            GenerateDiGraph(node);
        }

        public override void visit(ArithExprNode node)
        {
            GenerateDiGraph(node);
        }

        public override void visit(FunctionCallNode node)
        {
            GenerateDiGraph(node);
        }

        public override void visit(MainNode node)
        {
            GenerateDiGraph(node);
        }

        public override void visit(VariableNode node)
        {
            GenerateDiGraph(node);
        }

        public override void visit(ReadStatementNode node)
        {
            GenerateDiGraph(node);
        }
    }
}
