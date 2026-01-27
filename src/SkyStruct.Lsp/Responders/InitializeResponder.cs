using System.Text.Json;

namespace SkyStruct.Lsp.Responders;

public static class InitializeResponder
{
    public static string InitializeResponse(dynamic input)
    {
        return JsonSerializer.Serialize(new
        {
            id = input.id,
            result = new { capabilities = new { textDocumentSync = 1, completionProvider = new { } } }
        });
    }
}