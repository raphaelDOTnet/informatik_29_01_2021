using LexerParserExample.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LexerParserExample
{
    /// <summary>
    /// The tokens that lexers generate
    /// </summary>
    public class Token : Node
    {
        /// <summary>
        /// Initialize a new token
        /// </summary>
        /// <param name="kind">the kind of token</param>
        /// <param name="position">the beginning of the token in characters</param>
        /// <param name="text">the text evaluated to generate this token</param>
        /// <param name="value">the value of the token</param>
        public Token(TokenKind kind, int position, string text, object value)
        {
            Kind = kind;
            Position = position;
            Text = text;
            Value = value;
        }

        public override TokenKind Kind { get; }
        /// <summary>
        /// The beginning of the token in characters
        /// </summary>
        public int Position { get; }
        /// <summary>
        /// the text evaluated to generate this token
        /// </summary>
        public string Text { get; }
        /// <summary>
        /// The value of the token
        /// </summary>
        public object Value { get; }

        public override IEnumerable<Node> GetChildren()
        {
            return Enumerable.Empty<Node>();
        }
    }
}
