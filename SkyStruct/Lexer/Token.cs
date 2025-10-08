namespace SkyStruct.Lexer;

public class Token
{
    public Token(TokenType type, string value)
    {
        Type = type;
        Value = value;
    }

    public TokenType Type { get; set; }
    public string Value { get; set; }
}

public enum TokenType
{
    Keyword,
    Identifier,
    DataType,
    Delimiter,
    EndOfInput
}
