using LexerParserExample.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LexerParserExample.Parsers
{
    /// <summary>
    /// A base class providing support for parsing text into T
    /// </summary>
    /// <typeparam name="T">A type that will be returned as a result of successfull parsing</typeparam>
    abstract class Parser<T>
    {
        /// <summary>
        /// Did errors occur scanning the text?
        /// </summary>
        public bool HasLexerErrors { get; protected set; }
        /// <summary>
        /// Did errors occur parsing the text
        /// </summary>
        public bool HasParserErrors { get; protected set; }
        /// <summary>
        /// The current position of the parser in tokens
        /// </summary>
        protected int Position = 0;
        /// <summary>
        /// The current token
        /// </summary>
        protected Token Current => Peek(0);
        /// <summary>
        /// The list of tokens as returned by the lexer.
        /// </summary>
        internal List<Token> Tokens { get; set; }
        /// <summary>
        /// Parse the text.
        /// </summary>
        /// <param name="expressionSyntax">Retrieve the T the text has been parsed into.</param>
        /// <returns>Whether the job finished successfully.</returns>
        internal abstract bool Parse(out T expressionSyntax);
        /// <summary>
        /// View an item in the Tokens list
        /// </summary>
        /// <param name="offset">An offset from the current position</param>
        /// <returns>The item from the tokens list at the given point</returns>
        protected Token Peek(int offset)
        {
            if (Position + offset >= Tokens.Count)
                return Tokens[Tokens.Count - 1];
            return Tokens[Position + offset];
        }
        /// <summary>
        /// Move to the next token.
        /// </summary>
        /// <returns>The current token before moving.</returns>
        protected Token NextToken()
        {
            Token current = Current;
            Position++;
            return current;
        }
    }
}
