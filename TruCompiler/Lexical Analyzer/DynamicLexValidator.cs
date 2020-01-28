﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Lexical_Analyzer
{
    public class DynamicLexValidator : IValidator<bool>
    {
        public bool Validate(string value, string type)
        {
            switch (type)
            {
                case "Integer":
                    return ValidateInteger(value);
                case "Float":
                    return ValidateFloat(value);
                case "Identifier":
                    return ValidateIdentifier(value);
                default:
                    throw new Exception("Argument type cannot be empty or null", new ArgumentNullException(type));
            }
        }

        private bool ValidateInteger(string value)
        {
            if (Regex.IsMatch(value, "/^[1-9]+[0-9]*/"))
            {
                return true;
            } else if (value.Length == 1 && value.StartsWith("0"))
            {
                return true;
            } 
            return false;
        }

        private bool ValidateIdentifier(string value)
        {
            return false;
        }

        private bool ValidateFloat(string value)
        {
            if (value.Length > 0 && ValidateInteger(""+value[0]))
            {
                if (Regex.IsMatch(value, "/[1-9]+[0-9]*(.)0/"))
                {
                    return true;
                } else if (Regex.IsMatch(value, "/[1-9]+[0-9]*(.)[0-9]*[1-9]+/"))
                {
                    return true;
                } else if (Regex.IsMatch(value, "/[1-9]+[0-9]*(.)[0-9]*[1-9]+(e)[+--][1-9]+[0-9]*/"))
                {
                    return true;
                }
                return false;
            }
            return false; 
        }

       
    }
}