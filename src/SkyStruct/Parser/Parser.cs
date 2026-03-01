using SkyStruct.Lexer;

namespace SkyStruct.Parser;

public class Parser
{
    private readonly IEnumerator<Token> _tokens;
    private Token _currentToken;
    private bool _finished;
    
    public Parser(IEnumerable<Token> tokens)
    {
        _currentToken = null!;
        _finished = false;
        _tokens = tokens.GetEnumerator();
        Advance();
    }

    private void Advance()
    {
        if (_tokens.MoveNext())
            _currentToken = _tokens.Current!;
        else
            _finished = true;
    }

    public List<TypeNode> Parse()
    {
        var dataContracts = new List<TypeNode>();

        while (!_finished)
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

        var constraints = CheckNextToken() switch
        {
            (TokenType.Constraint, _) => ConsumeConstraints(),
            _ => []
        };
        
        var dataType = Consume(TokenType.DataType).Value;
        var name = Consume(TokenType.Identifier).Value;
        return new PropertyNode { Name = name, DataType = dataType,  Constraints = constraints };
    }
    
    private Token Consume(TokenType? expectedType, string? expectedValue = null)
    {
        if (expectedValue is not null && _currentToken.Value != expectedValue)
            throw new Exception($"Expected {expectedValue} but found {_currentToken.Value} at line {_currentToken.LineNumber} column {_currentToken.ColumnStart}");
        if (expectedType is not null && _currentToken.Type != TokenType.NotResolved && _currentToken.Type != expectedType)
            throw new Exception($"Expected {expectedType} but found {_currentToken.Type} at line {_currentToken.LineNumber} column {_currentToken.ColumnStart}");
        
        var token = _currentToken;
        Advance();
        return token; 
    }

    private List<Constraint> ConsumeConstraints()
    {
        //Todo: implement constraints
        return [];
    }

    private (TokenType, string) CheckNextToken()
    {
        return (_currentToken.Type, _currentToken.Value);
    }
}
