using System;
using System.Collections.Generic;

namespace Tokenizing
{
    /// <summary>
    /// Used to read characters from a code, check conditions to read character and identifies if reaches an end of line or an end of file.
    /// </summary>
    public class CodeReader
    {
        /// <summary>
        /// Code to be read.
        /// </summary>
        IEnumerable<char> code;

        /// <summary>
        /// Character of reader current position.
        /// </summary>
        char current;

        /// <summary>
        /// Enumerator used to travel through code enumerable. This enumerator is always positioned at next character.
        /// </summary>
        IEnumerator<char> enumerator;

        /// <summary>
        /// Start position of current line to compute column position.
        /// </summary>
        int startLinePosition;

        /// <summary>
        /// Initializes a code reader by an IEnumerable object of code characters.
        /// </summary>
        /// <param name="code"></param>
        public CodeReader(IEnumerable<char> code)
        {
            this.code = code;

            this.Position = -1;
            this.startLinePosition = 0;
            this.enumerator = code.GetEnumerator();
            this.HasNext = this.enumerator.MoveNext();
            this.current = '\0';
        }

        /// <summary>
        /// Gets whenever the end of file is reached.
        /// </summary>
        public bool EndOfFile { get; private set; }

        /// <summary>
        /// Gets whenever the end of a line is reached.
        /// </summary>
        public bool EndOfLine { get { return current == '\n'; } }

        /// <summary>
        /// Gets current character of reader.
        /// </summary>
        public char Current
        {
            get
            {
                if (Position == -1)
                    throw new InvalidOperationException();
                return current;
            }
        }

        /// <summary>
        /// Gets next character to be read.
        /// </summary>
        public char Next
        {
            get
            {
                return enumerator.Current;
            }
        }

        /// <summary>
        /// Determines if a next character exists.
        /// </summary>
        public bool HasNext { get; private set; }

        /// <summary>
        /// Gets the position (in characters) of the current character relative to the code.
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// Gets the line of current character relative to the code lines.
        /// </summary>
        public int Line { get; private set; }

        /// <summary>
        /// Gets the column of current character relative to the current line.
        /// </summary>
        public int Column { get { return Position - startLinePosition; } }

        /// <summary>
        /// Moves the position of the reader to the next character.
        /// If ends of file has been reached, an InvalidOperationException is thrown.
        /// If there is no a next character, current character is nulled and eof condition is reached.
        /// </summary>
        public void MoveHead()
        {
            if (EndOfFile) throw new InvalidOperationException();

            if (!HasNext)
            {
                EndOfFile = true;
                current = '\0';
                Position++;
                return;
            }

            if (EndOfLine) // new line
            {
                Line++;
                startLinePosition = Position + 1;
            }

            current = enumerator.Current;
            HasNext = enumerator.MoveNext();
            Position++;
        }

        /// <summary>
        /// Returns current character and move head to next character.
        /// </summary>
        public char Consume()
        {
            var result = current;

            MoveHead();

            return result;
        }

        /// <summary>
        /// Moves the head to next character if current character equals specified character, stays in place otherwise.
        /// </summary>
        /// <returns>True if characters matches, false otherwise.</returns>
        public bool TryMatch(char c)
        {
            if (EndOfFile)
                return false;

            if (current == c)
            {
                Consume();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Allows to check current reader character (if exist) using a predicate.
        /// </summary>
        public bool CheckCurrent(Func<char, bool> predicate)
        {
            return !EndOfFile && predicate(current);
        }
    }
}
