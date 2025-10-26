using System.Diagnostics.CodeAnalysis;
using SkyStruct.Lexer;

namespace SkyStruct.Parser;

public class Parser
{
    private readonly IEnumerator<Token> _tokens;
    private Token _currentToken = null!;
    
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
        
        _tokens.Dispose();
        return dataContracts;
    }

    private TypeNode ParseType()
    {
        //Should always be the start of a type
        Consume(TokenType.Keyword, "Define");
        var name = Consume(TokenType.Identifier).Value;
        var typeNode = new TypeNode { Name = name };

        if (CheckNextToken() is (TokenType.Keyword, "is"))
        {
            Consume(TokenType.Keyword, "is");
            var inheritedType = Consume(TokenType.Inherited);
            typeNode.InheritedType = inheritedType.Value;
        }

        Consume(TokenType.Delimiter, "{");

        while (_currentToken.Type != TokenType.Delimiter && _currentToken.Type != TokenType.Keyword)
        {
            var property = ParseProperty();
            typeNode.Properties.Add(property);
        }
        
        Consume(TokenType.Delimiter, "}");

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
        var token = _currentToken;
        if(expectedValue is not null && expectedValue != token.Value)
            throw new Exception($"Expected {expectedValue} but got {token.Value}");
        Advance();
        return token with { Type = expectedType };
    }

    private (TokenType, string) CheckNextToken()
    {
        return (_currentToken.Type, _currentToken.Value);
    }
}
