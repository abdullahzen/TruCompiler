using System;
using System.Collections.Generic;
using System.Text;
using TruCompiler.Nodes;
using TruCompiler.Syntactical_Analyzer;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Semantic_Analyzer
{
    public abstract class Visitor<T>
    {
        public abstract void visit(Node<T> node);
        public abstract void visit(ProgNode node);
        public abstract void visit(ClassListNode node);
        public abstract void visit(ClassNode node);
        public abstract void visit(ClassMembersNode node);
        public abstract void visit(MemberNode node);
        public abstract void visit(FuncDeclNode node);
        public abstract void visit(VariableDeclNode node);
        public abstract void visit(FuncDefsNode node);
        public abstract void visit(FuncDefNode node);

        public abstract void visit(AddOpNode node);
        public abstract void visit(FuncBodyNode node);
        public abstract void visit(MultOpNode node);
        public abstract void visit(NumNode node);
        public abstract void visit(AssignStatementNode node);
        public abstract void visit(ArithExprNode node);
        public abstract void visit(MainNode node);
        public abstract void visit(FunctionCallNode node);
        public abstract void visit(IdNode node);
        public abstract void visit(ReturnStatementNode node);
        public abstract void visit(WriteStatementNode node);
        public abstract void visit(RelExprNode node);
        public abstract void visit(VariableNode node);
        public abstract void visit(ReadStatementNode node);
        public abstract void visit(IfStatementNode node);

        public abstract void visit(WhileStatementNode node);

        public abstract void visit(ExprNode node);
    }
}
