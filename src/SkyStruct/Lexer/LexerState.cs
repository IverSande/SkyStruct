namespace SkyStruct.Lexer;

public enum LexerState
{ 
        Default,
        Keyword,
        StartType,
        EndType,
        Identifier, 
        Whitespace
}