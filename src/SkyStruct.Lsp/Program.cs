
namespace SkyStruct.Lsp;

internal static class Program
{
    static void Main(string[] args)
    {
        var stdin = Console.OpenStandardInput();
        var stdout = Console.OpenStandardOutput();

        using var reader = new StreamReader(stdin);
        using var writer = new StreamWriter(stdout);
        //var languageServer = new LanguageServer(reader, writer);
        //languageServer.Start();
    }
}