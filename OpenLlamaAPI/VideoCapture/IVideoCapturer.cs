namespace OpenLlamaAPI.VideoCapture;

public interface IVideoCapturer
{
    bool TryCapture(string url, out byte[] image);
}