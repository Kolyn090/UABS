namespace UABS.Wrapper
{
    internal static class UnityTextureFormatExtensions
    {
        /// <summary>
        /// Default formats supported by the texture decoder. Return the corresponding compression format
        /// if available. Otherwise null. 
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static TextureCompressionFormat? SupportedCompression(this UnityTextureFormat format)
        {
            switch (format)
            {
                // Uncompressed 8-bit/channel formats
                case UnityTextureFormat.RGBA32:
                case UnityTextureFormat.ARGB32:
                case UnityTextureFormat.BGRA32:
                    return TextureCompressionFormat.Rgba;
                case UnityTextureFormat.RGB24:
                    return TextureCompressionFormat.Rgb;
                case UnityTextureFormat.R8:
                    return TextureCompressionFormat.R;
                case UnityTextureFormat.R16:
                    return TextureCompressionFormat.Rg;
                // BCn (DXT) compressed formats
                case UnityTextureFormat.DXT1:
                case UnityTextureFormat.BC4:
                    return TextureCompressionFormat.Bc1;
                case UnityTextureFormat.DXT5:
                    return TextureCompressionFormat.Bc3;
                case UnityTextureFormat.BC5:
                    return TextureCompressionFormat.Bc5;
                case UnityTextureFormat.BC6H:
                    return TextureCompressionFormat.Bc6U;
                case UnityTextureFormat.BC7:
                    return TextureCompressionFormat.Bc7;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Return true if the given texture format is single channel. Otherwise false.
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static bool IsSingleChannel(this UnityTextureFormat format)
        {
            return format is 
                UnityTextureFormat.Alpha8 or
                UnityTextureFormat.R8 or
                UnityTextureFormat.R16 or
                UnityTextureFormat.RHalf or
                UnityTextureFormat.RFloat or
                UnityTextureFormat.BC4 or
                UnityTextureFormat.EAC_R or
                UnityTextureFormat.EAC_R_SIGNED;
        }

        public static bool IsAndroid(this UnityTextureFormat format)
        {
            return format is
                UnityTextureFormat.ETC2_RGB or
                UnityTextureFormat.ETC2_RGBA1 or
                UnityTextureFormat.ETC2_RGBA8 or
                UnityTextureFormat.ETC2_RGBA8Crunched or
                UnityTextureFormat.ETC_RGB4 or
                UnityTextureFormat.ETC_RGB4Crunched or
                UnityTextureFormat.RGBA4444;
        }

        public static bool IsAstc(this UnityTextureFormat format)
        {
            return format is
                UnityTextureFormat.ASTC_4x4 or
                UnityTextureFormat.ASTC_5x5 or
                UnityTextureFormat.ASTC_6x6 or
                UnityTextureFormat.ASTC_8x8 or
                UnityTextureFormat.ASTC_10x10 or
                UnityTextureFormat.ASTC_12x12 or
                UnityTextureFormat.ASTC_HDR_4x4 or
                UnityTextureFormat.ASTC_HDR_5x5 or
                UnityTextureFormat.ASTC_HDR_6x6 or
                UnityTextureFormat.ASTC_HDR_8x8 or
                UnityTextureFormat.ASTC_HDR_10x10 or
                UnityTextureFormat.ASTC_HDR_12x12;
        }
    }
}