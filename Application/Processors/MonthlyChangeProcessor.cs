using Application.Services;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Application.Processors;

public static class MonthlyChangeProcessor
{
    public static List<MonthlyChange> GetOrderedMonthlyChanges(ILogger<ChangeDataService> logger,
        IGrouping<string, ClassChange> group)
    {
        logger.LogInformation("Getting ordered monthly changes for group: {GroupName}", group.Key);
        var orderedMonthlyChanges = group.GroupBy(x => YearMonth.New(x.ChangeDate.Year, x.ChangeDate.Month))
            .Select(g => MonthlyChange.New(g.Key, g.Count()))
            .OrderBy(g => g.Month)
            .ToList();
        logger.LogInformation("Number of ordered monthly changes: {Count}", orderedMonthlyChanges.Count);
        return orderedMonthlyChanges;
    }

    public static IReadOnlyCollection<MonthlyChange> FillMissingMonthsOptimized(ILogger<ChangeDataService> logger,
        List<MonthlyChange> orderedMonthlyChanges)
    {
        logger.LogInformation("Filling missing months in ordered monthly changes...");
        if (!orderedMonthlyChanges.Any())
        {
            logger.LogWarning("No monthly changes to process.");
            return orderedMonthlyChanges.AsReadOnly();
        }

        var completeMonthlyChanges = new LinkedList<MonthlyChange>();
        var currentMonth = orderedMonthlyChanges.First().Month;
        var lastMonth = orderedMonthlyChanges.Last().Month;

        var index = 0;
        while (currentMonth.CompareTo(lastMonth) <= 0)
        {
            if (index < orderedMonthlyChanges.Count && orderedMonthlyChanges[index].Month.Equals(currentMonth))
            {
                completeMonthlyChanges.AddLast(orderedMonthlyChanges[index]);
                index++;
            }
            else
            {
                logger.LogInformation("Adding default monthly change for month: {Month}", currentMonth);
                completeMonthlyChanges.AddLast(MonthlyChange.Default(currentMonth));
            }

            currentMonth = currentMonth.NextMonth();
        }

        logger.LogInformation("Number of complete monthly changes: {Count}", completeMonthlyChanges.Count);
        return completeMonthlyChanges.ToList().AsReadOnly();
    }
}