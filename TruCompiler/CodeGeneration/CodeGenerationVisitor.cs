using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Nodes;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;
using static TruCompiler.Driver;
using static TruCompiler.CodeGeneration.CodeGenerationHelper;
using TruCompiler.Semantic_Analyzer.SymbolTableClasses;

namespace TruCompiler.CodeGeneration
{
    public class CodeGenerationVisitor : Visitor<Token>
    {
        private static int _StatementNumID = 1;

        public static int StatementNumID
        {
            get { return _StatementNumID; }
            set { _StatementNumID = value; }
        }

        public override void visit(Node<Token> node)
        {
            foreach (var child in node.Children){child.accept(this);}
        }

        public override void visit(ProgNode node)
        {
            foreach (var child in node.Children){child.accept(this);}
        }

        public override void visit(ClassListNode node)
        {
            foreach (var child in node.Children){child.accept(this);}
        }

        public override void visit(ClassNode node)
        {
            foreach (var child in node.Children){child.accept(this);}
        }

        public override void visit(ClassMembersNode node)
        {
            foreach (var child in node.Children){child.accept(this);}
        }

        public override void visit(MemberNode node)
        {
            foreach (var child in node.Children){child.accept(this);}
        }

        public override void visit(FuncDeclNode node)
        {
            foreach (var child in node.Children){child.accept(this);}
        }

        public override void visit(VariableDeclNode node)
        {
            foreach (var child in node.Children){child.accept(this);}
            Driver.GeneratedCode["Data"] += CodeGenerationHelper.ReserveData(node.Entry); 
        }

        public override void visit(FuncDefsNode node)
        {
            foreach (var child in node.Children){child.accept(this);}
        }

        public override void visit(FuncDefNode node)
        {
            foreach (var child in node.Children){child.accept(this);}
        }

        public override void visit(AddOpNode node)
        {
            foreach (var child in node.Children){child.accept(this);}
            Driver.GeneratedCode["Data"] += CodeGenerationHelper.ReserveData(node.Entry);
            GeneratedCode["Program"] += AddOp(node.Left[0].Entry, node.Right[0].Entry, node.Operation.Value.Value, node.Entry);
        }

        public override void visit(FuncBodyNode node)
        {
            foreach (var child in node.Children){child.accept(this);}
        }

        public override void visit(MultOpNode node)
        {
            foreach (var child in node.Children){child.accept(this);}
            Driver.GeneratedCode["Data"] += CodeGenerationHelper.ReserveData(node.Entry);
            GeneratedCode["Program"] += MultOp(node.Left[0].Entry, node.Right[0].Entry, node.Operation.Value.Value, node.Entry);
        }

        public override void visit(NumNode node)
        {
            foreach (var child in node.Children){child.accept(this);}
            Driver.GeneratedCode["Data"] += CodeGenerationHelper.ReserveData(node.Entry);
            GeneratedCode["Program"] += StoreLiteralValue(node.Entry);
        }

        public override void visit(AssignStatementNode node)
        {
            foreach (var child in node.Children){child.accept(this);}
            Node<Token> right = node.Right;
            while (right.Entry == null)
            {
                right = right[0];
            }
            GeneratedCode["Program"] += AssignSides(node.Left.Entry, right.Entry);
        }

        public override void visit(ArithExprNode node)
        {
            foreach (var child in node.Children){child.accept(this);}
        }

        public override void visit(VariableNode node)
        {
            foreach (var child in node.Children) { child.accept(this); }
        }

        public override void visit(MainNode node)
        {
            GeneratedCode["Program"] += "%- Main Function -%\n";
            GeneratedCode["Program"] += "%- PROGRAM START -%\nentry\n";
            foreach (var child in node.Children){child.accept(this);}
            GeneratedCode["Program"] += "hlt\n%- PROGRAM END -%\n\n";
        }

        public override void visit(FunctionCallNode node)
        {
            foreach (var child in node.Children){child.accept(this);}
        }

