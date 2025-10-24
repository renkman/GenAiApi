using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OpenLlamaAPI.Ai;
using OpenLlamaAPI.Contracts;
using OpenLlamaAPI.VideoCapture;

namespace OpenLlamaAPI;

public static class WebApplicationExtensions
{
    public static WebApplication MapRoutes(this WebApplication app)
    {
        app.MapPost("/chat",
                async Task<Results<Ok<string>, BadRequest>> (HttpContext httpContext,
                    [FromBody] ChatInput chatInput,
                    [FromKeyedServices("genericAiClient")] IGenericAiClient genericAiClient, CancellationToken ct) =>
                {
                    var result = await genericAiClient.ChatAsync(chatInput, ct);

                    return TypedResults.Ok(result);
                })
            .WithName("PostChat")
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Chat with the Ollama model.")
            .WithDescription("Send a message to the Ollama server with the selected chat role and get the response.")
            .WithOpenApi();

        var videoUrl = app.Configuration["LiveCamPageUri"] ?? "";
        app.MapGet("/image",
                async Task<Ok<string>> (HttpContext httpContext,
                    [FromKeyedServices("videoCapturer")] IVideoCapturer videoCapturer,
                    [FromKeyedServices("genericAiClient")] IGenericAiClient genericAiClient, CancellationToken ct) =>
                {
                    if (!videoCapturer.TryCapture(videoUrl, out var image))
                        Results.Problem($"No image found under {videoUrl}", statusCode: 500);
                    
                    var result = await genericAiClient.AnalyzeImage(image, ct);

                    return TypedResults.Ok(result.ToString());
                })
            .WithName("GetImage")
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Upload image.")
            .WithDescription(
                "Send an image to the Ollama server and get a description of the image as response.")
            .WithOpenApi();
        
        return app;
    }
}