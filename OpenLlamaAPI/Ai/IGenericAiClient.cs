using OpenLlamaAPI.Contracts;

namespace OpenLlamaAPI.Ai;

public interface IGenericAiClient
{
    Task<string> ChatAsync(ChatInput chatInput, CancellationToken ct);
    Task<string> AnalyzeImage(byte[] image, CancellationToken ct);
}