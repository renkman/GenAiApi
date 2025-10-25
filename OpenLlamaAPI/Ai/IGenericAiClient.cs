using OpenLlamaAPI.Contracts;

namespace OpenLlamaAPI.Ai;

public interface IGenericAiClient
{
    Task<string> ChatAsync(Prompt prompt, CancellationToken ct);
    Task<string> AnalyzeImage(byte[] image, CancellationToken ct);
}