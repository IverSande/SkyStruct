namespace SkyStruct.Lexer;

public record Token
{
    public Token(TokenType type, string value, int lineNumber, int columnStart, int columnEnd)
    {
        Type = type;
        Value = value;
        LineNumber = lineNumber;
        ColumnStart = columnStart;
        ColumnEnd = columnEnd;
    }

    public TokenType Type { get; set; }
    public string Value { get; set; }
    public int LineNumber { get; set; }
    public int ColumnStart { get; set; }
    public int ColumnEnd { get; set; }
}

public enum TokenType
{
    Keyword,
    Identifier,
    DataType,
    Delimiter,
    EndOfInput,
    Inherited
}
