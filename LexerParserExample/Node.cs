using LexerParserExample.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LexerParserExample
{
    /// <summary>
    /// A base class providing support for more complex expressions.
    /// </summary>
    public abstract class Node
    {
        /// <summary>
        /// The Kind of token this node represents
        /// </summary>
        public abstract TokenKind Kind { get; }
        /// <summary>
        /// Get all nodes managed by this one.
        /// Useful for drawing trees of expressions
        /// </summary>
        /// <returns>Get all nodes managed by this one.</returns>
        public abstract IEnumerable<Node> GetChildren();
    }
}
