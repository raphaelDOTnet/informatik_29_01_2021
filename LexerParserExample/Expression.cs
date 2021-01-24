using LexerParserExample.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LexerParserExample
{
    /// <summary>
    /// A base class for expressions
    /// </summary>
    abstract class Expression : Node
    {
    }
    /// <summary>
    /// An expression for a single token
    /// </summary>
    class SingleExpression : Expression
    {
        public override TokenKind Kind => TokenKind.SingleExpression;
        /// <summary>
        /// The token held by this expression
        /// </summary>
        public Token Token { get; }
        /// <summary>
        /// Initialize a new SingleExpression.
        /// </summary>
        /// <param name="token">The token this expression will use</param>
        public SingleExpression(Token token) 
        {
            Token = token;
        }
        public override IEnumerable<Node> GetChildren()
        {
            yield return Token;
        }
    }
    /// <summary>
    /// An Expression for a couple of expressions
    /// </summary>
    class DualExpression : Expression
    {
        public override TokenKind Kind => TokenKind.DualExpression;
        /// <summary>
        /// The first expression
        /// </summary>
        public Expression A { get; }
        /// <summary>
        /// The second expression
        /// </summary>
        public Expression B { get; }
        /// <summary>
        /// Initialize a new DualExpression
        /// </summary>
        /// <param name="a">the first expression</param>
        /// <param name="b">the second expression</param>
        public DualExpression(Expression a, Expression b)
        {
            A = a;
            B = b;
        }
        public override IEnumerable<Node> GetChildren()
        {
            yield return A;
            yield return B;
        }
    }
}
