using System.Text;

namespace SkyStruct;

public class Worker(ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var input = "Define Person { Text Name Number Age } Define Employee { Text EmployeeID Text Title } End";
        var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(input));
        var streamReader = new StreamReader(inputStream);
        
        var lexer = new Lexer.Lexer(streamReader, cancellationToken);
        var tokens = lexer.Tokenize();
        
        var parser = new Parser.Parser(tokens);
        var dataContracts = parser.Parse();
        
        foreach (var contract in dataContracts)
        {
            Console.WriteLine($"Data Contract: {contract.Name}, Inherits: {contract.Parent}");
            foreach (var property in contract.Properties)
            {
                Console.WriteLine($"  Property: {property.Name}, Type: {property.DataType}");
            }
        }
    }
}
