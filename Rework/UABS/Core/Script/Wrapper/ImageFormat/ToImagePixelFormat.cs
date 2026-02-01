namespace UABS.Wrapper
{
    public static class ToImagePixelFormat
    {
        // TODO: Add more data
        public static ImagePixelFormat Convert(int unityFormat)
        {
            return unityFormat switch
            {
                0 => ImagePixelFormat.RGBA32,
                1 => ImagePixelFormat.BGRA32,
                10 => ImagePixelFormat.Grayscale8,
                28 => ImagePixelFormat.Grayscale8,
                _ => ImagePixelFormat.Unknown
            };
        }
    }
}