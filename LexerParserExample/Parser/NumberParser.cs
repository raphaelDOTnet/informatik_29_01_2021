using LexerParserExample.Enums;
using LexerParserExample.Lexers;
using System;
using System.Collections.Generic;
using System.Text;

namespace LexerParserExample.Parsers
{
    /// <summary>
    /// A parser for integers as defined in "Aufgabe 4"
    /// </summary>
    class NumberParser : Parser<Expression>
    {
        /// <summary>
        /// Initialize a new NumberParser
        /// </summary>
        /// <param name="text">The text to parse.</param>
        public NumberParser(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Tokens = new List<Token>();
            NumberLexer lexer = new NumberLexer(text);
            Token token;
            do
            {
                token = lexer.NextToken();
                if (token.Kind == TokenKind.BadToken)
                {
                    Console.WriteLine($"Error: There was an unexpected symbol \"{token.Text}\" at position {token.Position}");
                    HasLexerErrors = true;
                }
                else
                    Tokens.Add(token);
            } while (token.Kind != TokenKind.EndOfFile);
        }
        internal override bool Parse(out Expression expressionSyntax)
        {
            expressionSyntax = null;
            Console.ForegroundColor = ConsoleColor.Red;
            if (HasLexerErrors)
                return false;
            if (Current.Kind == TokenKind.MinusToken ||
                Current.Kind == TokenKind.PlusToken)
            {
                expressionSyntax = new SingleExpression(Current);
                NextToken();
                if (!ParseNumber(out var number))
                    return false;
                expressionSyntax = new DualExpression(expressionSyntax, number);
                return true;
            }
            if (ParseNumber(out expressionSyntax))
                return true;

            Console.WriteLine("Error: A word needs to contain at least one symbol.");
            HasParserErrors = true;
            return false;
        }
        /// <summary>
        /// Parse the current token as an unsigned number
        /// </summary>
        /// <param name="expression">The evaluated expression of the token</param>
        /// <returns>Whether the job finished successfully</returns>
        private bool ParseNumber(out Expression expression)
        {
            expression = null;
            if (Current.Kind == TokenKind.NumberToken)
            {
                expression = new SingleExpression(Current);
                NextToken();
                if (Current.Kind != TokenKind.EndOfFile)
                {
                    Console.WriteLine($"Error: A number may not be followed by another symbol at position {Current.Position}.");
                    HasParserErrors = true;
                    return false;
                }
                return true;
            }
            return false;
        }
    }
}
