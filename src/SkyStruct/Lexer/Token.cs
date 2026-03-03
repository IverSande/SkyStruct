namespace SkyStruct.Lexer;

public record Token(TokenType Type, string Value, int LineNumber, int ColumnStart, int ColumnEnd)
{
    public TokenType Type { get; set; } = Type;
    public string Value { get; set; } = Value;
    public int LineNumber { get; set; } = LineNumber;
    public int ColumnStart { get; set; } = ColumnStart;
    public int ColumnEnd { get; set; } = ColumnEnd;
}

public enum TokenType
{
    Keyword,
    Identifier,
    DataType,
    Delimiter,
    EndOfInput,
    Inherited,
    Constraint,
    NotResolved
}
