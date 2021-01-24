using LexerParserExample.Enums;
using LexerParserExample.Lexers;
using System;
using System.Collections.Generic;
using System.Text;

namespace LexerParserExample.Parsers
{
    /// <summary>
    /// The parser parsing the user input from the console window.
    /// </summary>
    class CommandLineParser : Parser<Command>
    {
        /// <summary>
        /// Initialize a new CommandLineParser
        /// </summary>
        /// <param name="text">The text to parse.</param>
        internal CommandLineParser(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Tokens = new List<Token>();
            CommandLineLexer lexer = new CommandLineLexer(text);
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
        internal override bool Parse(out Command command)
        {
            command = null;
            Console.ForegroundColor = ConsoleColor.Red;
            if (HasLexerErrors)
                return false;

            if (Current.Kind == TokenKind.WhiteSpaceToken)
                NextToken();
            if(Current.Kind != TokenKind.TextToken)
            {
                Console.WriteLine("Error: Every command must start with the identifier of the command.");
                HasParserErrors = true;
                return false;
            }
            string commandName = Current.Text;

            command = new Command(commandName, false);
            int pos = -1;
            while (NextToken().Kind != TokenKind.EndOfFile && Current.Kind != TokenKind.EndOfFile)
            {
                pos++;
                if(Current.Kind != TokenKind.WhiteSpaceToken)
                {
                    Console.WriteLine("Error: Parameters must be seperated with whitespace.");
                    HasParserErrors = true;
                    return false;
                }
                NextToken();
                if (Current.Kind != TokenKind.MinusToken)
                {
                    Console.WriteLine("Error: Parameters must be initialized using '-'.");
                    HasParserErrors = true;
                    return false;
                }
                NextToken();
                if (Current.Kind == TokenKind.TextToken &&
                    Peek(1).Kind == TokenKind.WhiteSpaceToken &&
                    Peek(2).Kind == TokenKind.TextToken)
                {
                    command.AddParameter(Current.Text, Peek(2).Text, pos);
                    NextToken();
                    NextToken();
                    continue;
                }
                if (Current.Kind == TokenKind.TextToken &&
                   Peek(1).Kind == TokenKind.WhiteSpaceToken &&
                   Peek(2).Kind == TokenKind.DoubleQuoteToken)
                {
                    if (Peek(3).Kind == TokenKind.DoubleQuoteToken)
                    {
                        command.AddParameter(Current.Text, string.Empty, pos);
                        for (int i = 0; i < 3; i++)
                            NextToken();
                        continue;
                    }
                    if (Peek(4).Kind != TokenKind.DoubleQuoteToken)
                    {
                        Console.WriteLine($"Error: You must close the paranthesis opened at position {Peek(2).Position}.");
                        HasParserErrors = true;
                        return false;
                    }
                    command.AddParameter(Current.Text, Peek(3).Text, pos);
                    for(int i = 0; i < 4; i++)
                        NextToken();
                    continue;
                }
                if (Current.Kind == TokenKind.TextToken)
                {
                    command.AddParameter(pos, Current.Text);
                    continue;
                }
                Console.WriteLine($"Error: The token \"{Current.Text}\" at position {Current.Position} is invalid.");
                HasParserErrors = true;
                return false;
            }
            command.IsValid = true;
            return true;
        }
    }
}
