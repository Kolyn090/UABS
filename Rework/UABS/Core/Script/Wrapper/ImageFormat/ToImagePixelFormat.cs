namespace UABS.Wrapper
{
    public static class ToImagePixelFormat
    {
        // TODO: Add more data
        public static ImagePixelFormat Convert(int unityFormat)
        {
            return unityFormat switch
            {
                4 => ImagePixelFormat.RGBA32,
                14 => ImagePixelFormat.BGRA32,
                9 => ImagePixelFormat.Grayscale8,
                _ => ImagePixelFormat.Unknown
            };
        }
    }
}