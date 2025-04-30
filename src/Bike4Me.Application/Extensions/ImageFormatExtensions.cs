namespace Bike4Me.Application.Extensions;

public static class ImageFormatExtensions
{
    public static bool IsPng(this byte[] bytes)
    {
        if (bytes is null || bytes.Length < 8)
        {
            return false;
        }

        return bytes[0] == 0x89 &&
               bytes[1] == 0x50 &&
               bytes[2] == 0x4E &&
               bytes[3] == 0x47 &&
               bytes[4] == 0x0D &&
               bytes[5] == 0x0A &&
               bytes[6] == 0x1A &&
               bytes[7] == 0x0A;
    }

    public static bool IsBmp(this byte[] bytes)
    {
        if (bytes is null || bytes.Length < 2) return false;

        return bytes[0] == 0x42 && bytes[1] == 0x4D;
    }

    public static bool IsValidCnhImageFormat(this byte[] bytes)
    {
        return bytes.IsPng() || bytes.IsBmp();
    }
}