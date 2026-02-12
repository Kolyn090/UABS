namespace UABS.Wrapper
{
    public interface IImageResource
    {
        int Width { get; }
        int Height { get; }
        UnityTextureFormat UnityTextureFormat { get; }
        byte[] RawImageBytes { get; }  // This is only for display, always RGBA32
    }
}