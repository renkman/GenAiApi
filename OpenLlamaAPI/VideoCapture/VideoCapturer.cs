namespace OpenLlamaAPI.VideoCapture;

using OpenCvSharp;

public class VideoCapturer : IVideoCapturer
{
    private readonly VideoCaptureConfig _videoCaptureConfig;
    private readonly ILogger<VideoCapturer> _logger;

    public VideoCapturer(VideoCaptureConfig videoCaptureConfig, ILogger<VideoCapturer> logger)
    {
        _videoCaptureConfig = videoCaptureConfig;
        _logger = logger;
    }

    public bool TryCapture(out byte[] image)
    {
        image = [];
        using var capture = new VideoCapture(_videoCaptureConfig.LiveCamPageUri);
        using var frame = new Mat();

        if (!capture.Read(frame))
        {
            _logger.LogError("Failed to read video frame from {Url}", _videoCaptureConfig.LiveCamPageUri);
            return false;
        }
        
        image = frame.ImEncode(".jpg");
        return true;
    }
}