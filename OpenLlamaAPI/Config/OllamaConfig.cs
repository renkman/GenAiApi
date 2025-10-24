namespace OpenLlamaAPI.Config;

public record OllamaConfig
{
    public required Uri Uri { get; set; }
    public required TimeSpan TimeoutInMinutes { get; set; }
}