using LexerParserExample.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LexerParserExample.Lexers
{
    /// <summary>
    /// The lexer for the Lal language as described in "Aufgabe 3"
    /// </summary>
    class LalLexer : Lexer
    {
        /// <summary>
        /// Initialize a new LalLexer.
        /// </summary>
        /// <param name="text">The text to scan.</param>
        internal LalLexer(string text)
        {
            Text = text;
        }
        internal override Token NextToken()
        {
            if (Position >= Text.Length)
                return new Token(TokenKind.EndOfFile, Position, "\0", null);

            if (Current == 'l')
            {
                Next();
                switch (Current)
                {
                    case 'a':
                        Next();
                        return new Token(TokenKind.LaToken, Position - 1, "la", "la");
                    case 'e':
                        Next();
                        return new Token(TokenKind.LeToken, Position - 1, "le", "le");
                    case 'u':
                        Next();
                        return new Token(TokenKind.LuToken, Position - 1, "lu", "lu");
                }
                Next();
                if(Position > Text.Length)
                    return new Token(TokenKind.BadToken, Position - 1, Text.Substring(Position - 2, 1), null);
                else
                    return new Token(TokenKind.BadToken, Position - 1, Text.Substring(Position - 2, 2), null);
            }
            Next();
            return new Token(TokenKind.BadToken, Position, Text[Position - 1].ToString(), null);
        }
    }
}
