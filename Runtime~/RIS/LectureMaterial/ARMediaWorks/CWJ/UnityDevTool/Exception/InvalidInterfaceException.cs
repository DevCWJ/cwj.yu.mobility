using System;

namespace CWJ
{
    public class InvalidInterfaceException : InvalidOverrideException
    {
        public InvalidInterfaceException(string message, string cause) : base(message, cause) { }

        public InvalidInterfaceException(string message, string cause, Exception innerException) : base(message, cause, innerException) { }

        public override string Message => "Your code is override invalid interface";
    }
}