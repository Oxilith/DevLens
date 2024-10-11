using System.Collections.ObjectModel;
using Application.Extensions;
using Application.Factories;
using Domain.Entities;
using Domain.Enums;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class ChangeDataService
{
    private readonly ILogger<ChangeDataService> _logger;
    private readonly int _numberOfMonthsForAverageCalculation;
    private readonly int _minimumMonthlyChanges;
    private readonly IReadOnlyCollection<ChangeDataModel> _changes;

    public ChangeDataService(ILogger<ChangeDataService> logger, IConfiguration configuration,
        IEnumerable<ProjectChange> changes, FileType fileType)
    {
        _logger = logger;
        _logger.LogInformation("Initializing ChangeDataService...");
        ArgumentNullException.ThrowIfNull(changes, nameof(changes));
        ArgumentNullException.ThrowIfNull(fileType, nameof(fileType));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        _numberOfMonthsForAverageCalculation =
            configuration.TryGetValue("ChangeDataService:NumberOfMonthsForAverageCalculation", 6);
        _minimumMonthlyChanges = configuration.TryGetValue("ChangeDataService:MinimumMonthlyChanges", 5);

        _changes = ChangeDataModelFactory.CreateChangeDataModelsFromChanges(logger, changes, fileType).ToList()
            .AsReadOnly();
        _logger.LogInformation("ChangeDataService initialized successfully.");
    }

    private IReadOnlyCollection<ChangeDataModel> GetOrderedByAverageChangesFromLastMonths()
    {
        _logger.LogInformation("Getting ordered changes by average changes from last months...");
        var orderedChanges = new ReadOnlyCollection<ChangeDataModel>(_changes
            .Where(change => change.MonthlyChangesOrdered.Count() > _minimumMonthlyChanges)
            .OrderByDescending(GetAverageChangesFromLastMonths).ToList());
        _logger.LogInformation("Number of ordered changes: {Count}", orderedChanges.Count);
        return orderedChanges;
    }

    private double GetAverageChangesFromLastMonths(ChangeDataModel change)
    {
        if (!change.MonthlyChangesOrdered.Any())
        {
            _logger.LogWarning("No monthly changes available for {FileName}.", change.FileName);
            return 0;
        }

        var average = change.MonthlyChangesOrdered
            .Take(_numberOfMonthsForAverageCalculation)
            .Average(m => m.ChangeCount);
        _logger.LogInformation("Average changes for {FileName}: {Average}", change.FileName, average);
        return average;
    }

    private IReadOnlyCollection<ChangeDataModel> ApplyLimit(IReadOnlyCollection<ChangeDataModel> orderedChanges,
        int limit)
    {
        _logger.LogInformation("Applying limit: {Limit}", limit);
        if (limit < 0) throw new ArgumentOutOfRangeException(nameof(limit), "Limit must be non-negative.");
        var limitedChanges = limit >= orderedChanges.Count ? orderedChanges : orderedChanges.Take(limit).ToArray();
        _logger.LogInformation("Number of changes after applying limit: {Count}", limitedChanges.Count());
        return limitedChanges;
    }

    public IReadOnlyCollection<ChangeDataModel> GetData()
    {
        _logger.LogInformation("Retrieving data...");
        return GetOrderedByAverageChangesFromLastMonths();
    }

    public IReadOnlyCollection<ChangeDataModel> GetData(int limit)
    {
        _logger.LogInformation("Retrieving data with limit: {Limit}", limit);
        return ApplyLimit(GetOrderedByAverageChangesFromLastMonths(), limit);
    }
}