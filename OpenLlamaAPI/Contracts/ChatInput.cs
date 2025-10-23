using Microsoft.Extensions.AI;

namespace OpenLlamaAPI.Contracts;

public record ChatInput
{
    public required string Role { get; init; }
    public required string Message  { get; init; }

    public ChatMessage ToChatMessage() =>
        new ChatMessage(MapChatRole(Role), Message);

    private static ChatRole MapChatRole(string role) =>
        role.ToLowerInvariant() switch
        {
            "user" => ChatRole.User,
            "assistant" => ChatRole.Assistant,
            "system" => ChatRole.System,
            "tool" => ChatRole.Tool,
            _ => throw new ArgumentOutOfRangeException(nameof(role), role)
        };
}
