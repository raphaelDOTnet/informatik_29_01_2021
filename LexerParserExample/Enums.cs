using System;
using System.Collections.Generic;
using System.Text;

namespace LexerParserExample.Enums
{
    /// <summary>
    /// The type of parser to use
    /// </summary>
    public enum ParserType
    {
        Lal, 
        Integer
    }
    /// <summary>
    /// The kind of the token
    /// </summary>
    public enum TokenKind
    {
        LaToken,
        LeToken,
        LuToken,

        NumberToken, 
        PlusToken,
        MinusToken,

        TextToken,
        WhiteSpaceToken,
        DoubleQuoteToken,

        // Special Tokens
        BadToken,
        EndOfFile,

        // Expression Tokens
        SingleExpression,
        DualExpression,
    }
}
