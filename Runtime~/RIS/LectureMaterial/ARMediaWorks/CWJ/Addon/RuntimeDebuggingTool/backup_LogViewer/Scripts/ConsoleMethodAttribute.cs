using System;

namespace CWJ.RuntimeDebugging
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class ConsoleMethodAttribute : Attribute
    {
        public string Command { get; private set; }
        public string Description { get; private set; }

        public ConsoleMethodAttribute(string command, string description)
        {
            Command = command;
            Description = description;
        }
    }
}