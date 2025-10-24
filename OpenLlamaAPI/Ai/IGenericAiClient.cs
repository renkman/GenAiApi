using OpenLlamaAPI.Contracts;

namespace OpenLlamaAPI.Ai;

public interface IGenericAiClient
{
    Task<string> ChatAsync(ChatInput chatInput);
    Task<string> AnalyzeImage(string image);
}