using LexerParserExample.Enums;
using LexerParserExample.Parsers;
using System;
using System.Linq;

namespace LexerParserExample
{
    /// <summary>
    /// This class manages the console window and the application logic.
    /// </summary>
    class Program
    {
        /// <summary>
        /// The type of parser currently in use.
        /// </summary>
        static ParserType Type;
        /// <summary>
        /// Is debug information such as tokens list and expression trees being displayed?
        /// </summary>
        static bool ShowDebug = true;
        /// <summary>
        /// Main entry point and application loop
        /// </summary>
        static void Main()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("> ");
                string text = Console.ReadLine().Trim().ToLower();
                new CommandLineParser(text).Parse(out Command command);
                if (command == null || !command.IsValid)
                    continue;
                Console.ResetColor();
                switch (command.Name)
                {
                    case "mode":
                        Mode(command);
                        break;
                    case "debug":
                        Debug(command);
                        break;
                    case "help":
                        Help(command);
                        continue;
                    case "parse":
                        Parse(command);
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"The command \"{command.Name}\" was not recognized. Please see \"help\" for more information.");
                        break;
                }
            }
        }
        /// <summary>
        /// Set the parser in use
        /// </summary>
        /// <param name="command">The command as entered by the user</param>
        private static void Mode(Command command)
        {
            if (command.GetParameter("p", out var parserParam) ||
                command.GetParameter(0, out parserParam))
            {
                if (parserParam.Value == "lal")
                    Type = ParserType.Lal;
                else if (parserParam.Value == "integer")
                    Type = ParserType.Integer;
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{parserParam.Value} is not a valid parser. Please see \"help\" for more information.");
                    return;
                }
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"The current parser type is \"{Type.ToString()}\".");
                return;
            }
            if(command.TryPrintUncalledParams())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There is no parameter given to use as the new parser. Please see \"help\" for more information.");
            }

        }
        /// <summary>
        /// Toggle the display of debug information
        /// </summary>
        /// <param name="command">The command as entered by the user</param>
        private static void Debug(Command command)
        {
            if (!command.TryPrintUncalledParams())
                return;
            ShowDebug = !ShowDebug;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Debug information is {(ShowDebug? "enabled" : "disabled")}");
        }
        /// <summary>
        /// Parse a word entered by the user
        /// </summary>
        /// <param name="command">The command as entered by the user</param>
        private static void Parse(Command command)
        {
            ParserType useType = Type;
            if (command.GetParameter("p", out var ParserParam))
            {
                if (ParserParam.Value == "lal")
                    useType = ParserType.Lal;
                else if (ParserParam.Value == "integer")
                    useType = ParserType.Integer;
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{ParserParam.Value} is not a valid parser. Please see \"help\" for more information.");
                    return;
                }
                Console.ResetColor();
                Console.WriteLine($"Using {useType.ToString()} parser:");
            }
            if (command.GetParameter("t", out var textParam) ||
                (command.GetParameter(0, out textParam) && textParam.Name == ""))
                Console.WriteLine($"Starting {useType.ToString()} parser on \"{textParam.Value}\"...");
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You need to specify a text to parse. Please see \"help\" for more information.");
                return;
            }
            if (!command.TryPrintUncalledParams())
                return;

            Parser<Expression> parser = null;
            switch (useType)
            {
                case ParserType.Lal:
                    parser = new LalParser(textParam.Value);
                    break;
                case ParserType.Integer:
                    parser = new NumberParser(textParam.Value);
                    break;
            }
            if (parser.HasLexerErrors)
                return;
            if (!parser.Parse(out var expression))
                return;
            if (ShowDebug)
            {
                Console.ResetColor();
                PrintTokens(parser);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Syntax Tree:");
                Console.ResetColor();
                PrintExpression(expression);
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"The word \"{textParam.Value}\" is part of the {useType.ToString()} language.");
        }
        /// <summary>
        /// Provide help and a list of all commands
        /// </summary>
        /// <param name="command">The command as entered by the user</param>
        private static void Help(Command command)
        {
            if (!command.TryPrintUncalledParams())
                return;
            Console.WriteLine("help                     Get help for the tool.");
            Console.WriteLine("debug                    Enable or disable the display of tokens and expressions.");
            Console.WriteLine("mode                     Set the parser to use to...");
            Console.WriteLine("    -lal                 ...Lal parser.");
            Console.WriteLine("    -integer             ...Integer parser.");
            Console.WriteLine("    -p 'parser'          Set the parser to use.");
            Console.WriteLine("parse                    Initialize parsing text using the current parser.");
            Console.WriteLine("    -t 'text'|-'text'    The text you want to parse.");
            Console.WriteLine("    -p 'parser'          The parser you want to use.");
        }
        /// <summary>
        /// Print all the tokens of a given parser
        /// </summary>
        /// <typeparam name="T">The out-type of the parser</typeparam>
        /// <param name="parser">the parser to use</param>
        private static void PrintTokens<T>(Parser<T> parser)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Tokens:");
            Console.ResetColor();
            foreach (var token in parser.Tokens)
            {
                Console.WriteLine($"{token.Kind}: \"{token.Text}\" at position {token.Position}.");
            }
        }
        /// <summary>
        /// Print an expression as a tree 
        /// </summary>
        /// <param name="node">the root of the tree</param>
        /// <param name="indent">the current indent of the tree</param>
        /// <param name="isLastChild">is this node the last child of its parents?</param>
        private static void PrintExpression(Node node, string indent = "", bool isLastChild = true)
        {
            string marker = isLastChild ? "└─" : "├─";
            Console.Write(indent);
            Console.Write(marker);
            Console.Write(node.Kind);
            if (node is Token st && st.Value != null)
            {
                Console.Write($": {st.Value}");
            }
            indent += isLastChild ? "  " : "| ";
            Console.WriteLine();
            var lastChildNode = node.GetChildren().LastOrDefault();
            foreach (var child in node.GetChildren())
            {
                PrintExpression(child, indent, child == lastChildNode);
            }
        }
    }
}
