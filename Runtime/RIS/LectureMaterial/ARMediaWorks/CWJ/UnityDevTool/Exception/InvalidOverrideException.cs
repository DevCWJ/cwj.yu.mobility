using System;

namespace  CWJ
{
    public class InvalidOverrideException : Exception
    {
        public InvalidOverrideException(string message, string cause) : base(message + "\nCause: " + cause) { }

        public InvalidOverrideException(string message, string cause, Exception innerException) : base(message + "\nCause: " + cause, innerException) { }

        public override string Message => "Your code is override invalid thing";
    }
}