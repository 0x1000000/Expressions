using System;

namespace Expression.Arithmetic
{
    public class ParserException : Exception
    {
        public static ParserException Build(string message, ArraySegment<char> str, int segmentIndex)
        {
            var originalIndex = str.Offset + segmentIndex;

            var sourceText = str.Array != null ? new string(str.Array).Insert(originalIndex, "->") : null;

            return new ParserException(BuildMessage(message, sourceText, originalIndex), originalIndex, sourceText);
        }

        private static string BuildMessage(string message, string? sourceText, int index)
        {
            return $"{message} Col. {index} Input: {sourceText}";
        }

        private ParserException(string message, int col, string? sourceText) : base(message)
        {
            this.Col = col;
            this.SourceText = sourceText;
        }

        public int Col { get; private set; }

        public string? SourceText { get; private set; }
    }
}