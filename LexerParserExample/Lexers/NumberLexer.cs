using LexerParserExample.Enums;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace LexerParserExample.Lexers
{
    /// <summary>
    /// A lexer for integers as defined in "Aufgabe 4".
    /// </summary>
    /// <param name="text">The text to scan.</param>
    class NumberLexer : Lexer
    {
        /// <summary>
        /// Initialize a new Numberlexer.
        /// </summary>
        /// <param name="text">The text to scan.</param>
        internal NumberLexer(string text)
        {
            Text = text;
        }
        internal override Token NextToken()
        {
            if (Position >= Text.Length)
                return new Token(TokenKind.EndOfFile, Position, "\0", null);

            if (char.IsDigit(Current))
            {
                int start = Position;
                Next();
                while (char.IsDigit(Current))
                    Next();
                string text = Text[start..Position];
                return new Token(TokenKind.NumberToken, start, Text[start..Position], BigInteger.Parse(text));
            }
            if (Current == '+')
            {
                Next();
                return new Token(TokenKind.PlusToken, Position, "+", '+');
            }
            if (Current == '-')
            {
                Next();
                return new Token(TokenKind.MinusToken, Position, "-", '-');
            }

            Next();
            return new Token(TokenKind.BadToken, Position, Text[Position - 1].ToString(), null);
        }
    }
}
