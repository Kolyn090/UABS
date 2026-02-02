namespace UABS.Wrapper
{
    public interface IPngWriter
    {
        void SaveRgbaAsPng(
            byte[] rgba,
            int width,
            int height,
            string outputPath,
            bool flipVertically=true);
    }
}