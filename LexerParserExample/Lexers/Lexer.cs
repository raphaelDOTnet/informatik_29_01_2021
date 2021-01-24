using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using LexerParserExample.Enums;

namespace LexerParserExample.Lexers
{
    /// <summary>
    /// A base class providing support for adding lexers/tokenizer/scanner
    /// </summary>
    abstract class Lexer
    {
        /// <summary>
        /// The text to scan
        /// </summary>
        protected string Text;
        /// <summary>
        /// The current position of the scanner in the text
        /// </summary>
        protected int Position = 0;
        /// <summary>
        /// The char at the current position in the text
        /// </summary>
        protected char Current
        {
            get
            {
                if (Position >= Text.Length)
                    return '\0';
                else
                    return Text[Position];
            }
        }
        /// <summary>
        /// View a char from the text
        /// </summary>
        /// <param name="offset">An offset from the current position</param>
        /// <returns>The char from the text at the given position</returns>
        protected char Peek(int offset)
        {
            if (Position + offset >= Text.Length)
                return Text[^1];
            return Text[Position + offset];
        }
        /// <summary>
         /// Scan the entire text at once
         /// </summary>
         /// <returns>a list of tokens representing the text</returns>
        internal List<Token> ScanAll()
        {
            List<Token> result = new List<Token>();
            do
                result.Add(NextToken());
            while (result[result.Count - 1].Kind != TokenKind.EndOfFile);
            return result;
        }
        /// <summary>
        /// Get the next token from the text
        /// </summary>
        /// <returns>A token from the text</returns>
        internal abstract Token NextToken();
        /// <summary>
        /// Move to the next char in the text
        /// </summary>
        protected void Next()
        {
            Position++;
        }
    }
}
