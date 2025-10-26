using System.Reflection;

namespace SkyStruct.Tests;

[TestClass]
public class EndToEnd
{

    [TestMethod]
    public Task Test_TestFile()
    {
        var fileStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SkyStruct.Tests.Files.TestFile.skystruct");

        var lexer = new Lexer.Lexer(new StreamReader(fileStream!), new CancellationToken());
        var tokens = lexer.Tokenize().ToList();

        var parser = new Parser.Parser(tokens);
        var ast = parser.Parse();

        var codeGenerator = new CodeGenerator.Builder(ast, "HolyNamespace");
        var code = codeGenerator.BuildFile();

        var fileGenerator = new FileGenerator.FileGenerator();
        fileGenerator.GenerateFile(code, "TestFileGenerated.cs");

        return Task.CompletedTask;
    }
    
}