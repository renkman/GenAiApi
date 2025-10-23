using OpenLlamaAPI.Contracts;

namespace OpenLlamaAPI.Ai;

using Microsoft.Extensions.AI;

public class GenericAiClient : IGenericAiClient
{
    private readonly IChatClient _chatClient;

    public GenericAiClient(IChatClient chatClient)
    {
        _chatClient = chatClient;
    }

    public async Task<string> ChatAsync(ChatInput chatInput)
    {
        var chatMessage = chatInput.ToChatMessage();
        var response = await _chatClient.GetResponseAsync(chatMessage);
        return response.Text;
    }
}