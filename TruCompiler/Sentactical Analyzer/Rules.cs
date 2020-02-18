using System;
using System.Collections.Generic;
using System.Text;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Sentactical_Analyzer
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
        public bool Start()
        {
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "class")))
            {
                Prog();
                return true;
            } else if (lookahead.Equals(new Token(Lexeme.id)))
            {
                Prog();
                return true;
            } else if (lookahead.Equals(new Token(Lexeme.keyword, "main")))
            {
                Prog();
                return true;
            }
            return false;
        }

        public bool Prog()
        {
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "class")))
            {
                Rept_Prog0();
                //Rept_Prog1();
                match(new Token(Lexeme.keyword, "main"));
                //FuncBody();
                return true;
            }
            else if (lookahead.Equals(new Token(Lexeme.id)))
            {
                //Rept_Prog1();
                //match(new Token(Lexeme.keyword, "main"));
                //FuncBody();
                return true;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "main")))
            {
                match(new Token(Lexeme.keyword, "main"));
                //FuncBody()
                return true;
            }
            return false;
        }

        public bool FuncBody()
        {
            return true;
        }

        public bool Rept_Prog0()
        {
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "class")))
            {
                ClassDecl();
                lookahead = TokenScanner.Peek();
                if (lookahead.Equals(new Token(Lexeme.keyword, "class")))
                {
                    return Rept_Prog0();
                }
                return true;
            } else
            {
                return false;
            }
        }

        public bool Rept_Prog1()
        {
            throw new NotImplementedException();
        }

        public bool ClassDecl()
        {
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "class")))
            {
                match(new Token(Lexeme.keyword, "class"));
                match(new Token(Lexeme.id));
                Opt_ClassDecl2();
                match(new Token(Lexeme.opencbr));
                Rept_ClassDecl4();
                match(new Token(Lexeme.closecbr));
                match(new Token(Lexeme.semi));
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Rept_ClassDecl4()
        {
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "public")))
            {
                Visibility();
                MemberDecl();
                lookahead = TokenScanner.Peek();
                if (lookahead.Equals(new Token(Lexeme.keyword, "public")) || lookahead.Equals(new Token(Lexeme.keyword, "private")))
                {
                    return Rept_ClassDecl4();
                }
                return true;
            } else if (lookahead.Equals(new Token(Lexeme.keyword, "private")))
            {
                Visibility();
                MemberDecl();
                lookahead = TokenScanner.Peek();
                if (lookahead.Equals(new Token(Lexeme.keyword, "public")) || lookahead.Equals(new Token(Lexeme.keyword, "private")))
                {
                    return Rept_ClassDecl4();
                }
                return true;
            } else
            {
                return true;
            }
        }

        public bool MemberDecl()
        {
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.id))) {
                match(new Token(Lexeme.id));
                AfterMemberDecl();
                return true;
            } else if (lookahead.Equals(new Token(Lexeme.keyword, "integer"))) {
                Before_VarDeclNotId();
                VarDecl();
                return true;
            } else if (lookahead.Equals(new Token(Lexeme.keyword, "float"))) {
                Before_VarDeclNotId();
                VarDecl();
                return true;
            } else {
                return false;
            }
        }

        public bool VarDecl()
        {
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.id)))
            {
                match(new Token(Lexeme.id));
                Rept_VarDecl2();
                match(new Token(Lexeme.semi));
                return true;
            } else
            {
                return false;
            }
        }

        public bool Rept_VarDecl2()
        {
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.opensqbr)))
            {
                ArraySize();
                lookahead = TokenScanner.Peek();
                if (lookahead.Equals(new Token(Lexeme.opensqbr)))
                {
                    return Rept_VarDecl2();
                }
                return true;
            } else
            {
                return true;
            }
        }

        private bool ArraySize()
        {
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.opensqbr)))
            {
                match(new Token(Lexeme.opensqbr));
                ArraySizeValue();
                match(new Token(Lexeme.closesqbr));
                return true;
            } else
            {
                return false;
            }
        }

        private bool ArraySizeValue()
        {
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.intnum)))
            {
                match(new Token(Lexeme.intnum));
                return true;
            } else
            {
                return true;
            }
        }

        public bool Before_VarDeclNotId()
        {
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "integer")))
            {
                TypeInt();
                return true;
            } else if (lookahead.Equals(new Token(Lexeme.keyword, "float")))
            {
                TypeFloat();
                return true;
            } else
            {
                return false;
            }
        }

        public bool TypeInt()
        {
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "integer")))
            {
                match(new Token(Lexeme.keyword, "integer"));
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TypeFloat()
        {
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "float")))
            {
                match(new Token(Lexeme.keyword, "float"));
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AfterMemberDecl()
        {
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.id)))
            {
                VarDecl();
                return true;
            } else if (lookahead.Equals(new Token(Lexeme.openpar))) {
                FuncDecl();
                return true;
            } else {
                return false;
            }
        }

        public bool FuncDecl()
        {
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.openpar)))
            {
                match(new Token(Lexeme.openpar));
                FParams();
                match(new Token(Lexeme.closepar));
                match(new Token(Lexeme.colon));
                AfterFuncDecl();
                return true;
            } else
            {
                return false;
            }
        }

        private bool AfterFuncDecl()
        {
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "void")))
            {
                match(new Token(Lexeme.keyword, "void"));
                match(new Token(Lexeme.semi));
                return true;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "integer")))
            {
                Type();
                match(new Token(Lexeme.semi));
                return true;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "float")))
            {
                Type();
                match(new Token(Lexeme.semi));
                return true;
            }
            else if (lookahead.Equals(new Token(Lexeme.id)))
            {
                Type();
                match(new Token(Lexeme.semi));
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool Rept_FParams3()
        {
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.comma)))
            {
                FParamsTail();
                lookahead = TokenScanner.Peek();
                if (lookahead.Equals(new Token(Lexeme.comma)))
                {
                    return Rept_FParams3();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool FParamsTail()
        {
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.comma)))
            {
                match(new Token(Lexeme.comma));
                Type();
                match(new Token(Lexeme.id));
                Rept_FParamsTail3();
                return true;
            } else
            {
                return false;
            }
        }

        private bool Rept_FParamsTail3()
        {
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.opensqbr)))
            {
                ArraySize();
                lookahead = TokenScanner.Peek();
                if (lookahead.Equals(new Token(Lexeme.opensqbr)))
                {
                    return Rept_FParamsTail3();
                }
                return true;
            }
            else
            {
                return true;
            }
        }

        private bool Rept_FParams2()
        {
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.opensqbr)))
            {
                ArraySize();
                lookahead = TokenScanner.Peek();
                if (lookahead.Equals(new Token(Lexeme.opensqbr)))
                {
                   return Rept_FParams2();
                }
                return true;
            } else
            {
                return true;
            }
        }

        private bool Type()
        {
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "integer")))
            {
                TypeInt();
                return true;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "float")))
            {
                TypeFloat();
                return true;
            }
            else if (lookahead.Equals(new Token(Lexeme.id)))
            {
                TypeId();
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool TypeId()
        {
            if (match(new Token(Lexeme.id)))
            {
                return true;
            } else
            {
                return false;
            }
        }

        private bool FParams()
        {
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "integer")))
            {
                Type();
                match(new Token(Lexeme.id));
                Rept_FParams2();
                Rept_FParams3();
                return true;
            }
            else if (lookahead.Equals(new Token(Lexeme.keyword, "float")))
            {
                Type();
                match(new Token(Lexeme.id));
                Rept_FParams2();
                Rept_FParams3();
                return true;
            }
            else if (lookahead.Equals(new Token(Lexeme.id)))
            {
                Type();
                match(new Token(Lexeme.id));
                Rept_FParams2();
                Rept_FParams3();
                return true;
            }
            else
            {
                return true;
            }
        }

        public bool Visibility()
        {
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "public")))
            {
                match(new Token(Lexeme.keyword, "public"));
                return true;
            } else if (lookahead.Equals(new Token(Lexeme.keyword, "private")))
            {
                match(new Token(Lexeme.keyword, "private"));
                return true;
            }
            return false;
        }

        public bool Opt_ClassDecl2()
        {
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.keyword, "inherits"))) 
            {
                match(new Token(Lexeme.keyword, "inherits"));
                match(new Token(Lexeme.id));
                Rept_Opt_ClassDecl22();
                return true;
            } 
            else
            {
                return true;
            }
        }

        public bool Rept_Opt_ClassDecl22()
        {
            Token lookahead = TokenScanner.Peek();
            if (lookahead.Equals(new Token(Lexeme.comma)))
            {
                match(new Token(Lexeme.comma));
                match(new Token(Lexeme.id));
                lookahead = TokenScanner.Peek();
                if (lookahead.Equals(new Token(Lexeme.comma)))
                {
                    Rept_Opt_ClassDecl22();
                }
                return true;
            }
            else
            {
                return true;
            }
        }

        //===========================================//
        //              Private methods
        //===========================================//
        private bool match(Token token)
        {
            if (TokenScanner.Peek().Equals(token))
            {
                TokenScanner.NextToken();
                return true;
            } else
            {
                return false;
            }
        }
    }
}
