using Domain.ValueObjects;

namespace Domain.Models;

public class ChangeDataModel
{
    public ChangeDataModel(string fileName)
    {
        FileName = fileName;
        MonthlyChangesOrdered = new List<MonthlyChange>().OrderBy(x => x.Month);
    }

    public string FileName { get; private set; }
    public IOrderedEnumerable<MonthlyChange> MonthlyChangesOrdered { get; private set; }

    public void SetMonthlyChanges(IEnumerable<MonthlyChange> monthlyChanges)
    {
        MonthlyChangesOrdered = monthlyChanges.OrderBy(x => x.Month);
    }
}