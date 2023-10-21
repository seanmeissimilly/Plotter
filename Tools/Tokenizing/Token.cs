namespace Tokenizing
{
    /// <summary>
    /// Represents a token in a code.
    /// </summary>
    public struct Token
    {
        /// <summary>
        /// Gets the token text. This is the representative text of the token.
        /// </summary>
        public string Text { get; private set; }
        /// <summary>
        /// Gets the token line position on code.
        /// </summary>
        public int Line { get; private set; }
        /// <summary>
        /// Gets the token column position on code.
        /// </summary>
        public int Column { get; private set; }
        /// <summary>
        /// Gets the token kind.
        /// </summary>
        public TokenKind Type { get; private set; }

        /// <summary>
        /// Initializes a token from a string, its kind and position on code.
        /// </summary>
        internal Token(string text, TokenKind type, int line, int column)
            : this()
        {
            this.Text = text;
            this.Type = type;
            this.Line = line;
            this.Column = column;
        }
    }

    /// <summary>
    /// Represents the different kind of tokens for a basic programming language.
    /// </summary>
    public enum TokenKind
    {
        /// <summary>
        /// Identifies a token of an unknown symbol.
        /// </summary>
        Unknown,
        /// <summary>
        /// Identifies a token of an identifier in the code.
        /// </summary>
        Identifier,
        /// <summary>
        /// Identifies a token of a literal number in the code.
        /// </summary>
        Number,
        /// <summary>
        /// Identifies a token of a literal text in the code.
        /// </summary>
        Text,
        /// <summary>
        /// Identifies a token of a symbol in the code.
        /// </summary>
        Symbol,
        /// <summary>
        /// Identifies a token of a keyword in the code.
        /// </summary>
        Keyword,
        /// <summary>
        /// Identifies a token of a comment in the code.
        /// </summary>
        Comment,
        /// <summary>
        /// Identifies a token of an instruction seperator in the code.
        /// </summary>
        Separator,

        /// <summary>
        /// Identifies a token of a comparetor.
        /// </summary>
        Comparetor
    }
}
