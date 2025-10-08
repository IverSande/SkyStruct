using SkyStruct.Lexer;

namespace SkyStruct.Parser;

public class Parser
{
    private readonly IEnumerator<Token> _tokens;
    private Token _currentToken;

    public Parser(IEnumerable<Token> tokens)
    {
        _tokens = tokens.GetEnumerator();
        Advance();
    }

    private void Advance()
    {
        if (_tokens.MoveNext())
            _currentToken = _tokens.Current;
        else
            _currentToken = new Token(TokenType.EndOfInput, "END");
    }

    public List<TypeNode> Parse()
    {
        var dataContracts = new List<TypeNode>();

        while (_currentToken.Type != TokenType.EndOfInput)
        {
            var dataContract = ParseType();
            dataContracts.Add(dataContract);
        }

        return dataContracts;
    }

    private TypeNode ParseType()
    {
        Consume(TokenType.Keyword, "Define");

        var name = Consume(TokenType.Identifier).Value;
        var typeNode = new TypeNode { Name = name };

        Consume(TokenType.Keyword, "with");

        while (_currentToken.Type != TokenType.EndOfInput && _currentToken.Type != TokenType.Keyword)
        {
            var property = ParseProperty();
            typeNode.Properties.Add(property);
        }

        return typeNode;
    }

    private PropertyNode ParseProperty()
    {
        var dataType = Consume(TokenType.DataType).Value;
        var name = Consume(TokenType.Identifier).Value;
        return new PropertyNode { Name = name, DataType = dataType };
    }

    private Token Consume(TokenType expectedType, string expectedValue = null)
    {
        if (_currentToken.Type != expectedType || (expectedValue != null && _currentToken.Value != expectedValue))
            throw new Exception($"Unexpected token: {_currentToken.Type}, {_currentToken.Value}");

        var token = _currentToken;
        Advance();
        return token;
    }
}
