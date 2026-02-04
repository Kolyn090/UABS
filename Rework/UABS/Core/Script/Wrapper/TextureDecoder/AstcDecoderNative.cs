using System.Runtime.InteropServices;

namespace UABS.Wrapper
{
    public class AstcDecoderNative
    {
        [DllImport("astc_decoder", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool DecodeASTC(
        byte[] astcData,
        int dataLength,
        int width,
        int height,
        byte[] outRgba,
        int blockX,
        int blockY);
    }
}