
namespace SkyStruct.Lexer;

public class Lexer
{
    private LexerState _currentState;
    private StreamReader _input;
    private CancellationToken _cancellationToken;
    
    public Lexer(StreamReader input, CancellationToken cancellationToken)
    {
        _input = input;
        _currentState = LexerState.Default;
        _cancellationToken = cancellationToken;
    }

    public IEnumerable<Token> Tokenize()
    {
        var tokenValue = new List<(char, int)>();
        int intChar;
        var currentLine = 1;
        var currentColumn = 1;

        while ((intChar = _input.Read()) != -1)
        {
            var currentChar = (char) intChar;
            currentColumn++;
            
            switch (_currentState)
            {
                case LexerState.Default:
                    switch (currentChar)
                    {
                        case ' ' :
                            _currentState = LexerState.Whitespace;
                            break;
                        case '{' : 
                            _currentState = LexerState.StartType;
                            tokenValue.Add((currentChar, currentColumn));
                            break;
                        case '}' : 
                            _currentState = LexerState.EndType;
                            tokenValue.Add((currentChar, currentColumn));
                            break;
                        case '\r':
                            break;
                        case '\n':
                            currentLine++;
                            currentColumn = 1;
                            break;
                        default :
                            _currentState = LexerState.Identifier; 
                            tokenValue.Add((currentChar, currentColumn));
                            break;
                    }
                    break;

                
                case LexerState.Whitespace:
                    if (!char.IsWhiteSpace(currentChar))
                    {
                        _currentState = LexerState.Default;
                        tokenValue.Add((currentChar, currentColumn));
                    }
                    break;
                
                case LexerState.Identifier:
                    if (char.IsLetterOrDigit(currentChar))
                    {
                        tokenValue.Add((currentChar, currentColumn));
                    }
                    else
                    {
                        var token = RecognizeToken(tokenValue, currentLine);
                        if(token is not null)
                            yield return token;
                        tokenValue.Clear();
                        if(IsDelimiter(currentChar))
                            tokenValue.Add((currentChar, currentColumn));
                        _currentState = LexerState.Default;
                    }
                    break;
                case LexerState.StartType:
                    _currentState = LexerState.Default;
                    yield return RecognizeToken(tokenValue, currentLine)!;
                    tokenValue.Clear();
                    break;
                case LexerState.EndType:
                    _currentState = LexerState.Default;
                    yield return RecognizeToken(tokenValue, currentLine)!;
                    tokenValue.Clear();
                    break;
            }

        }
    
        // Handle the last token if any after exiting the loop
        if (tokenValue.Count > 0)
        {
            var token = RecognizeToken(tokenValue, currentLine);
            if(token is not null)
                yield return token;
        }
    
    }
    
    private Token? RecognizeToken(List<(char, int)> values, int lineNumber)
    {
        var value = values.Aggregate("", (acc, cur) => acc + cur.Item1);
        var firstCharColumn = values.First().Item2;
        var lastCharColumn = values.Last().Item2;

        var tokenType = ResolveTokenType(value);

        return tokenType is null ? null : new Token((TokenType)tokenType, value, lineNumber, firstCharColumn, lastCharColumn);

        //return IsKeyword(value) ? new Token(TokenType.Keyword, value, lineNumber, firstCharColumn, lastCharColumn) : new Token(TokenType.Identifier, value, lineNumber, firstCharColumn, lastCharColumn);
    }
    
    private bool IsThrowAway(string value) =>
        value == "\r" || value == "\n";
    
    private bool IsDelimiter(char value) =>
        value == '{' || value == '}';
    
    private bool IsKeyword(string value) =>
        value == "Define" || value == "is";

    private TokenType? ResolveTokenType(string value) => value switch
    {
        "\r" => null,
        "\n" => null,
        "{" => TokenType.Delimiter,
        "}" => TokenType.Delimiter,
        "Define" => TokenType.Keyword,
        "is" => TokenType.Keyword,
        _ => TokenType.Identifier
    };

}