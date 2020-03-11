using System;
using System.Collections.Generic;
using System.Text;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Syntactical_Analyzer
{
    public class Rules
    {
        public TokenScanner TokenScanner { get; set; }

        public Rules(TokenScanner tokenScanner)
        {
            TokenScanner = tokenScanner;
        }

        //===========================================//
        //              Recursive Rules
        //===========================================//
        public TreeNode<Token> Start()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            node.Value = new Token(Lexeme.keyword, "Start");
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "class")))
            {
                node = node.AddChild(Prog());
                return node;
            } else if (lookahead.Equals(new Token(Lexeme.id)))
            {
                node = node.AddChild(Prog());
                return node;
            } else if (lookahead.Equals(new Token(Lexeme.keyword, "main")))
            {
                node = node.AddChild(Prog());
                return node;
            }
              return node; 
        }

        public TreeNode<Token> Prog()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            node.Value = new Token(Lexeme.keyword, "Program");
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "class")))
            {
                TreeNode<Token> classes = new TreeNode<Token>(new Token(Lexeme.keyword, "Classes"));
                classes = classes.AddChild(Rept_Prog0());
                node = node.AddChild(classes);
                TreeNode<Token> funcdef = new TreeNode<Token>();
                funcdef.Value = new Token(Lexeme.keyword, "FunctionDefinitions");
                funcdef = funcdef.AddChild(Rept_Prog1());
                node = node.AddChild(funcdef);
                TreeNode<Token> main = new TreeNode<Token>(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.keyword, "main"));
                main = main.AddChild(FuncBody());
                node = node.AddChild(main);
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.id)))
            {
                TreeNode<Token> funcdef = new TreeNode<Token>();
                funcdef.Value = new Token(Lexeme.keyword, "FunctionDefinitions");
                funcdef = funcdef.AddChild(Rept_Prog1());
                node = node.AddChild(funcdef);
                TreeNode<Token> main = new TreeNode<Token>(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.keyword, "main"));
                main = main.AddChild(FuncBody());
                node = node.AddChild(main);
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "main")))
            {
                TreeNode<Token> main = new TreeNode<Token>(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.keyword, "main"));
                main = main.AddChild(FuncBody());
                node = node.AddChild(main);
                return node;
            } else
            {
                //false
                  return node;
            }
        }

        public TreeNode<Token> FuncBody()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            node.Value = new Token(Lexeme.keyword, "FuncBody");
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "do")))
            {
                //node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.keyword, "do"));
                node = node.AddChild(Rept_FuncBody2());
                //node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.keyword, "end"));
                return node;
            } else if (lookahead.Equals(new Token(Lexeme.keyword, "local")))
            {
                node = node.AddChild(Opt_FuncBody0());
                //node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.keyword, "do"));
                node = node.AddChild(Rept_FuncBody2());
                //node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.keyword, "end"));
                return node;
            } else
            {
                //false
                  return node;
            }
        }

        public TreeNode<Token> Rept_FuncBody2()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "if")))
            {
                node = node.AddChild(Statement());
                node = node.AddChild(Rept_FuncBody2());
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "while")))
            {
                node = node.AddChild(Statement());
                node = node.AddChild(Rept_FuncBody2());
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "read")))
            {
                node = node.AddChild(Statement());
                node = node.AddChild(Rept_FuncBody2());
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "write")))
            {
                node = node.AddChild(Statement());
                node = node.AddChild(Rept_FuncBody2());
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "return")))
            {
                node = node.AddChild(Statement());
                node = node.AddChild(Rept_FuncBody2());
            }
            else if (lookahead.Equals(new Token(Lexeme.id)))
            {
                node = node.AddChild(Statement());
                node = node.AddChild(Rept_FuncBody2());
            }
            else
            {
                //true
                return node;
            }
            return node;
        }

        public TreeNode<Token> Statement()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            node.Value = new Token(Lexeme.keyword, "Statement");
            if (lookahead.Equals(new Token(Lexeme.keyword, "if")))
            {
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.keyword, "if"));
                Match(new Token(Lexeme.openpar));
                node = node.AddChild(ArithExpr());
                node = node.AddChild(RelExpr());
                Match(new Token(Lexeme.closepar));
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.keyword, "then"));
                node = node.AddChild(StatBlock());
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.keyword, "else"));
                node = node.AddChild(StatBlock());
                Match(new Token(Lexeme.semi));
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "while")))
            {
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.keyword, "while"));
                Match(new Token(Lexeme.openpar));
                node = node.AddChild(ArithExpr());
                node = node.AddChild(RelExpr());
                Match(new Token(Lexeme.closepar));
                node = node.AddChild(StatBlock());
                Match(new Token(Lexeme.semi));
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "read")))
            {
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.keyword, "read"));
                Match(new Token(Lexeme.openpar));
                node = node.AddChild(Variable());
                Match(new Token(Lexeme.closepar));
                Match(new Token(Lexeme.semi));
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "write")))
            {
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.keyword, "write"));
                Match(new Token(Lexeme.openpar));
                node = node.AddChild(Expr());
                Match(new Token(Lexeme.closepar));
                Match(new Token(Lexeme.semi));
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "return")))
            {
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.keyword, "return"));
                Match(new Token(Lexeme.openpar));
                node = node.AddChild(Expr());
                Match(new Token(Lexeme.closepar));
                Match(new Token(Lexeme.semi));
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.id)))
            {
                node = node.AddChild(FunctionCallOrAssignment());
                return node;
            }
            else
            {
                //false
                  return node;
            }
        }

        public TreeNode<Token> FunctionCallOrAssignment()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.id)))
            {
                node.Value = TokenScanner.Peek().Clone();
                Match(new Token(Lexeme.id));
                node = node.AddChild(Idnest());
                node = node.AddChild(MoreMemberCalls());
                node = node.AddChild(FunctionEndOrAssignment());
                Match(new Token(Lexeme.semi));
                return node;
            } else
            {
                //false
                return node;
            }
        }

        public TreeNode<Token> FunctionEndOrAssignment()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            TreeNode<Token> assign = new TreeNode<Token>(new Token(Lexeme.keyword, "Assign"));
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.eq)))
            {
                AssignOp();
                assign = assign.AddChild(Expr());
                node = node.AddChild(assign);
                return node;
            }
            else
            {
                //true
                return node;
            }
        }

        public TreeNode<Token> MoreMemberCalls()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            TreeNode<Token> dot = new TreeNode<Token>(new Token(Lexeme.dot));
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.dot)))
            {
                //node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.dot));

                dot = dot.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.id));
                dot = dot.AddChild(Idnest());
                dot = dot.AddChild(MoreMemberCalls());
                node = node.AddChild(dot);
                return node;
            }
            else
            {
                //true
                return node;
            }
        }

        public TreeNode<Token> StatBlock()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            node.Value = new Token(Lexeme.keyword, "StatBlock");
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "do")))
            {
                //node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.keyword, "do"));
                node = node.AddChild(Rept_StatBlock1());
                //node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.keyword, "end"));
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "if")))
            {
                node = node.AddChild(Statement());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "while")))
            {
                node = node.AddChild(Statement());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "read")))
            {
                node = node.AddChild(Statement());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "write")))
            {
                node = node.AddChild(Statement());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "return")))
            {
                node = node.AddChild(Statement());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.id)))
            {
                node = node.AddChild(Statement());
                return node;
            }
            else
            {
                //true
                return node;
            }
        }

        public TreeNode<Token> Rept_StatBlock1()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "if")))
            {
                node = node.AddChild(Statement());
                node = node.AddChild(Rept_StatBlock1());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "while")))
            {
                node = node.AddChild(Statement());
                node = node.AddChild(Rept_StatBlock1());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "read")))
            {
                node = node.AddChild(Statement());
                node = node.AddChild(Rept_StatBlock1());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "write")))
            {
                node = node.AddChild(Statement());
                node = node.AddChild(Rept_StatBlock1());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "return")))
            {
                node = node.AddChild(Statement());
                node = node.AddChild(Rept_StatBlock1());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.id)))
            {
                node = node.AddChild(Statement());
                node = node.AddChild(Rept_StatBlock1());
                return node;
            }
            else
            {
                //true
                return node;
            }
        }

        public TreeNode<Token> AssignOp()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.eq)))
            {
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.eq));
                return node;
            }
            else
            {
                //false
                  return node;
            }
        }

        public TreeNode<Token> ArithExpr()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            node.Value = new Token(Lexeme.keyword, "ArithExpr");
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.intnum)))
            {
                node = node.AddChild(Term());
                node = node.AddChild(Rightrec_ArithExpr());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.floatnum)))
            {
                node = node.AddChild(Term());
                node = node.AddChild(Rightrec_ArithExpr());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.openpar)))
            {
                node = node.AddChild(Term());
                node = node.AddChild(Rightrec_ArithExpr());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "not")))
            {
                node = node.AddChild(Term());
                node = node.AddChild(Rightrec_ArithExpr());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.id)))
            {
                node = node.AddChild(Term());
                node = node.AddChild(Rightrec_ArithExpr());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.plus)))
            {
                node = node.AddChild(Term());
                node = node.AddChild(Rightrec_ArithExpr());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.minus)))
            {
                node = node.AddChild(Term());
                node = node.AddChild(Rightrec_ArithExpr());
                return node;
            }
            else
            {
                //false
                  return node;
            }
        }

        public TreeNode<Token> Rightrec_ArithExpr()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.plus)))
            {
                node = node.AddChild(AddOp());
                node = node.AddChild(Term());
                node = node.AddChild(Rightrec_ArithExpr());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.minus)))
            {
                node = node.AddChild(AddOp());
                node = node.AddChild(Term());
                node = node.AddChild(Rightrec_ArithExpr());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "or")))
            {
                node = node.AddChild(AddOp());
                node = node.AddChild(Term());
                node = node.AddChild(Rightrec_ArithExpr());
                return node;
            }
            else
            {
                //true
                return node;
            }
        }

        public TreeNode<Token> AddOp()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.plus)))
            {
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.plus));
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.minus)))
            {
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.minus));
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "or")))
            {
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.keyword, "or"));
                return node;
            }
            else
            {
                //false
                  return node;
            }
        }

        public TreeNode<Token> Term()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.intnum)))
            {
                node = node.AddChild(Factor());
                node = node.AddChild(Rightrec_Term());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.floatnum)))
            {
                node = node.AddChild(Factor());
                node = node.AddChild(Rightrec_Term());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.openpar)))
            {
                node = node.AddChild(Factor());
                node = node.AddChild(Rightrec_Term());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "not")))
            {
                node = node.AddChild(Factor());
                node = node.AddChild(Rightrec_Term());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.id)))
            {
                node = node.AddChild(Factor());
                node = node.AddChild(Rightrec_Term());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.plus)))
            {
                node = node.AddChild(Factor());
                node = node.AddChild(Rightrec_Term());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.minus)))
            {
                node = node.AddChild(Factor());
                node = node.AddChild(Rightrec_Term());
                return node;
            }
            else
            {
                //false
                  return node;
            }
        }

        public TreeNode<Token> Rightrec_Term()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.mult)))
            {
                node = node.AddChild(MultOp());
                node = node.AddChild(Factor());
                node = node.AddChild(Rightrec_Term());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.div)))
            {
                node = node.AddChild(MultOp());
                node = node.AddChild(Factor());
                node = node.AddChild(Rightrec_Term());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "and")))
            {
                node = node.AddChild(MultOp());
                node = node.AddChild(Factor());
                node = node.AddChild(Rightrec_Term());
                return node;
            } else
            {
                //true
                return node;
            }
        }

        public TreeNode<Token> MultOp()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.mult)))
            {
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.mult));
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.div)))
            {
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.div));
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "and")))
            {
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.keyword, "and"));
                return node;
            }
            else
            {
                //false
                  return node;
            }
        }

        public TreeNode<Token> Factor()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.intnum)))
            {
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.intnum));
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.floatnum)))
            {
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.floatnum));
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.openpar)))
            {
                Match(new Token(Lexeme.openpar));
                node = node.AddChild(ArithExpr());
                Match(new Token(Lexeme.closepar));
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "not")))
            {
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.keyword, "not"));
                node = node.AddChild(Factor());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.id)))
            {
                node = node.AddChild(FunctionCallOrVariable());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.plus)))
            {
                node = node.AddChild(Sign());
                node = node.AddChild(Factor());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.minus)))
            {
                node = node.AddChild(Sign());
                node = node.AddChild(Factor());
                return node;
            }
            else
            {
                //false
                  return node;
            }
        }

        public TreeNode<Token> FunctionCallOrVariable()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.id)))
            {
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.id));
                node = node.AddChild(Idnest());
                node = node.AddChild(MoreMemberCalls());
                return node;
            }
            else
            {
                //false
                return node;
            }
        }

        public TreeNode<Token> Variable()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.id)))
            {
                node.Value = new Token(Lexeme.keyword, "Variable");
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.id));
                node = node.AddChild(Idnest_VariableOnly());
                node = node.AddChild(MoreVariableCalls());
                return node;
            }
            else
            {
                //false
                  return node;
            }
        }

        public TreeNode<Token> MoreVariableCalls()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            TreeNode<Token> dot = new TreeNode<Token>(new Token(Lexeme.dot));
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.dot)))
            {
                //node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.dot));
                dot = dot.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.id));
                dot = dot.AddChild(Idnest_VariableOnly());
                dot = dot.AddChild(MoreVariableCalls());
                node = node.AddChild(dot);
                return node;
            }
            else
            {
                //true
                return node;
            }
        }

        public TreeNode<Token> Idnest_VariableOnly()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.id)))
            {
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.id));
                node = node.AddChild(Rept_Indexing());
                return node;
            }
            else
            {
                //false
                return node;
            }
        }

        public TreeNode<Token> Sign()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            node.Value = new Token(Lexeme.keyword, "Sign");
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.plus)))
            {
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.plus));
                return node;
            } else if (lookahead.Equals(new Token(Lexeme.minus)))
            {
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.minus));
                return node;
            }
            else
            {
                //false
                  return node;
            }
        }

        public TreeNode<Token> Idnest()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.openpar)))
            {
                node = node.AddChild(IdnestTail());
                return node;
            } else if (lookahead.Equals(new Token(Lexeme.opensqbr)))
            {
                node = node.AddChild(IdnestTail());
                return node;
            }
            else
            {
               //false
                return node;
            }
        }

        public TreeNode<Token> IdnestTail()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.openpar)))
            {
                Match(new Token(Lexeme.openpar));
                node = node.AddChild(AParams());
                Match(new Token(Lexeme.closepar));
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.opensqbr)))
            {
                node = node.AddChild(Rept_Indexing());
                return node;
            }
            else
            {
                //false
                return node;
            }
        }

        public TreeNode<Token> Rept_Indexing()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.opensqbr)))
            {
                Match(new Token(Lexeme.opensqbr));
                node = node.AddChild(ArithExpr());
                Match(new Token(Lexeme.closesqbr));
                node = node.AddChild(Rept_Indexing());
                return node;
            }
            else
            {
                //true
                return node;
            }
        }

        public TreeNode<Token> AParams()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            node.Value = new Token(Lexeme.keyword, "AParams");
            if (lookahead.Equals(new Token(Lexeme.intnum)))
            {
                node = node.AddChild(Expr());
                node = node.AddChild(Rept_AParams1());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.floatnum)))
            {
                node = node.AddChild(Expr());
                node = node.AddChild(Rept_AParams1());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.openpar)))
            {
                node = node.AddChild(Expr());
                node = node.AddChild(Rept_AParams1());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "not")))
            {
                node = node.AddChild(Expr());
                node = node.AddChild(Rept_AParams1());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.id)))
            {
                node = node.AddChild(Expr());
                node = node.AddChild(Rept_AParams1());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.plus)))
            {
                node = node.AddChild(Expr());
                node = node.AddChild(Rept_AParams1());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.minus)))
            {
                node = node.AddChild(Expr());
                node = node.AddChild(Rept_AParams1());
                return node;
            }
            else
            {
                //true
                return node;
            }
        }

        public TreeNode<Token> Rept_AParams1()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.comma)))
            {
                node = node.AddChild(AParamsTail());
                node = node.AddChild(Rept_AParams1());
                return node;
            } else
            {
                //true
                return node;
            }
        }

        public TreeNode<Token> AParamsTail()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.comma)))
            {
                Match(new Token(Lexeme.comma));
                node = node.AddChild(Expr());
                return node;
            }
            else
            {
                //false
                  return node;
            }
        }

        public TreeNode<Token> Expr()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            node.Value = new Token(Lexeme.keyword, "Expr");
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.intnum)))
            {
                node = node.AddChild(ArithExpr());
                node = node.AddChild(AfterExpr());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.floatnum)))
            {
                node = node.AddChild(ArithExpr());
                node = node.AddChild(AfterExpr());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.openpar)))
            {
                node = node.AddChild(ArithExpr());
                node = node.AddChild(AfterExpr());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "not")))
            {
                node = node.AddChild(ArithExpr());
                node = node.AddChild(AfterExpr());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.id)))
            {
                node = node.AddChild(ArithExpr());
                node = node.AddChild(AfterExpr());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.plus)))
            {
                node = node.AddChild(ArithExpr());
                node = node.AddChild(AfterExpr());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.minus)))
            {
                node = node.AddChild(ArithExpr());
                node = node.AddChild(AfterExpr());
                return node;
            }
            else
            {
                //false
                  return node;
            }
        }

        public TreeNode<Token> AfterExpr()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.eq)))
            {
                node = node.AddChild(RelExpr());
                return node;
            } else if (lookahead.Equals(new Token(Lexeme.noteq)))
            {
                node = node.AddChild(RelExpr());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.lt)))
            {
                node = node.AddChild(RelExpr());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.gt)))
            {
                node = node.AddChild(RelExpr());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.leq)))
            {
                node = node.AddChild(RelExpr());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.geq)))
            {
                node = node.AddChild(RelExpr());
                return node;
            }
            else 
            {
                //true
                return node;
            }
        }

        public TreeNode<Token> RelExpr()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.eq)))
            {
                node = node.AddChild(RelOp());
                node = node.AddChild(ArithExpr());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.noteq)))
            {
                node = node.AddChild(RelOp());
                node = node.AddChild(ArithExpr());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.lt)))
            {
                node = node.AddChild(RelOp());
                node = node.AddChild(ArithExpr());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.gt)))
            {
                node = node.AddChild(RelOp());
                node = node.AddChild(ArithExpr());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.leq)))
            {
                node = node.AddChild(RelOp());
                node = node.AddChild(ArithExpr());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.geq)))
            {
                node = node.AddChild(RelOp());
                node = node.AddChild(ArithExpr());
                return node;
            }
            else
            {
                //false
                  return node;
            }
        }

        public TreeNode<Token> RelOp()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.eq)))
            {
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.eq));
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.noteq)))
            {
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.noteq));
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.lt)))
            {
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.lt));
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.gt)))
            {
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.gt));
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.leq)))
            {
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.leq));
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.geq)))
            {
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.geq));
                return node;
            }
            else
            {
                //false
                  return node;
            }
        }

        public TreeNode<Token> Opt_FuncBody0()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "local")))
            {
                node.Value = TokenScanner.Peek().Clone();
                Match(new Token(Lexeme.keyword, "local"));
                node = node.AddChild(Rept_Opt_FuncBody01());
                return node;
            } else
            {
                //true
                return node;
            }
        }

        public TreeNode<Token> Rept_Opt_FuncBody01()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            TreeNode<Token> variable = new TreeNode<Token>(new Token(Lexeme.keyword, "Variable"));
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "integer")))
            {
                variable = variable.AddChild(Before_VarDeclNotId());
                variable = variable.AddChild(VarDecl());
                node = node.AddChild(variable);
                lookahead = TokenScanner.Peek();
                if (lookahead.Equals(new Token(Lexeme.keyword, "integer")))
                {
                    node = node.AddChild(Rept_Opt_FuncBody01());
                    return node;
                } else if (lookahead.Equals(new Token(Lexeme.keyword, "float")))
                {
                    node = node.AddChild(Rept_Opt_FuncBody01());
                    return node;
                } else if (lookahead.Equals(new Token(Lexeme.id)))
                {
                    node = node.AddChild(Rept_Opt_FuncBody01());
                    return node;
                }
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "float")))
            {
                variable = variable.AddChild(Before_VarDeclNotId());
                variable = variable.AddChild(VarDecl());
                node = node.AddChild(variable);
                lookahead = TokenScanner.Peek();
                if (lookahead.Equals(new Token(Lexeme.keyword, "integer")))
                {
                    node = node.AddChild(Rept_Opt_FuncBody01());
                    return node;
                }
                else if (lookahead.Equals(new Token(Lexeme.keyword, "float")))
                {
                    node = node.AddChild(Rept_Opt_FuncBody01());
                    return node;
                }
                else if (lookahead.Equals(new Token(Lexeme.id)))
                {
                    node = node.AddChild(Rept_Opt_FuncBody01());
                    return node;
                }
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.id)))
            {
                variable = variable.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.id));
                variable = variable.AddChild(VarDecl());
                node = node.AddChild(variable);
                lookahead = TokenScanner.Peek();
                if (lookahead.Equals(new Token(Lexeme.keyword, "integer")))
                {
                    node = node.AddChild(Rept_Opt_FuncBody01());
                    return node;
                }
                else if (lookahead.Equals(new Token(Lexeme.keyword, "float")))
                {
                    node = node.AddChild(Rept_Opt_FuncBody01());
                    return node;
                }
                else if (lookahead.Equals(new Token(Lexeme.id)))
                {
                    node = node.AddChild(Rept_Opt_FuncBody01());
                    return node;
                }
                return node;
            }
            else
            {
                //true
                return node;
            }
        }

        public TreeNode<Token> Rept_Prog0()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "class")))
            {
                node = node.AddChild(ClassDecl());
                lookahead = TokenScanner.Peek();
                if (lookahead.Equals(new Token(Lexeme.keyword, "class")))
                {
                    node = node.AddChild(Rept_Prog0());
                }
                return node;
            } else
            {
                //true
                return node;
            }
        }

        public TreeNode<Token> Rept_Prog1()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            TreeNode<Token> function = new TreeNode<Token>(new Token(Lexeme.keyword, "Function"));
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.id)))
            {
                function = function.AddChild(FuncDef());
                node = node.AddChild(function);
                lookahead = TokenScanner.Peek();
                if (lookahead.Equals(new Token(Lexeme.id)))
                {
                    node = node.AddChild(Rept_Prog1());
                }
                return node;
            }
            else
            {
                //true
                return node;
            }
        }

        public TreeNode<Token> FuncDef()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.id)))
            {
                node = node.AddChild(FuncHead());
                node = node.AddChild(FuncBody());
                Match(new Token(Lexeme.semi));
                return node;
            }
            else
            {
                //false
                  return node;
            }
        }

        public TreeNode<Token> FuncHead()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.id)))
            {
                node.Value = new Token(Lexeme.keyword, "FuncHead");
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.id));
                Opt_ClassDecl2_FuncHead0();
                Match(new Token(Lexeme.openpar));
                node = node.AddChild(FParams());
                Match(new Token(Lexeme.closepar));
                Match(new Token(Lexeme.colon));
                node = node.AddChild(AfterFuncHead());
                return node;
            }
            else
            {
                //false
                  return node;
            }
        }

        public TreeNode<Token> AfterFuncHead()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "void")))
            {
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.keyword, "void"));
                return node;
            } else if (lookahead.Equals(new Token(Lexeme.keyword, "integer")))
            {
                node = node.AddChild(Type());
            } else if (lookahead.Equals(new Token(Lexeme.keyword, "float")))
            {
                node = node.AddChild(Type());
            } else if (lookahead.Equals(new Token(Lexeme.id)))
            {
                node = node.AddChild(Type());
            } else
            {
                //false
                  return node;
            }
            return node;
        }

        public bool Opt_ClassDecl2_FuncHead0()
        {
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.coloncolon))) {
                Match(new Token(Lexeme.coloncolon));
                Match(new Token(Lexeme.id));
                return true;
            } else
            {
                return true;
            }
        }

        public TreeNode<Token> ClassDecl()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "class")))
            {
                node = new TreeNode<Token>(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.keyword, "class"));
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.id));
                if (TokenScanner.Peek().Equals(new Token(Lexeme.keyword, "inherits")))
                {
                    TreeNode<Token> inheritanceList = new TreeNode<Token>(new Token(Lexeme.keyword, "InheritanceList"));
                    inheritanceList = inheritanceList.AddChild(Opt_ClassDecl2());
                    node = node.AddChild(inheritanceList);
                }
                Match(new Token(Lexeme.opencbr));
                TreeNode<Token> classMembers = new TreeNode<Token>(new Token(Lexeme.keyword, "ClassMembers"));
                classMembers = classMembers.AddChild(Rept_ClassDecl4());
                node = node.AddChild(classMembers);
                Match(new Token(Lexeme.closecbr));
                Match(new Token(Lexeme.semi));
                return node;
            }
            else
            {
                //false
                  return node;
            }
        }

        public TreeNode<Token> Rept_ClassDecl4()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "public")))
            {
                TreeNode<Token> member = new TreeNode<Token>(new Token(Lexeme.keyword, "member"));
                member = member.AddChild(Visibility());
                member = member.AddChild(MemberDecl());
                node = node.AddChild(member);
                lookahead = TokenScanner.Peek();
                if (lookahead.Equals(new Token(Lexeme.keyword, "public")) || lookahead.Equals(new Token(Lexeme.keyword, "private")))
                {
                    node = node.AddChild(Rept_ClassDecl4());
                }
                return node;
            } else if (lookahead.Equals(new Token(Lexeme.keyword, "private")))
            {
                TreeNode<Token> member = new TreeNode<Token>(new Token(Lexeme.keyword, "member"));
                member = member.AddChild(Visibility());
                member = member.AddChild(MemberDecl());
                node = node.AddChild(member);
                lookahead = TokenScanner.Peek();
                if (lookahead.Equals(new Token(Lexeme.keyword, "public")) || lookahead.Equals(new Token(Lexeme.keyword, "private")))
                {
                    node = node.AddChild(Rept_ClassDecl4());
                }
                return node;
            } else
            {
                //true
                return node;
            }
        }

        public TreeNode<Token> MemberDecl()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            TreeNode<Token> function = new TreeNode<Token>(new Token(Lexeme.keyword, "Function"));
            TreeNode<Token> variable = new TreeNode<Token>(new Token(Lexeme.keyword, "Variable"));
            if (lookahead.Equals(new Token(Lexeme.id))) {
                Match(new Token(Lexeme.id));
                if (TokenScanner.Peek().Equals(new Token(Lexeme.id)))
                {
                    variable = variable.AddChild(TokenScanner.Current.Clone());
                    variable = variable.AddChild(AfterMemberDecl());
                    node = node.AddChild(variable);
                }
                else if (TokenScanner.Peek().Equals(new Token(Lexeme.openpar)))
                {
                    function = function.AddChild(TokenScanner.Current.Clone());
                    function = function.AddChild(AfterMemberDecl());
                    node = node.AddChild(function);
                }
                return node;
            } else if (lookahead.Equals(new Token(Lexeme.keyword, "integer"))) {
                variable = variable.AddChild(Before_VarDeclNotId());
                variable = variable.AddChild(VarDecl());
                node = node.AddChild(variable);
                return node;
            } else if (lookahead.Equals(new Token(Lexeme.keyword, "float"))) {
                variable = variable.AddChild(Before_VarDeclNotId());
                variable = variable.AddChild(VarDecl());
                node = node.AddChild(variable);
                return node;
            } else {
                //false
                  return node;
            }
        }

        public TreeNode<Token> VarDecl()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.id)))
            {
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.id));
                node = node.AddChild(Rept_VarDecl2());
                Match(new Token(Lexeme.semi));
                return node;
            } else
            {
                //false
                  return node;
            }
        }

        public TreeNode<Token> Rept_VarDecl2()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.opensqbr)))
            {
                node = node.AddChild(ArraySize());
                lookahead = TokenScanner.Peek();
                if (lookahead.Equals(new Token(Lexeme.opensqbr)))
                {
                    node = node.AddChild(Rept_VarDecl2());
                }
                return node;
            } else
            {
                return node;
            }
        }

        public TreeNode<Token> ArraySize()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            node.Value = new Token(Lexeme.keyword, "array");
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.opensqbr)))
            {
                Match(new Token(Lexeme.opensqbr));
                node = node.AddChild(ArraySizeValue());
                Match(new Token(Lexeme.closesqbr));
                return node;
            } else
            {
                //false
                  return node;
            }
        }

        public TreeNode<Token> ArraySizeValue()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.intnum)))
            {
                node.Value = TokenScanner.Peek().Clone();
                Match(new Token(Lexeme.intnum));
                return node;
            } else
            {
                return node;
            }
        }

        public TreeNode<Token> Before_VarDeclNotId()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "integer")))
            {
                node = node.AddChild(TypeInt());
                return node;
            } else if (lookahead.Equals(new Token(Lexeme.keyword, "float")))
            {
                node = node.AddChild(TypeFloat());
                return node;
            } else
            {
                //false
                  return node;
            }
        }

        public TreeNode<Token> TypeInt()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "integer")))
            {
                node.Value = TokenScanner.Peek().Clone();
                Match(new Token(Lexeme.keyword, "integer"));
                return node;
            }
            else
            {
                //false
                  return node;
            }
        }

        public TreeNode<Token> TypeFloat()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "float")))
            {
                node.Value = TokenScanner.Peek().Clone();
                Match(new Token(Lexeme.keyword, "float"));
                return node;
            }
            else
            {
                //false
                  return node;
            }
        }

        public TreeNode<Token> AfterMemberDecl()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.id)))
            {
                node = node.AddChild(VarDecl());
                return node;
            } else if (lookahead.Equals(new Token(Lexeme.openpar))) {
                node = node.AddChild(FuncDecl());
                return node;
            } else {
                //false
                  return node;
            }
        }

        public TreeNode<Token> FuncDecl()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.openpar)))
            {
                Match(new Token(Lexeme.openpar));
                TreeNode<Token> fparams = new TreeNode<Token>(new Token(Lexeme.keyword, "FParams"));
                fparams = fparams.AddChild(FParams());
                node = node.AddChild(fparams);
                Match(new Token(Lexeme.closepar));
                Match(new Token(Lexeme.colon));
                node = node.AddChild(AfterFuncDecl());
                return node;
            } else
            {
                //false
                  return node;
            }
        }

        public TreeNode<Token> AfterFuncDecl()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "void")))
            {
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.keyword, "void"));
                Match(new Token(Lexeme.semi));
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "integer")))
            {
                node = node.AddChild(Type());
                Match(new Token(Lexeme.semi));
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "float")))
            {
                node = node.AddChild(Type());
                Match(new Token(Lexeme.semi));
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.id)))
            {
                node = node.AddChild(Type());
                Match(new Token(Lexeme.semi));
                return node;
            }
            else
            {
                //false
                  return node;
            }
        }

        public TreeNode<Token> Rept_FParams3()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            TreeNode<Token> param = new TreeNode<Token>(new Token(Lexeme.keyword, "Param"));
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.comma)))
            {
                param = param.AddChild(FParamsTail());
                node = node.AddChild(param);
                lookahead = TokenScanner.Peek();
                if (lookahead.Equals(new Token(Lexeme.comma)))
                {
                    node = node.AddChild(Rept_FParams3());
                }
                return node;
            }
            else
            {
                //true
                return node;
            }
        }

        public TreeNode<Token> FParamsTail()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.comma)))
            {
                Match(new Token(Lexeme.comma));
                node = node.AddChild(Type());
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.id));
                node = node.AddChild(Rept_FParamsTail3());
                return node;
            } else
            {
                //false
                  return node;
            }
        }

        public TreeNode<Token> Rept_FParamsTail3()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.opensqbr)))
            {
                node = node.AddChild(ArraySize());
                lookahead = TokenScanner.Peek();
                if (lookahead.Equals(new Token(Lexeme.opensqbr)))
                {
                    node = node.AddChild(Rept_FParamsTail3());
                }
                return node;
            }
            else
            {
                return node;
            }
        }

        public TreeNode<Token> Rept_FParams2()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            TreeNode<Token> param = new TreeNode<Token>(new Token(Lexeme.keyword, "Param"));
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.opensqbr)))
            {
                param = param.AddChild(ArraySize());
                node = node.AddChild(param);
                lookahead = TokenScanner.Peek();
                if (lookahead.Equals(new Token(Lexeme.opensqbr)))
                {
                   node = node.AddChild(Rept_FParams2());
                }
                return node;
            } else
            {
                return node;
            }
        }

        public TreeNode<Token> Type()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "integer")))
            {
                node = node.AddChild(TypeInt());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "float")))
            {
                node = node.AddChild(TypeFloat());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.id)))
            {
                node = node.AddChild(TypeId());
                return node;
            }
            else
            {
                //false
                  return node;
            }
        }

        public TreeNode<Token> TypeId()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.id)))
            {
                node.Value = TokenScanner.Peek().Clone();
                Match(new Token(Lexeme.id));
                return node;
            } else
            {
                //false
                  return node;
            }
        }

        public TreeNode<Token> FParams()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            TreeNode<Token> param = new TreeNode<Token>(new Token(Lexeme.keyword, "Param"));
            if (lookahead.Equals(new Token(Lexeme.keyword, "integer")))
            {
                param = param.AddChild(Type());
                param = param.AddChild(TokenScanner.Peek().Clone());
                node = node.AddChild(param);
                Match(new Token(Lexeme.id));
                node = node.AddChild(Rept_FParams2());
                node = node.AddChild(Rept_FParams3());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "float")))
            {
                param = param.AddChild(Type());
                param = param.AddChild(TokenScanner.Peek().Clone());
                node = node.AddChild(param);
                Match(new Token(Lexeme.id));
                node = node.AddChild(Rept_FParams2());
                node = node.AddChild(Rept_FParams3());
                return node;
            }
            else if (lookahead.Equals(new Token(Lexeme.id)))
            {
                param = param.AddChild(Type());
                param = param.AddChild(TokenScanner.Peek().Clone());
                node = node.AddChild(param);
                Match(new Token(Lexeme.id));
                node = node.AddChild(Rept_FParams2());
                node = node.AddChild(Rept_FParams3());
                return node;
            }
            else
            {
                //true
                return node;
            }
        }

        public TreeNode<Token> Visibility()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "public")))
            {
                node = node.AddChild(TokenScanner.Peek());
                Match(new Token(Lexeme.keyword, "public"));
                return node;
            } else if (lookahead.Equals(new Token(Lexeme.keyword, "private")))
            {
                node = node.AddChild(TokenScanner.Peek());
                Match(new Token(Lexeme.keyword, "private"));
                return node;
            }
              //false
              return node; 
        }

        public TreeNode<Token> Opt_ClassDecl2()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "inherits"))) 
            {
                //node = new TreeNode<Token>(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.keyword, "inherits"));
                node = node.AddChild(TokenScanner.Peek().Clone());
                Match(new Token(Lexeme.id));
                node = node.AddChild(Rept_Opt_ClassDecl22());
                return node;
            } 
            else
            {
                //true
                return node;
            }
        }

        public TreeNode<Token> Rept_Opt_ClassDecl22()
        {
            TreeNode<Token> node = new TreeNode<Token>();
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.comma)))
            {
                Match(new Token(Lexeme.comma));
                node.Value = TokenScanner.Peek().Clone();
                Match(new Token(Lexeme.id));
                lookahead = TokenScanner.Peek();
                if (lookahead.Equals(new Token(Lexeme.comma)))
                {
                    node = node.AddChild(Rept_Opt_ClassDecl22());
                }
                return node;
            }
            else
            {
                //true
                return node;
            }
        }

        //===========================================//
        //              public methods
        //===========================================//
        public bool Match(Token token)
        {
            if (TokenScanner.Peek().Equals(token))
            {
                TokenScanner.NextToken();
                return true;
            } else
            {
                if (TokenScanner != null && TokenScanner.Current != null)
                {
                    Console.WriteLine("Syntax error at line " + TokenScanner.Current.Line);
                }
                return false;
            }
        }
    }
}
