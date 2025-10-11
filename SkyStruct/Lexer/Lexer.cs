
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
                        _currentState = LexerState.Default;
                    }
                    break;
                case LexerState.EndType:
                    _currentState = LexerState.Default;
                    yield return new Token(TokenType.Delimiter, new string(tokenValue.ToArray())) ;
                    tokenValue.Clear();
                    break;
                case LexerState.StartType:
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
            yield return RecognizeToken(idToken)!;
        }
    
        yield return new Token(TokenType.EndOfInput, "END");
    }
    
    private Token? RecognizeToken(string value)
    {
        if (IsWhitespace(value))
            return null;
        return IsKeyword(value) ? new Token(TokenType.Keyword, value) : new Token(TokenType.Identifier, value);
    }
    
    private bool IsWhitespace(string value) =>
        value == "\r" || value == "\n";
    
    private bool IsKeyword(string value) =>
        value == "Define" || value == "inherits";
    
}