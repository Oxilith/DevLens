using Domain.Enums;

namespace Domain.Extensions;

public static class FileTypeExtensions
{
    public static string ToFileExtension(this FileType fileType)
    {
        return fileType switch
        {
            FileType.Json => ".json",
            FileType.Class => ".cs",
            FileType.Ts => ".ts",
            FileType.Tsx => ".tsx",
            _ => throw new ArgumentOutOfRangeException(nameof(fileType), fileType, null)
        };
    }
}