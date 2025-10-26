using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenLlamaAPI;
using OpenLlamaAPI.VideoCapture;

namespace OpenLlamaApiTests.VideoCapture;

public class VideoCapturerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    
    public VideoCapturerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public void TryCapture_WithValidVideoStream_CapturesImageFromVideoAndReturnsTrue()
    {
        var config = _factory.Services.GetRequiredService<VideoCaptureConfig>();
        var logger = _factory.Services.GetRequiredService<ILogger<VideoCapturer>>();
        
        var videoCapturer = new VideoCapturer(config, logger);
        var result = videoCapturer.TryCapture(out var resultImage);

        Assert.True(result);
        Assert.NotEmpty(resultImage);
    }
    
        
    [Fact]
    public void TryCapture_WithValidVideoStream_DoesNotCaptureAnImageFromVideoAndReturnsFalse()
    {
        var config = new VideoCaptureConfig { LiveCamPageUri = "Nothing_here!" };
        var logger = _factory.Services.GetRequiredService<ILogger<VideoCapturer>>();
        
        var videoCapturer = new VideoCapturer(config, logger);
        var result = videoCapturer.TryCapture(out var resultImage);

        Assert.False(result);
        Assert.Empty(resultImage);
    }
}