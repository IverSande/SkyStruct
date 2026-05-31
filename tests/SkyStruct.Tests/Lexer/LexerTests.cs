using System.Text;
using SkyStruct.Lexer;

namespace SkyStruct.Tests.Lexer;

[TestClass]
public class LexerTests
{

    [TestMethod]
    [DataRow("{", TokenType.Delimiter)]
    [DataRow("}", TokenType.Delimiter)]
    [DataRow("Define", TokenType.Keyword)]
    [DataRow("is", TokenType.Keyword)]
    [DataRow("optional", TokenType.Constraint)]
    [DataRow("required", TokenType.Constraint)]
    [DataRow("decimal", TokenType.Constraint)]
    [DataRow("list", TokenType.Constraint)]
    [DataRow("Type", TokenType.NotResolved)]
    public async Task RecognizeTokensAsync_HappyPath(string input, TokenType expectedTokenType)
    {
        byte[] byteArray = Encoding.UTF8.GetBytes(input);

        using (MemoryStream stream = new MemoryStream(byteArray))
        {
            var streamReader = new StreamReader(stream);
            var lexer = new SkyStruct.Lexer.Lexer(streamReader, CancellationToken.None);
            var token = lexer.Tokenize().First();
            
            Assert.AreEqual(expectedTokenType, token.Type);
        }
    }
    
    [TestMethod]
    [DataRow("\n")]
    [DataRow("\t")]
    [DataRow("      ")]
    public async Task RecognizeTokensAsync_NegativePath(string input)
    {
        byte[] byteArray = Encoding.UTF8.GetBytes(input);

        using (MemoryStream stream = new MemoryStream(byteArray))
        {
            var streamReader = new StreamReader(stream);
            var lexer = new SkyStruct.Lexer.Lexer(streamReader, CancellationToken.None);
            var tokens = lexer.Tokenize();
            
            Assert.AreEqual(0, tokens.Count());
        }
    }
    
}