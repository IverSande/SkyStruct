using System.Text.Json;

namespace SkyStruct.Lsp;

public class LanguageServer(StreamReader reader, StreamWriter writer)
{
    public void Start()
    {
        while (true)
        {
            var message = reader.ReadLine();
            if (message != null)
            {
                HandleMessage(message);
            }
        }
    }

    private void HandleMessage(string message)
    {
        var response = ProcessMessage(message);
        SendResponse(response);
    }

    private string ProcessMessage(string message)
    {
        var parsedMessage = JsonSerializer.Deserialize<dynamic>(message);

        return parsedMessage?.method switch
        {
            "initialize" => Responder.RespondInitialize(parsedMessage),
            "textDocument/completion" => Responder.RespondTdCompletion(parsedMessage),
            _ => null
        } ?? string.Empty;
    }

    private void SendResponse(string response)
    {
        writer.WriteLine(response);
        writer.Flush();
    }
}