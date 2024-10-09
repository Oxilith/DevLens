using Domain.ValueObjects;

namespace Domain.Models;

public class ChangeDataModel
{
    public ChangeDataModel(string fileName)
    {
        FileName = fileName;
        MonthlyChanges = new List<MonthlyChange>().OrderBy(x => x.Month);
    }

    public string FileName { get; private set; }
    public IOrderedEnumerable<MonthlyChange> MonthlyChanges { get; private set; }

    public void SetMonthlyChanges(IEnumerable<MonthlyChange> monthlyChanges)
    {
        MonthlyChanges = monthlyChanges.OrderBy(x => x.Month);
    }
}