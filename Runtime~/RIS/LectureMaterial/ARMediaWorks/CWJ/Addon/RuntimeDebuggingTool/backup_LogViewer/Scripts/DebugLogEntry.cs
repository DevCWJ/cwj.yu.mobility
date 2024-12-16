using UnityEngine;

namespace CWJ.RuntimeDebugging
{
    public class DebugLogEntry : System.IEquatable<DebugLogEntry>
    {
        private const int HASH_NOT_CALCULATED = -623218;

        public string logString;
        public string stackTrace;

        private string completeLog = null;

        public Sprite logTypeSpriteRepresentation;

        public int count;

        private int hashValue = HASH_NOT_CALCULATED;

        public DebugLogEntry(string logString, string stackTrace, Sprite sprite)
        {
            this.logString = logString;
            this.stackTrace = stackTrace;

            logTypeSpriteRepresentation = sprite;

            count = 1;
        }

        public bool Equals(DebugLogEntry other)
        {
            return this.logString == other.logString && this.stackTrace == other.stackTrace;
        }

        public override string ToString()
        {
            if (completeLog == null)
                completeLog = string.Concat(logString, "\n", stackTrace);

            return completeLog;
        }

        public override int GetHashCode()
        {
            if (hashValue == HASH_NOT_CALCULATED)
            {
                unchecked
                {
                    hashValue = 17;
                    hashValue = hashValue * 23 + logString == null ? 0 : logString.GetHashCode();
                    hashValue = hashValue * 23 + stackTrace == null ? 0 : stackTrace.GetHashCode();
                }
            }

            return hashValue;
        }
    }
}