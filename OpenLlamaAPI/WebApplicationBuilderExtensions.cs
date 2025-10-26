using Microsoft.Extensions.AI;
using OllamaSharp;
using OpenLlamaAPI.Ai;
using OpenLlamaAPI.VideoCapture;

namespace OpenLlamaAPI;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder RegisterOpenLlamaApi(this WebApplicationBuilder builder)
    {
        var ollamaConfig = builder.Configuration.GetSection("Ollama").Get<OllamaConfig>() ??
                           throw new InvalidOperationException("Missing ollama configuration");
        var httpClient = new HttpClient
        {
            BaseAddress = ollamaConfig.Uri,
            Timeout = ollamaConfig.TimeoutInMinutes
        };
        
        builder.Services.AddSingleton(builder.Configuration.GetSection("VideoCapture").Get<VideoCaptureConfig>()!);
        
        builder.Services.AddKeyedSingleton<IVideoCapturer, VideoCapturer>("videoCapturer");
        builder.Services.AddKeyedSingleton<IGenericAiClient, GenericAiClient>("genericAiClient");
        builder.Services.AddSingleton<IChatClient>(new OllamaApiClient(httpClient, "gemma3:4b"));

        return builder;
    }
}