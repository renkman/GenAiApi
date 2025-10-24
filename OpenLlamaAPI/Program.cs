using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using OllamaSharp;
using OpenLlamaAPI.Ai;
using OpenLlamaAPI.Contracts;
using OpenLlamaAPI.VideoCapture;

namespace OpenLlamaAPI;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var ollamaUri = builder.Configuration["OllamaUri"] ?? "http://localhost:11434";
        
        builder.Services.AddKeyedSingleton<IVideoCapturer, VideoCapturer>("videoCapturer");
        builder.Services.AddKeyedSingleton<IGenericAiClient, GenericAiClient>("genericAiClient");
        builder.Services.AddSingleton<IChatClient>(new OllamaApiClient(new Uri(ollamaUri), "gemma3:4b"));
        
        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        var videoUrl = app.Configuration["LiveCamUri"] ?? "";

        app.MapPost("/chat",
                async Task<Results<Ok<string>, BadRequest>> (HttpContext httpContext,
                    [FromBody] ChatInput chatInput,
                    [FromKeyedServices("genericAiClient")] IGenericAiClient genericAiClient) =>
                {
                    var result = await genericAiClient.ChatAsync(chatInput);

                    return TypedResults.Ok(result);
                })
            .WithName("PostChat")
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Chat with the Ollama model.")
            .WithDescription("Send a message to the Ollama server with the selected chat role and get the response.")
            .WithOpenApi();

        app.MapGet("/image",
            async Task<Ok> (HttpContext httpContext, [FromKeyedServices("videoCapturer")] IVideoCapturer videoCapturer, [FromKeyedServices("genericAiClient")] IGenericAiClient genericAiClient) =>
                {
                    if (!videoCapturer.TryCapture(videoUrl, out var image))
                        Results.Problem($"No image found under {videoUrl}", statusCode: 500);

                    var result = await genericAiClient.AnalyzeImage(image.ToBase64());
                    
                    return TypedResults.Ok(); 
                })
            .WithName("GetImage")
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Upload image.")
                .WithDescription(
                    "Send an image to the Ollama server and get a description of the image as response.")
                .WithOpenApi();
        
        app.Run();
    }
}