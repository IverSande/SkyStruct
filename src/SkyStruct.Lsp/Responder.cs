using SkyStruct.Lsp.Responders;

namespace SkyStruct.Lsp;

public static class Responder
{
    public static string RespondInitialize(dynamic input) =>
        InitializeResponder.InitializeResponse(input);

    public static string RespondTdCompletion(dynamic input) =>
        TextDocumentCompletionResponder.TextDocumentCompletionResponse(input);
}