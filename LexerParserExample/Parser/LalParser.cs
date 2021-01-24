using LexerParserExample.Enums;
using LexerParserExample.Lexers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LexerParserExample.Parsers
{
    /// <summary>
    /// A parser for the Lal language as described in "Aufgabe 3".
    /// </summary>
    class LalParser : Parser<Expression>
    {
        /// <summary>
        /// Initialize a new LalParser.
        /// </summary>
        /// <param name="text">The text to parse.</param>
        public LalParser(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Tokens = new List<Token>();
            LalLexer lexer = new LalLexer(text);
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

            if(Peek(2).Kind == TokenKind.EndOfFile ||
                Peek(3).Kind != TokenKind.EndOfFile)
            {
                Console.WriteLine("Error: The word must contain exactly 3 silibles.");
                HasParserErrors = true;
                return false;
            }
            expressionSyntax = new DualExpression(
                new SingleExpression(NextToken()), 
                new SingleExpression(NextToken()));
            if(Current.Kind != TokenKind.LuToken)
            {
                Console.WriteLine("Error: The word must end with \"lu\".");
                HasParserErrors = true;
                return false;
            }
            expressionSyntax = new DualExpression(expressionSyntax,
                new SingleExpression(Current));
            return true;
        }
    }
}
