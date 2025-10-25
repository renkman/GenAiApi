using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Moq;
using OpenLlamaAPI.Ai;
using OpenLlamaAPI.Contracts;

namespace OpenLlamaApiTests.Ai;

public class GenericAiClientTests
{
    private readonly Mock<IChatClient> _chatClient = new(MockBehavior.Strict);
    private readonly Mock<ILogger<GenericAiClient>> _logger = new();

    
    [Fact]
    public async Task ChatAsync_WithValidPrompt_SendsPromptToAi()
    {
        const string message = "Moin!";
        const string chatBotMessage = "Moin moin!";
        
        var prompt = new Prompt { Message = message, Role = "user" };
        _chatClient.Setup(c =>
                c.GetResponseAsync(It.Is<IEnumerable<ChatMessage>>(m => m.Single().Text == prompt.Message), null,
                    CancellationToken.None))
            .ReturnsAsync(new ChatResponse(new ChatMessage(ChatRole.User, chatBotMessage)));
        
        var genericAiClient = new GenericAiClient(_chatClient.Object, _logger.Object);

        var response = await genericAiClient.ChatAsync(prompt, CancellationToken.None);
        
        Assert.Equal(response, response);
        _chatClient.Verify(c =>
            c.GetResponseAsync(It.Is<IEnumerable<ChatMessage>>(m => m.Single().Text == prompt.Message), null,
                CancellationToken.None), Times.Once());

    }

    [Fact]
    public async Task AnalyzeImage_WithImage_SendsImageToAi()
    {
        var image = new byte[] { 5, 0, 0 };
        ChatMessage? createdMessage = null;

        _chatClient.Setup(c =>
                c.GetResponseAsync(It.IsAny<IEnumerable<ChatMessage>>(), null, CancellationToken.None))
            .Returns<IEnumerable<ChatMessage>, ChatOptions?, CancellationToken>((messages, _, _) =>
            {
                createdMessage = messages.Single();
                return Task.FromResult(new ChatResponse(createdMessage));
            });

        var genericApiClient = new GenericAiClient(_chatClient.Object, _logger.Object);
        
        var response = await genericApiClient.AnalyzeImage(image, CancellationToken.None);

        Assert.NotNull(createdMessage);
        Assert.Equal(createdMessage.Text, response);
        Assert.Equal(createdMessage.Role, ChatRole.User);

        var contents = createdMessage.Contents.OrderBy(c => c.GetType().FullName);
        Assert.Collection(contents, c =>
            {
                var imageContent = Assert.IsType<DataContent>(c);
                Assert.Equal(image, imageContent.Data);
            },
            c =>
            {
                var textContent = Assert.IsType<TextContent>(c);
                Assert.Equal(Prompts.WeatherPrompt, textContent.Text);
            });
    }
}