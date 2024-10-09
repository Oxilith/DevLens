using Domain.Enums;

namespace Domain.Extensions;

public static class FileTypeExtensions
{
    public static string ToFileExtension(this FileType fileType)
    {
        return fileType switch
        {
            FileType.JSON => ".json",
            FileType.Class => ".cs",
            FileType.TS => ".ts",
            FileType.TSX => ".tsx",
            _ => throw new ArgumentOutOfRangeException(nameof(fileType), fileType, null)
        };
    }
}