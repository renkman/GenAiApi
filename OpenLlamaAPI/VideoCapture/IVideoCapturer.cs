namespace OpenLlamaAPI.VideoCapture;

public interface IVideoCapturer
{
    bool TryCapture(out byte[] image);
}