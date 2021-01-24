using LexerParserExample.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LexerParserExample.Lexers
{
    /// <summary>
    /// The lexer scanning user input from the console window.
    /// </summary>
    class CommandLineLexer : Lexer
    {
        private bool inParantheses = false;
        /// <summary>
        /// Initialize a new Commandlinelexer.
        /// </summary>
        /// <param name="text">The text to scan.</param>
        internal CommandLineLexer(string text)
        {
            Text = text;
        }
        internal override Token NextToken()
        {
            if (Position >= Text.Length)
                return new Token(TokenKind.EndOfFile, Position, "\0", null);

            if (Current == '"')
            {
                inParantheses = !inParantheses;
                return new Token(TokenKind.DoubleQuoteToken, Position++, "\"", '\"');
            }
            if (inParantheses)
            {
                int start = Position;
                while(Current != '"' && Current != '\0')
                    Next();
                return new Token(TokenKind.TextToken, start, Text[start..Position], Text[start..Position]);
            }

            if (char.IsWhiteSpace(Current))
            {
                int start = Position;
                while (char.IsWhiteSpace(Current))
                    Next();
                return new Token(TokenKind.WhiteSpaceToken, start, Text[start..Position], " ");
            }
            if (char.IsLetterOrDigit(Current))
            {
                int start = Position;
                while (char.IsLetterOrDigit(Current))
                    Next();
                var text = Text[start..Position];
                return new Token(TokenKind.TextToken, start, text, text);
            }
            if (Current == '-')
            {
                return new Token(TokenKind.MinusToken, Position++, "-", "-");
            }
            Next();
            return new Token(TokenKind.BadToken, Position, Text[Position - 1].ToString(), null);
        }
    }
}
