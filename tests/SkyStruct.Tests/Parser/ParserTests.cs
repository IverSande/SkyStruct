using SkyStruct.Lexer;
using SkyStruct.Parser;

namespace SkyStruct.Tests.Parser;

[TestClass]
public class ParserTests
{

    [TestMethod]
    public async Task ParseTokensAsync_HappyPath_BasicDefinition()
    {
        List<Token> input = [TokenGen(TokenType.Keyword, "Define"), TokenGen(TokenType.Identifier, "Person"), TokenGen(TokenType.Delimiter, "{"),  TokenGen(TokenType.Delimiter, "}")];
        var parser = new SkyStruct.Parser.Parser(input);
        var nodes = parser.Parse();
        Assert.IsNotNull(nodes);
        Assert.HasCount(1, nodes);
        Assert.AreEqual("Person", nodes[0].Name);
        Assert.HasCount(0, nodes[0].Properties);
    }




    public Token TokenGen(TokenType tokenType, string? value)
    {
        return new Token(tokenType, value ?? "", 0, 0, 0);
    }
    
}