        public override void visit(IdNode node)
        {
            foreach (var child in node.Children){child.accept(this);}
        }

        public override void visit(ReturnStatementNode node)
        {
            foreach (var child in node.Children){child.accept(this);}
        }

        public override void visit(WriteStatementNode node)
        {
            foreach (var child in node.Children){child.accept(this);}
            Node<Token> writeNode = node;
            while(writeNode.Entry == null)
            {
                writeNode = writeNode[0];
            }
            GeneratedCode["Program"] += WriteToConsole(writeNode.Entry);
        }

        public override void visit(RelExprNode node)
        {
            foreach (var child in node.Children){child.accept(this);}
            Entry left = null;
            Entry right = null;
            if (node.LeftArithExpr.Children.Count > 0)
            {
                left = node.LeftArithExpr[0].Entry;
            }
            if (node.RightArithExpr.Children.Count > 0)
            {
                right = node.RightArithExpr[0].Entry;
            }
            if (left != null && right != null)
            {
                GeneratedCode["Program"] += RelExpr(left, right, node.RelOp.Value.Value, node.Entry);
                Driver.GeneratedCode["Data"] += CodeGenerationHelper.ReserveData(node.Entry);
            }
        }

        public override void visit(IfStatementNode node)
        {
            string endthen = "endthen" + StatementNumID + "_" + SymbolTableVisitor.TempNum.ToString();
            SymbolTableVisitor.TempNum++;
            string endelse = "endelse" + StatementNumID + "_" + SymbolTableVisitor.TempNum.ToString();
            SymbolTableVisitor.TempNum++;
            StatementNumID++;
            if (node.Children.Count > 0)
            {
                node[0].accept(this);
                //Branch if then
                GeneratedCode["Program"] += BranchIf(node[0].Entry, endthen);
            }
            GeneratedCode["Program"] += "% Then branch starts here %\n\n";
            if (node.Children.Count > 1)
            {
                node[1].accept(this);
            }
            GeneratedCode["Program"] += "% Then branch ends here %\n\n";
            GeneratedCode["Program"] += String.Format("{0,-8}j {1}\n", "", endelse);
            GeneratedCode["Program"] += endthen + "\n\n";
            GeneratedCode["Program"] += "% Else branch starts here %\n\n";
            if (node.Children.Count > 2)
            {
                node[2].accept(this);
            }
            GeneratedCode["Program"] += "% Else branch ends here %\n\n";
            GeneratedCode["Program"] += endelse + "\n\n";
        }

        public override void visit(WhileStatementNode node)
        {
            string startwhile = "startwhile" + StatementNumID + "_" + SymbolTableVisitor.TempNum.ToString();
            SymbolTableVisitor.TempNum++;
            string endwhile = "endwhile" + StatementNumID + "_" + SymbolTableVisitor.TempNum.ToString();
            SymbolTableVisitor.TempNum++;
            StatementNumID++;
            if (node.Children.Count > 0)
            {
                GeneratedCode["Program"] += startwhile + "\n\n";
                node[0].accept(this);
                //Branch while if condition is false break else continue
                GeneratedCode["Program"] += BranchWhile(node[0].Entry, endwhile);

                GeneratedCode["Program"] += "% While body starts here %\n\n";
                if (node.Children.Count > 1)
                {
                    node[1].accept(this);
                }
                GeneratedCode["Program"] += "% While body ends here, loop back %\n\n";
                GeneratedCode["Program"] += String.Format("{0,-8}j {1}\n", "", startwhile);
                GeneratedCode["Program"] += endwhile + "\n\n";
            }
        }

        public override void visit(ReadStatementNode node)
        {
            foreach (var child in node.Children) { child.accept(this); }
            Node<Token> readNode = node;
            while (readNode.Entry == null)
            {
                readNode = readNode[0];
            }
            GeneratedCode["Program"] += ReadFromConsole(readNode.Entry);
        }
    }
}
