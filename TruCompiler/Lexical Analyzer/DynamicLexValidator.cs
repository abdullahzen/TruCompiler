using System;
using System.Collections.Generic;
using System.Text;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Lexical_Analyzer
{
    public class DynamicLexValidator : IValidator<Token>
    {
        public Token Validate(string value, int i, string type)
        {
            switch (type)
            {
                case "Integer":
                    return ValidateInteger(value, i);
                case "Float":
                    return ValidateFloat(value, i);
                case "Identifier":
                    return ValidateIdentifier(value, i);
                default:
                    throw new Exception("Argument type cannot be empty or null", new ArgumentNullException(type));
            }
        }

        private Token ValidateInteger(string value, int i)
        {
            return new Token();
        }

        private Token ValidateIdentifier(string value, int i)
        {
            return new Token();

        }

        private Token ValidateFloat(string value, int i)
        {
            return new Token();


        }

       
    }
}
