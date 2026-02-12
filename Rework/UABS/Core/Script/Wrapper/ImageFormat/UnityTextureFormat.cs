namespace UABS.Wrapper
{
    // * Directly from Unity 2021 TextureFormat, anything that
    // is decrypted or obsolete are not included here
    public enum UnityTextureFormat
    {
        //
        // Summary:
        //     Alpha-only texture format, 8 bit integer.
        Alpha8 = 1,
        //
        // Summary:
        //     A 16 bits/pixel texture format. Texture stores color with an alpha channel.
        ARGB4444 = 2,
        //
        // Summary:
        //     Color texture format, 8-bits per channel.
        RGB24 = 3,
        //
        // Summary:
        //     Color with alpha texture format, 8-bits per channel.
        RGBA32 = 4,
        //
        // Summary:
        //     Color with alpha texture format, 8-bits per channel.
        ARGB32 = 5,
        //
        // Summary:
        //     A 16 bit color texture format.
        RGB565 = 7,
        //
        // Summary:
        //     Single channel (R) texture format, 16 bit integer.
        R16 = 9,
        //
        // Summary:
        //     Compressed color texture format.
        DXT1 = 10,
        //
        // Summary:
        //     Compressed color with alpha channel texture format.
        DXT5 = 12,
        //
        // Summary:
        //     Color and alpha texture format, 4 bit per channel.
        RGBA4444 = 13,
        //
        // Summary:
        //     Color with alpha texture format, 8-bits per channel.
        BGRA32 = 14,
        //
        // Summary:
        //     Scalar (R) texture format, 16 bit floating point.
        RHalf = 15,
        //
        // Summary:
        //     Two color (RG) texture format, 16 bit floating point per channel.
        RGHalf = 16,
        //
        // Summary:
        //     RGB color and alpha texture format, 16 bit floating point per channel.
        RGBAHalf = 17,
        //
        // Summary:
        //     Scalar (R) texture format, 32 bit floating point.
        RFloat = 18,
        //
        // Summary:
        //     Two color (RG) texture format, 32 bit floating point per channel.
        RGFloat = 19,
        //
        // Summary:
        //     RGB color and alpha texture format, 32-bit floats per channel.
        RGBAFloat = 20,
        //
        // Summary:
        //     A format that uses the YUV color space and is often used for video encoding or
        //     playback.
        YUY2 = 21,
        //
        // Summary:
        //     RGB HDR format, with 9 bit mantissa per channel and a 5 bit shared exponent.
        RGB9e5Float = 22,
        //
        // Summary:
        //     Compressed one channel (R) texture format.
        BC4 = 26,
        //
        // Summary:
        //     Compressed two-channel (RG) texture format.
        BC5 = 27,
        //
        // Summary:
        //     HDR compressed color texture format.
        BC6H = 24,
        //
        // Summary:
        //     High quality compressed color texture format.
        BC7 = 25,
        //
        // Summary:
        //     Compressed color texture format with Crunch compression for smaller storage sizes.
        DXT1Crunched = 28,
        //
        // Summary:
        //     Compressed color with alpha channel texture format with Crunch compression for
        //     smaller storage sizes.
        DXT5Crunched = 29,
        //
        // Summary:
        //     PowerVR (iOS) 2 bits/pixel compressed color texture format.
        PVRTC_RGB2 = 30,
        //
        // Summary:
        //     PowerVR (iOS) 2 bits/pixel compressed with alpha channel texture format.
        PVRTC_RGBA2 = 31,
        //
        // Summary:
        //     PowerVR (iOS) 4 bits/pixel compressed color texture format.
        PVRTC_RGB4 = 32,
        //
        // Summary:
        //     PowerVR (iOS) 4 bits/pixel compressed with alpha channel texture format.
        PVRTC_RGBA4 = 33,
        //
        // Summary:
        //     ETC (GLES2.0) 4 bits/pixel compressed RGB texture format.
        ETC_RGB4 = 34,
        //
        // Summary:
        //     ETC2 EAC (GL ES 3.0) 4 bitspixel compressed unsigned single-channel texture format.
        EAC_R = 41,
        //
        // Summary:
        //     ETC2 EAC (GL ES 3.0) 4 bitspixel compressed signed single-channel texture format.
        EAC_R_SIGNED = 42,
        //
        // Summary:
        //     ETC2 EAC (GL ES 3.0) 8 bitspixel compressed unsigned dual-channel (RG) texture
        //     format.
        EAC_RG = 43,
        //
        // Summary:
        //     ETC2 EAC (GL ES 3.0) 8 bitspixel compressed signed dual-channel (RG) texture
        //     format.
        EAC_RG_SIGNED = 44,
        //
        // Summary:
        //     ETC2 (GL ES 3.0) 4 bits/pixel compressed RGB texture format.
        ETC2_RGB = 45,
        //
        // Summary:
        //     ETC2 (GL ES 3.0) 4 bits/pixel RGB+1-bit alpha texture format.
        ETC2_RGBA1 = 46,
        //
        // Summary:
        //     ETC2 (GL ES 3.0) 8 bits/pixel compressed RGBA texture format.
        ETC2_RGBA8 = 47,
        //
        // Summary:
        //     ASTC (4x4 pixel block in 128 bits) compressed RGB(A) texture format.
        ASTC_4x4 = 48,
        //
        // Summary:
        //     ASTC (5x5 pixel block in 128 bits) compressed RGB(A) texture format.
        ASTC_5x5 = 49,
        //
        // Summary:
        //     ASTC (6x6 pixel block in 128 bits) compressed RGB(A) texture format.
        ASTC_6x6 = 50,
        //
        // Summary:
        //     ASTC (8x8 pixel block in 128 bits) compressed RGB(A) texture format.
        ASTC_8x8 = 51,
        //
        // Summary:
        //     ASTC (10x10 pixel block in 128 bits) compressed RGB(A) texture format.
        ASTC_10x10 = 52,
        //
        // Summary:
        //     ASTC (12x12 pixel block in 128 bits) compressed RGB(A) texture format.
        ASTC_12x12 = 53,
        //
        // Summary:
        //     Two color (RG) texture format, 8-bits per channel.
        RG16 = 62,
        //
        // Summary:
        //     Single channel (R) texture format, 8 bit integer.
        R8 = 63,
        //
        // Summary:
        //     Compressed color texture format with Crunch compression for smaller storage sizes.
        ETC_RGB4Crunched = 64,
        //
        // Summary:
        //     Compressed color with alpha channel texture format using Crunch compression for
        //     smaller storage sizes.
        ETC2_RGBA8Crunched = 65,
        //
        // Summary:
        //     ASTC (4x4 pixel block in 128 bits) compressed RGB(A) HDR texture format.
        ASTC_HDR_4x4 = 66,
        //
        // Summary:
        //     ASTC (5x5 pixel block in 128 bits) compressed RGB(A) HDR texture format.
        ASTC_HDR_5x5 = 67,
        //
        // Summary:
        //     ASTC (6x6 pixel block in 128 bits) compressed RGB(A) HDR texture format.
        ASTC_HDR_6x6 = 68,
        //
        // Summary:
        //     ASTC (8x8 pixel block in 128 bits) compressed RGB(A) texture format.
        ASTC_HDR_8x8 = 69,
        //
        // Summary:
        //     ASTC (10x10 pixel block in 128 bits) compressed RGB(A) HDR texture format.
        ASTC_HDR_10x10 = 70,
        //
        // Summary:
        //     ASTC (12x12 pixel block in 128 bits) compressed RGB(A) HDR texture format.
        ASTC_HDR_12x12 = 71,
        //
        // Summary:
        //     Two channel (RG) texture format, 16 bit integer per channel.
        RG32 = 72,
        //
        // Summary:
        //     Three channel (RGB) texture format, 16 bit integer per channel.
        RGB48 = 73,
        //
        // Summary:
        //     Four channel (RGBA) texture format, 16 bit integer per channel.
        RGBA64 = 74,
    }
}