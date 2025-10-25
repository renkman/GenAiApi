using OpenLlamaAPI.Contracts;

namespace OpenLlamaAPI.Ai;

using Microsoft.Extensions.AI;

public class GenericAiClient : IGenericAiClient
{
    private readonly IChatClient _chatClient;
    private readonly ILogger<GenericAiClient> _logger;

    public GenericAiClient(IChatClient chatClient, ILogger<GenericAiClient> logger)
    {
        _chatClient = chatClient;
        _logger = logger;
    }

    public async Task<string> ChatAsync(Prompt prompt, CancellationToken ct)
    {
        var chatMessage = prompt.ToChatMessage();
        var response = await _chatClient.GetResponseAsync(chatMessage, cancellationToken: ct);
        return response.Text;
    }

    public async Task<string> AnalyzeImage(byte[] image, CancellationToken ct)
    {
        var prompt = new TextContent(Prompts.WeatherPrompt);
        var imageContent = new DataContent(image, "image/jpeg");
        var chatMessage = new ChatMessage(ChatRole.User, [prompt, imageContent]);
        
        _logger.LogInformation("Send {ImageSize} bytes large image to AI model.", image.Length);
        var response = await _chatClient.GetResponseAsync(chatMessage, cancellationToken: ct);
        return response.Text;
    }
}