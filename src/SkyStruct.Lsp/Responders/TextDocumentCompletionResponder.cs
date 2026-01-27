using System.Text.Json;

namespace SkyStruct.Lsp.Responders;

public static class TextDocumentCompletionResponder
{
    public static string TextDocumentCompletionResponse(dynamic input)
    {
        return JsonSerializer.Serialize(new
        {
            id = input.id,
            result = new { capabilities = new { textDocumentSync = 1, completionProvider = new { items = GetCompletions() } } }
        });
    }
    
    private static dynamic[] GetCompletions()
    {
        return
        [
            new { label = "example1", kind = 1 },
            new { label = "example2", kind = 1 }
        ];
    }
}