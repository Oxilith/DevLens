using Application.Services;
using Common.Extensions;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Application.Filters;

public static class ChangeDataFilter
{
    public static IEnumerable<ClassChange> GetClassChanges(ILogger<ChangeDataService> logger,
        IEnumerable<ProjectChange> changes, FileType fileType)
    {
        logger.LogInformation("Filtering class changes by file type: {FileType}", fileType);
        var allClassChanges = changes.SelectMany(c => c.ClassChanges);
        var filteredClassChanges = FilterClassChangesByFileType(logger, allClassChanges, fileType);
        var classChanges = filteredClassChanges.ToList();
        logger.LogInformation("Number of filtered class changes: {Count}", classChanges.Count);

        return classChanges;
    }

    public static IEnumerable<ClassChange> FilterClassChangesByFileType(ILogger<ChangeDataService> logger,
        IEnumerable<ClassChange> classChanges,
        FileType fileType)
    {
        logger.LogInformation("Filtering class changes for file type: {FileType}", fileType);
        return classChanges.Where(x =>
            x.ClassName.EndsWith(fileType.ToFileExtension(), StringComparison.OrdinalIgnoreCase));
    }

    public static IEnumerable<IGrouping<string, ClassChange>> GroupByClassName(ILogger<ChangeDataService> logger,
        IEnumerable<ClassChange> classChanges)
    {
        logger.LogInformation("Grouping class changes by class name...");
        var groupedClasses = classChanges.GroupBy(x => x.ClassName);
        logger.LogInformation("Number of groups formed: {Count}", groupedClasses.Count());
        return groupedClasses;
    }
}