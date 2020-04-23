using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Nodes;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Semantic_Analyzer.SymbolTableClasses;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.CodeGeneration
{
    public class ComputeOffsetVisitor : Visitor<Token>
    {
        public override void visit(Node<Token> node)
        {
            foreach(Node<Token> child in node.Children){child.accept(this);}
        }

        public override void visit(ProgNode node)
        {
            foreach(Node<Token> child in node.Children){child.accept(this);}
        }

        public override void visit(ClassListNode node)
        {
            foreach(Node<Token> child in node.Children){child.accept(this);}
            foreach(var classNode in node.Classes)
            {
                if (classNode.InheritanceList != null && classNode.InheritanceList.Classes.Count > 0)
                {
                    foreach (var c in classNode.InheritanceList.Classes)
                    {
                        if (classNode.SymbolTable != null)
                        {
                            Entry ent = classNode.SymbolTable.SearchName(c.IdValue);
                            if (ent != null)
                            {
                                SymbolTable e = ent.SubTable;
                                if (e != null)
                                {
                                    classNode.SymbolTable.Offset -= e.Offset;
                                    classNode.SymbolTable.Size += e.Size;
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void visit(ClassNode node)
        {
            foreach(Node<Token> child in node.Children){child.accept(this);}
        }

        public override void visit(ClassMembersNode node)
        {
            foreach(Node<Token> child in node.Children){child.accept(this);}
            int offset = 0;
            
            foreach (Entry e in node.SymbolTable.SymList)
            {
                if (e.Kind != "function")
                {
                    e.Offset = offset;
                    offset = e.Offset - e.Size;
                }
            }
            node.SymbolTable.Offset = offset;
            node.SymbolTable.Size = offset * (-1);
        }

        public override void visit(MemberNode node)
        {
            foreach(Node<Token> child in node.Children){child.accept(this);}
            
        }

        public override void visit(FuncDeclNode node)
        {
            foreach(Node<Token> child in node.Children){child.accept(this);}
        }

        public override void visit(VariableDeclNode node)
        {
            foreach(Node<Token> child in node.Children){child.accept(this);}
        }

        public override void visit(FuncDefsNode node)
        {
            foreach(Node<Token> child in node.Children){child.accept(this);}
        }

        public override void visit(FuncDefNode node)
        {
            foreach(Node<Token> child in node.Children){child.accept(this);}
            int offset = 0;
            if (node.Entry != null)
            {
                if (node.Entry.Type != "void")
                {
                    if (node.Entry.Type == "integer" || node.Entry.Type == "float")
                    {
                        node.Entry.Offset = offset;
                        offset -= node.Entry.Size;
                    }
                    else if (node.Entry.Type != null)
                    {
                        if (node.SymbolTable.SearchName(node.Entry.Type) != null)
                        {
                            node.Entry.Size = node.SymbolTable.SearchName(node.Entry.Type).SubTable.Size;
                            node.Entry.Offset = offset;
                            offset -= node.Entry.Size;
                        }
                    }
                }
                foreach (Entry e in node.SymbolTable.SymList)
                {
                    e.Offset = offset;
                    offset = e.Offset - e.Size;
                }
                node.SymbolTable.Offset = offset;
                node.SymbolTable.Size = offset * (-1);
            }
        }

        public override void visit(AddOpNode node)
        {
            foreach(Node<Token> child in node.Children){child.accept(this);}
        }

        public override void visit(FuncBodyNode node)
        {
            foreach(Node<Token> child in node.Children){child.accept(this);}
        }

        public override void visit(MultOpNode node)
        {
            foreach(Node<Token> child in node.Children){child.accept(this);}
        }

        public override void visit(NumNode node)
        {
            foreach(Node<Token> child in node.Children){child.accept(this);}
        }

        public override void visit(AssignStatementNode node)
        {
            foreach(Node<Token> child in node.Children){child.accept(this);}
        }

        public override void visit(ArithExprNode node)
        {
            foreach(Node<Token> child in node.Children){child.accept(this);}
        }

        public override void visit(MainNode node)
        {
            foreach (Node<Token> child in node.Children) { child.accept(this); }
            int offset = -4;
            if (node.Entry != null)
            {
                foreach (Entry e in node.SymbolTable.SymList)
                {
                    e.Offset = offset;
                    offset = e.Offset - e.Size;
                }
                node.SymbolTable.Offset = offset;
                node.SymbolTable.Size = offset * (-1);
            }
        }

        public override void visit(FunctionCallNode node)
        {
            foreach(Node<Token> child in node.Children){child.accept(this);}
        }

        public override void visit(IdNode node)
        {
            foreach(Node<Token> child in node.Children){child.accept(this);}
        }

        public override void visit(ReturnStatementNode node)
        {
            foreach(Node<Token> child in node.Children){child.accept(this);}
        }

        public override void visit(WriteStatementNode node)
        {
            foreach(Node<Token> child in node.Children){child.accept(this);}
        }

        public override void visit(RelExprNode node)
        {
            foreach(Node<Token> child in node.Children){child.accept(this);}
        }

        public override void visit(VariableNode node)
        {
            foreach(Node<Token> child in node.Children){child.accept(this);}
        }

        public override void visit(ReadStatementNode node)
        {
            foreach(Node<Token> child in node.Children){child.accept(this);}
        }

        public override void visit(IfStatementNode node)
        {
            foreach(Node<Token> child in node.Children){child.accept(this);}
        }

        public override void visit(WhileStatementNode node)
        {
            foreach(Node<Token> child in node.Children){child.accept(this);}
        }

        public override void visit(ExprNode node)
        {
            foreach (Node<Token> child in node.Children) { child.accept(this); }
        }

    }
}
