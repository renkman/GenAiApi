namespace OpenLlamaAPI.Ai;

public record OllamaConfig
{
    public required Uri Uri { get; init; }
    public required TimeSpan TimeoutInMinutes { get; init; }
}