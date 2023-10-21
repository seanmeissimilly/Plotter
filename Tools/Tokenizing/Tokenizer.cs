using System;
using System.Collections.Generic;
using System.Linq;

namespace Tokenizing
{
    /// <summary>
    /// Represents a tokenizer for MSharp language.
    /// Allows to separate a code into tokens of specific types, literals, keywords, separators, symbols, etc.
    /// </summary>
    public class Tokenizer
    {
        #region Tokenizer Settings

        public Tokenizer()
        {
        }

        /// <summary>
        /// Collection populated with supported keywords in the language.
        /// </summary>
        ICollection<string> _keywords = new HashSet<string>()
        {
            "let",
            "set",
            "def",
            "print",
            "read",
            "graph",
            "if",
            "else",
            "true",
            "include",
            "false"
        };

        /// <summary>
        /// Collection populated with supported symbols in the language
        /// </summary>
        ICollection<char> _symbols = new HashSet<char>()
        {
            '+', '-', '*', '%', '/', '^', '(', ')', '=','`'
        };

        ICollection<char> _whiteSpaces = new HashSet<char>()
        {
            ' ', '\t', '\r', '\n'
        };

        ICollection<char> _comparetor = new HashSet<char>()
        {
            '>', '<', '=', '!'
        };

        ICollection<char> _textLiteralDelimiters = new HashSet<char>()
        {
            '\"'
        };

        /// <summary>
        /// When Determines if a character is a comparetor.
        /// </summary>
        bool IsComparetorChar(char c)
        {
            return _comparetor.Contains(c);
        }

        bool IsCaseSensitive = true;

        /// <summary>
        /// When Determines if a character is an instruction separator. I.e. ; character in C#.
        /// This implementations return true if new-line character is found.
        /// </summary>
        bool IsSeparatorChar(char c)
        {
            return c == '\n';
        }

        /// <summary>
        /// Determines if a character is a valid identifier char.
        /// This implementation supports identifiers using digits and letters.
        /// </summary>
        bool IsValidIdentifierChar(char c)
        {
            return char.IsLetterOrDigit(c);
        }

        /// <summary>
        /// Determines if a character is a valid identifier start character.
        /// This implementation supports identifiers starting with a letter.
        /// </summary>
        bool IsValidIdentifierStartChar(char c)
        {
            return char.IsLetter(c);
        }

