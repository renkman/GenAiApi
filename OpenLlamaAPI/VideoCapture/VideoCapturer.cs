namespace OpenLlamaAPI.VideoCapture;

using OpenCvSharp;

public class VideoCapturer : IVideoCapturer
{
    private ILogger<VideoCapturer> _logger;

    public VideoCapturer(ILogger<VideoCapturer> logger)
    {
        _logger = logger;
    }

    public bool TryCapture(string url, out byte[] image)
    {
        image = [];
        using var capture = new VideoCapture(url);
        using var frame = new Mat();

        if (!capture.Read(frame))
        {
            _logger.LogError("Failed to read video frame from {Url}", url);
            return false;
        }
        
        image = frame.ImEncode(".jpg");
        return true;
    }
}