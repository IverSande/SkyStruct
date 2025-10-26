
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
        var tokenValue = new List<char>();
        int intChar;

        while ((intChar = _input.Read()) != -1)
        {
            var currentChar = (char) intChar;
            
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
                            tokenValue.Add(currentChar);
                            break;
                        case '}' : 
                            _currentState = LexerState.EndType;
                            tokenValue.Add(currentChar);
                            break;
                        case '\r':
                            break;
                        case '\n':
                            break;
                        default :
                            _currentState = LexerState.Identifier; 
                            tokenValue.Add(currentChar);
                            break;
                    }
                    break;

                
                case LexerState.Whitespace:
                    if (!char.IsWhiteSpace(currentChar))
                    {
                        _currentState = LexerState.Default;
                        tokenValue.Add(currentChar);
                    }
                    break;
                
                case LexerState.Identifier:
                    if (char.IsLetterOrDigit(currentChar))
                    {
                        tokenValue.Add(currentChar);
                    }
                    else
                    {
                        var token = RecognizeToken(new string(tokenValue.ToArray()));
                        if(token is not null)
                            yield return token;
                        tokenValue.Clear();
                        if(IsDelimiter(currentChar))
                            tokenValue.Add(currentChar);
                        _currentState = LexerState.Default;
                    }
                    break;
                case LexerState.StartType:
                    _currentState = LexerState.Default;
                    yield return new Token(TokenType.Delimiter, new string(tokenValue.ToArray()));
                    tokenValue.Clear();
                    break;
                case LexerState.EndType:
                    _currentState = LexerState.Default;
                    yield return new Token(TokenType.Delimiter, new string(tokenValue.ToArray()));
                    tokenValue.Clear();
                    break;
            }

        }
    
        // Handle the last token if any after exiting the loop
        if (tokenValue.Count > 0)
        {
            var idToken = new string(tokenValue.ToArray());
            var token = RecognizeToken(idToken);
            if(token is not null)
                yield return token;
        }
    
        yield return new Token(TokenType.EndOfInput, "END");
    }
    
    private Token? RecognizeToken(string value)
    {
        if (IsThrowAway(value))
            return null;
        return IsKeyword(value) ? new Token(TokenType.Keyword, value) : new Token(TokenType.Identifier, value);
    }
    
    private bool IsThrowAway(string value) =>
        value == "\r" || value == "\n";
    
    private bool IsDelimiter(char value) =>
        value == '{' || value == '}';
    
    private bool IsKeyword(string value) =>
        value == "Define" || value == "is";
    
}