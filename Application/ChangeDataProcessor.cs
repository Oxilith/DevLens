using System.Collections.ObjectModel;
using Domain;
using Domain.Entities;
using Domain.Enums;
using Domain.Extensions;
using Domain.Models;
using Domain.ValueObjects;

namespace Application;

public class ChangeDataProcessor
{
    private readonly List<ChangeDataModel> _changes;

    public ChangeDataProcessor(IEnumerable<ProjectChange> changes, FileType fileType)
    {
        var fileExtension = fileType.ToFileExtension();

        var classes = changes.SelectMany(c => c.ClassChanges)
            .Where(x => x.ClassName.EndsWith(fileExtension))
            .GroupBy(x => x.ClassName);

        _changes = classes.Select(CreateChangeDataModel).ToList();
    }

    private ChangeDataModel CreateChangeDataModel(IGrouping<string, ClassChange> group)
    {
        var orderedMonthlyChanges = group.OrderBy(x => x.ChangeDate)
            .GroupBy(x => YearMonth.New(x.ChangeDate.Year, x.ChangeDate.Month))
            .Select(dateGroup => MonthlyChange.New(dateGroup.Key, dateGroup.Count()))
            .OrderBy(x => x.Month)
            .ToList();

        var completeMonthlyChanges = FillMissingMonths(orderedMonthlyChanges).OrderBy(x => x.Month);

        return new ChangeDataModel(group.Key)
        {
            MonthlyChanges = completeMonthlyChanges
        };
    }

    private List<MonthlyChange> FillMissingMonths(List<MonthlyChange> orderedMonthlyChanges)
    {
        var completeMonthlyChanges = new List<MonthlyChange>();
        if (orderedMonthlyChanges.Count == 0) return completeMonthlyChanges;

        var currentMonth = orderedMonthlyChanges.First().Month;
        var lastMonth = orderedMonthlyChanges.Last().Month;

        while (currentMonth.CompareTo(lastMonth) <= 0)
        {
            var existingChange = orderedMonthlyChanges.FirstOrDefault(mc => mc.Month.Equals(currentMonth));
            completeMonthlyChanges.Add(existingChange ?? MonthlyChange.Default(currentMonth));
            currentMonth = currentMonth.NextMonth();
        }

        return completeMonthlyChanges;
    }

    private List<ChangeDataModel> LimitAndOrderData(int limit)
    {
        return ApplyLimit(GetOrderedByAverageChangesFromLastSixMonths(), limit);
    }

    private IEnumerable<ChangeDataModel> GetOrderedByAverageChangesFromLastSixMonths()
    {
        return _changes.Where(change => change.MonthlyChanges.Count() > 5)
            .OrderBy(GetAverageChangesFromLastSixMonths);
    }

    private double GetAverageChangesFromLastSixMonths(ChangeDataModel change)
    {
        return change.MonthlyChanges.OrderByDescending(m => m.Month)
            .Take(6)
            .Average(m => m.ChangeCount);
    }

    private List<ChangeDataModel> ApplyLimit(IEnumerable<ChangeDataModel> orderedChanges, int limit)
    {
        return orderedChanges
            .Reverse()
            .Take(limit == 0 ? _changes.Count : limit)
            .ToList();
    }

    public ReadOnlyCollection<ChangeDataModel> GetData()
    {
        return GetOrderedByAverageChangesFromLastSixMonths().Reverse().ToList().AsReadOnly();
    }

    public ReadOnlyCollection<ChangeDataModel> GetData(int limit)
    {
        return LimitAndOrderData(limit).ToList().AsReadOnly();
    }
}