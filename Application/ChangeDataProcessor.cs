using System.Collections.ObjectModel;
using Domain.Entities;
using Domain.Enums;
using Domain.Extensions;
using Domain.Models;
using Domain.ValueObjects;

namespace Application;

public class ChangeDataProcessor
{
    private const int NumberOfMonthsForAverageCalculation = 6;
    private const int MinimumMonthlyChanges = 5;
    private readonly IReadOnlyCollection<ChangeDataModel> _changes;

    public ChangeDataProcessor(IEnumerable<ProjectChange> changes, FileType fileType)
    {
        ArgumentNullException.ThrowIfNull(changes, nameof(changes));
        ArgumentNullException.ThrowIfNull(fileType, nameof(fileType));

        var classes = changes.SelectMany(c => c.ClassChanges)
            .Where(x => x.ClassName.EndsWith(fileType.ToFileExtension(), StringComparison.OrdinalIgnoreCase))
            .GroupBy(x => x.ClassName);

        _changes = new ReadOnlyCollection<ChangeDataModel>(classes.Select(CreateChangeDataModel).ToList());
    }

    private ChangeDataModel CreateChangeDataModel(IGrouping<string, ClassChange> group)
    {
        var orderedMonthlyChanges = group.OrderBy(x => x.ChangeDate)
            .GroupBy(x => YearMonth.New(x.ChangeDate.Year, x.ChangeDate.Month))
            .Select(dateGroup => MonthlyChange.New(dateGroup.Key, dateGroup.Count()))
            .OrderBy(x => x.Month)
            .ToList();

        var completeMonthlyChanges = FillMissingMonths(orderedMonthlyChanges);
        var changeDataModel = new ChangeDataModel(group.Key);
        changeDataModel.SetMonthlyChanges(completeMonthlyChanges);

        return changeDataModel;
    }

    private IReadOnlyCollection<MonthlyChange> FillMissingMonths(List<MonthlyChange> orderedMonthlyChanges)
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

    private IReadOnlyCollection<ChangeDataModel> LimitAndOrderData(int limit)
    {
        return ApplyLimit(GetOrderedByAverageChangesFromLastMonths(), limit);
    }

    private IReadOnlyCollection<ChangeDataModel> GetOrderedByAverageChangesFromLastMonths()
    {
        return new ReadOnlyCollection<ChangeDataModel>(_changes
            .Where(change => change.MonthlyChanges.Count() > MinimumMonthlyChanges)
            .OrderByDescending(GetAverageChangesFromLastMonths).ToList());
    }

    private double GetAverageChangesFromLastMonths(ChangeDataModel change)
    {
        if (!change.MonthlyChanges.Any()) return 0;

        return change.MonthlyChanges.OrderByDescending(m => m.Month)
            .Take(NumberOfMonthsForAverageCalculation)
            .Average(m => m.ChangeCount);
    }

    private IReadOnlyCollection<ChangeDataModel> ApplyLimit(IReadOnlyCollection<ChangeDataModel> orderedChanges,
        int limit)
    {
        if (limit == 0 || limit >= orderedChanges.Count) return orderedChanges.ToList();

        return orderedChanges.Take(limit).ToList();
    }

    public IReadOnlyCollection<ChangeDataModel> GetData()
    {
        return GetOrderedByAverageChangesFromLastMonths();
    }

    public IReadOnlyCollection<ChangeDataModel> GetData(int limit)
    {
        return LimitAndOrderData(limit);
    }
}