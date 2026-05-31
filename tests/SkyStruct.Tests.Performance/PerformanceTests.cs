using System.Diagnostics;
using System.Reflection;

namespace SkyStruct.Tests.Performance;

[TestClass]
public sealed class PerformanceTests 
{
    [TestMethod]
    [DataRow("100")]
    [DataRow("1000")]
    [DataRow("10000")]
    [DataRow("100000")]
    public void PerformanceTest_OneType(string fileName)
    {
        List<long> times = [];
        for (int i = 0; i < 100; i++)
        {
            var sw = Stopwatch.StartNew();
            var fileStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"SkyStruct.Tests.Performance.TestFilesOneType.OneType{fileName}Rows.skystruct");

            // Act
            var lexer = new Lexer.Lexer(new StreamReader(fileStream!), new CancellationToken());
            var tokens = lexer.Tokenize().ToList();

            var parser = new Parser.Parser(tokens);
            var ast = parser.Parse();

            var codeGenerator = new CodeGenerator.Builder(ast, "HolyNamespace");
            _ = codeGenerator.BuildFile(); 
        
            sw.Stop();
            times.Add(sw.ElapsedMilliseconds);
            
        }

        // Assert
        Console.WriteLine(times.Average());
    }
    
    [TestMethod]
    [DataRow("100")]
    [DataRow("1000")]
    [DataRow("10000")]
    [DataRow("33334")]
    public void PerformanceTest_XTypes_OneField(string fileName)
    {
        List<long> times = [];
        for (int i = 0; i < 100; i++)
        {
            var sw = Stopwatch.StartNew();

            var fileStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream(
                    $"SkyStruct.Tests.Performance.TestFilesXTypesOneField.{fileName}TypesOneField.skystruct");

            var lexer = new Lexer.Lexer(new StreamReader(fileStream!), new CancellationToken());
            var tokens = lexer.Tokenize().ToList();

            var parser = new Parser.Parser(tokens);
            var ast = parser.Parse();

            var codeGenerator = new CodeGenerator.Builder(ast, "HolyNamespace");
            _ = codeGenerator.BuildFile();

            sw.Stop();
            times.Add(sw.ElapsedMilliseconds);
            
        }

        // Assert
        Console.WriteLine(times.Average());
    }
    
    [TestMethod]
    [DataRow("10")]
    [DataRow("100")]
    [DataRow("315")]
    public void PerformanceTest_XTypes_XFields(string fileName)
    {
        List<long> times = [];
        for (int i = 0; i < 100; i++)
        {
            var sw = Stopwatch.StartNew();

            var fileStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream(
                    $"SkyStruct.Tests.Performance.TestFilesXTypesXFields.{fileName}Types{fileName}Fields.skystruct");

            var lexer = new Lexer.Lexer(new StreamReader(fileStream!), new CancellationToken());
            var tokens = lexer.Tokenize().ToList();

            var parser = new Parser.Parser(tokens);
            var ast = parser.Parse();

            var codeGenerator = new CodeGenerator.Builder(ast, "HolyNamespace");
            _ = codeGenerator.BuildFile();

            sw.Stop();
            times.Add(sw.ElapsedMilliseconds);
        }

        // Assert
        Console.WriteLine(times.Average());
    }
}