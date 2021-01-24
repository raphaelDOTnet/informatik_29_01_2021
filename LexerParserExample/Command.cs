using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LexerParserExample
{
    /// <summary>
    /// A representation of a command the user enters in the console window.
    /// </summary>
    class Command
    {
        /// <summary>
        /// Is the command valid?
        /// </summary>
        public bool IsValid { get; set; }
        /// <summary>
        /// The name of the command
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// A dictionary of all parameters.
        /// </summary>
        private readonly Dictionary<(string name, int position), Parameter> parameters;
        /// <summary>
        /// A list of all parameters that have not been accessed yet.
        /// This is useful for better diagnostics.
        /// </summary>
        private readonly List<(string name, int position)> uncalledParams;
        /// <summary>
        /// Initialize a new command.
        /// </summary>
        /// <param name="name">the name of the command</param>
        /// <param name="isValid">is the command valid?</param>
        public Command(string name, bool isValid)
        {
            Name = name;
            IsValid = isValid;
            parameters = new Dictionary<(string, int), Parameter>();
            uncalledParams = new List<(string name, int position)>();
        }
        /// <summary>
        /// Get the value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to evaluate</param>
        /// <param name="param">The evaluated parameter</param>
        /// <returns>Whether the job finished successfully.</returns>
        internal bool GetParameter(string name, out Parameter param)
        {
            param = default;
            var key = parameters.Keys.Where(k => k.name == name).FirstOrDefault();
            if (key == default)
                return false;
            uncalledParams.Remove(key);
            return parameters.TryGetValue(key, out param);
        }
        /// <summary>
        /// Get the value of a parameter
        /// </summary>
        /// <param name="position">The position of the parameter to evaluate</param>
        /// <param name="param">The evaluated parameter</param>
        /// <returns>Whether the job finished successfully.</returns>
        internal bool GetParameter(int position, out Parameter param)
        {
            param = default;
            var key = parameters.Keys.Where(k => k.position == position).FirstOrDefault();
            if (key == default)
                return false;
            uncalledParams.Remove(key);
            return parameters.TryGetValue(key, out param);
        }
        /// <summary>
        /// Add another parameter to the command
        /// </summary>
        /// <param name="name">the name of the parameter (empty if entered as "-param")</param>
        /// <param name="value">the value of the parameter</param>
        /// <param name="position">the position of the parameter</param>
        internal void AddParameter(string name, string value, int position)
        {
            Parameter param = new Parameter(name, value, position);
            parameters.Add((name, position), param);
            uncalledParams.Add((name, position));
        }
        /// <summary>
        /// Add another parameter to the command
        /// </summary>
        /// <param name="value">the value of the parameter</param>
        /// <param name="position">the position of the parameter</param>
        internal void AddParameter(int position, string value)
        {
            Parameter param = new Parameter("", value, position);
            parameters.Add(("", position), param);
            uncalledParams.Add(("", position));
        }
        /// <summary>
        /// Print errors for all unused parameters.
        /// </summary>
        /// <returns>Whether there were no unused parameters</returns>
        internal bool TryPrintUncalledParams()
        {
            if (uncalledParams.Count == 0)
                return true;
            Console.ForegroundColor = ConsoleColor.Red;
            foreach(var (name, position) in uncalledParams)
            {
                if (name == "")
                    Console.WriteLine($"Unable to resolve the {position + 1}. parameter.");
                else
                    Console.WriteLine($"Unable to resolve parameter \"{name}\".");
            }
            return false;

        }
    }
    /// <summary>
    /// A representation of parameters the user enters in the console window.
    /// </summary>
    struct Parameter
    {
        /// <summary>
        /// The name of the parameter
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// The value of the parameter
        /// </summary>
        public string Value { get; }
        /// <summary>
        /// The position of the parameter
        /// </summary>
        public int Position { get; }
        /// <summary>
        /// Initialize a new parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="value">The value of the parameter</param>
        /// <param name="position">The position of the parameter</param>
        public Parameter(string name, string value, int position)
        {
            Name = name;
            Value = value;
            Position = position;
        }
    }
}
