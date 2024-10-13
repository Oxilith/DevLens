﻿using Domain.Enums;

namespace Common.Extensions;

public static class FileTypeExtensions
{
    public static string ToFileExtension(this FileType fileType)
    {
        return fileType switch
        {
            FileType.Json => ".json",
            FileType.Class => ".cs",
            FileType.Razor => ".razor",
            FileType.Ts => ".ts",
            FileType.Tsx => ".tsx",
            FileType.Js => ".js",
            _ => throw new ArgumentOutOfRangeException(nameof(fileType), fileType, null)
        };
    }
}