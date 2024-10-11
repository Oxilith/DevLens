using Application.Filters;
using Application.Processors;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Application.Factories;

public static class ChangeDataModelFactory
{
    public static IEnumerable<ChangeDataModel> CreateChangeDataModelsFromChanges(ILogger<ChangeDataService> logger,
        IEnumerable<ProjectChange> changes,
        FileType fileType)
    {
        logger.LogInformation("Creating ChangeDataModels from changes...");
        var classChanges = ChangeDataFilter.GetClassChanges(logger, changes, fileType);
        logger.LogInformation("Number of class changes: {Count}", classChanges.Count());
        var groupedClasses = ChangeDataFilter.GroupByClassName(logger, classChanges);
        logger.LogInformation("Number of grouped classes: {Count}", groupedClasses.Count());
        return CreateChangeDataModels(logger, groupedClasses);
    }

    private static IEnumerable<ChangeDataModel> CreateChangeDataModels(ILogger<ChangeDataService> logger,
        IEnumerable<IGrouping<string, ClassChange>> groupedClasses)
    {
        foreach (var group in groupedClasses)
        {
            logger.LogInformation("Creating ChangeDataModel for class: {ClassName}", group.Key);
            yield return CreateChangeDataModel(logger, group);
        }
    }

    private static ChangeDataModel CreateChangeDataModel(ILogger<ChangeDataService> logger,
        IGrouping<string, ClassChange> group)
    {
        logger.LogInformation("Creating ChangeDataModel for group: {GroupName}", group.Key);
        var orderedMonthlyChanges = MonthlyChangeProcessor.GetOrderedMonthlyChanges(logger, group);
        var completeMonthlyChanges = MonthlyChangeProcessor.FillMissingMonthsOptimized(logger, orderedMonthlyChanges);
        var changeDataModel = new ChangeDataModel(group.Key);
        changeDataModel.SetMonthlyChanges(completeMonthlyChanges);
        logger.LogInformation("ChangeDataModel created for group: {GroupName}", group.Key);
        return changeDataModel;
    }
}