        /// <summary>
        /// Gets whenever a text is considered a keyword using registrated keywords.
        /// </summary>
        bool IsKeyword(string text)
        {
            return _keywords.Contains(text, IsCaseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets whenever a char is a white-space character such as spaces, tabs, ends of lines.
        /// This implementation supports languages with space, tab, and new lines white spaces.
        /// </summary>
        bool IsEmptySpaceChar(char c)
        {
            return _whiteSpaces.Contains(c);
        }

        /// <summary>
        /// Determines if a character is considered a symbol in the language using registrated symbols. 
        /// </summary>
        bool IsSymbol(char c)
        {
            return _symbols.Contains(c);
        }

        /// <summary>
        /// Determines if a char can be the beggining of a literal text. I.e. " for C# or " and ' for JavaScript.
        /// This implementation supports languages with " starting literal texts.
        /// </summary>
        bool IsTextLiteralDelimiterChar(char c)
        {
            return _textLiteralDelimiters.Contains(c);
        }

        #endregion

        #region Error Handling

        /// <summary>
        /// Occurs when an error is raised from tokenizing operation.
        /// </summary>
        public event ErrorHandler Error;

        /// <summary>
        /// Raises Error event reporting messagen and position at code the error occurred.
        /// </summary>
        protected void OnError(string message, int line, int column)
        {
            if (Error != null)
                Error(message, line, column);
        }

        #endregion

        #region Reading Tokens from Code

        /// <summary>
        /// Separate a string with the code into several tokens specifying the content of the token, the kind and position on the code.
        /// </summary>
        /// <param name="code">A code in a specific language to be tokenized.</param>
        /// <returns>A IEnumerable object with all token decomposition of code.</returns>
        public IEnumerable<Token> GetTokens(string code)
        {

            // Creates a reader of the code. This object allow to consume characters, check conditions and determine whenever a new line or end of file is detected.
            CodeReader reader = new CodeReader(code);

            // If there is no characted on the code we get an empty enumerable.
            if (reader.EndOfFile)
                yield break;


            // Move reader to first character of the code.
            reader.MoveHead();

            // While reader has code to read.
            while (!reader.EndOfFile)
            {
                // save current character to check what kind of token should be read.
                char current = reader.Current;

                // The current character is an instruction-separator char that should be read.
                if (IsSeparatorChar(current))
                {
                    yield return ReadSeparator(reader);
                    continue;
                }

                // The current character is an instruction-separator char that should be read.
                if (IsComparetorChar(current))
                {
                    yield return ReadComparetor(reader);
                    continue;
                }

                // The current character is a white-space character.
                if (IsEmptySpaceChar(current))
                {
                    reader.Consume(); // just consume the white-space character.
                    continue;
                }

                // Current and next characters are slahes, identifying comment starting that should be read.
                if (current == '/' && reader.HasNext && reader.Next == '/')
                {
                    yield return ReadComment(reader);
                    continue;
                }

                // Current character is a symbol and should be read.
                if (IsSymbol(current))
                {
                    yield return ReadSymbol(reader);
                    continue;
                }

                // Current character is a text start character so literal text should be read.
                if (IsTextLiteralDelimiterChar(current))
                {
                    yield return ReadTextLiteral(reader);
                    continue;
                }

                // Current character is a valid identifier begginig that should be read.
                if (IsValidIdentifierStartChar(current))
                {
                    yield return ReadIdentifierOrKeyword(reader);
                    continue;
                }

                // Current character is a digit, representing a number that should be read.
                if (reader.CheckCurrent(char.IsDigit)) // read a number
                {
                    yield return ReadNumber(reader);
                    continue;
                }

                // Reaches an unknown character that is returned as a unknown token type.
                yield return ReadUnknownSymbol(reader);

                
            }
        }

        private Token ReadComparetor(CodeReader reader)
        {
            int tokenLine = reader.Line;
            int tokenColumn = reader.Column;
            return new Token(reader.Consume().ToString(), TokenKind.Comparetor, tokenLine, tokenColumn);
        }


        /// <summary>
        /// Matches an expected character with current character of the reader. If doesnt match an error will be reported.
        /// </summary>
        private void Match(CodeReader reader, char c)
        {
            if (!reader.TryMatch(c))
                OnError("Expected character " + c, reader.Line, reader.Column);
        }

        /// <summary>
        /// Reads an unknown symbol token.
        /// </summary>
        private Token ReadUnknownSymbol(CodeReader reader)
        {
            int tokenLine = reader.Line;
            int tokenColumn = reader.Column;

            var token = new Token(reader.Consume().ToString(), TokenKind.Unknown, tokenLine, tokenColumn);

            OnError("Unknown symbol " + token.Text, tokenLine, tokenColumn);

            return token;
        }

        /// <summary>
        /// Reads an instruction-separator token.
        /// </summary>
        private Token ReadSeparator(CodeReader reader)
        {
            int tokenLine = reader.Line;
            int tokenColumn = reader.Column;

            return new Token(reader.Consume().ToString(), TokenKind.Separator, tokenLine, tokenColumn);
        }        

        private Token ReadTextLiteral(CodeReader reader)
        {
            Match(reader, '\"');
            string text = "";
            int tokenLine = reader.Line;
            int tokenColumn = reader.Column;

            while (!reader.EndOfFile && !reader.EndOfLine)
            {
                if (reader.TryMatch('\"'))
                    return new Token(text, TokenKind.Text, tokenLine, tokenColumn);

                text += reader.Consume();
            }

            OnError("Invalid text literal ending", tokenLine, tokenColumn + text.Length);

            return new Token(text, TokenKind.Text, tokenLine, tokenColumn);
        }

        private Token ReadSymbol(CodeReader reader)
        {
            int tokenLine = reader.Line;
            int tokenColumn = reader.Column;
            return new Token(reader.Consume().ToString(), TokenKind.Symbol, tokenLine, tokenColumn);
        }

        private Token ReadNumber(CodeReader reader)
        {
            string number = "";
            int tokenLine = reader.Line;
            int tokenColumn = reader.Column;

            while (reader.CheckCurrent(char.IsDigit))
                number += reader.Consume();

            if (reader.TryMatch('.'))
            {
                number += '.';
                while (reader.CheckCurrent(char.IsDigit))
                    number += reader.Consume();
            }

            double result;
            if (!double.TryParse(number, out result))
                OnError("Invalid number", tokenLine, tokenColumn);

            return new Token(number, TokenKind.Number, tokenLine, tokenColumn);
        }

        private Token ReadComment(CodeReader reader)
        {
            string comment = "";
            int tokenLine = reader.Line;
            int tokenColumn = reader.Column;

            Match(reader, '\\');
            Match(reader, '\\');

            while (!reader.EndOfFile || !reader.EndOfLine)
                comment += reader.Consume();

            return new Token(comment, TokenKind.Comment, tokenLine, tokenColumn);
        }

        private Token ReadIdentifierOrKeyword(CodeReader reader)
        {
            string identifier = "";
            int tokenLine = reader.Line;
            int tokenColumn = reader.Column;

            if (!reader.CheckCurrent(IsValidIdentifierStartChar))
                OnError("Invalid identifier name start", tokenLine, tokenColumn);

            identifier += reader.Consume();

            while (reader.CheckCurrent(IsValidIdentifierChar))
                identifier += reader.Consume();

            return new Token(identifier, IsKeyword(identifier) ? TokenKind.Keyword : TokenKind.Identifier, tokenLine, tokenColumn);
        }

        #endregion
    }

    /// <summary>
    /// Represents methods that serves as error handlers.
    /// </summary>
    /// <param name="errorMessage">Error message text.</param>
    /// <param name="line">Line of the code the error occurs.</param>
    /// <param name="col">Column of the code the error occurs.</param>
    public delegate void ErrorHandler(string errorMessage, int line, int col);  

}
