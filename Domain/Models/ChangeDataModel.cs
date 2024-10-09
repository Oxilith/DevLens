namespace Domain.Models;

public class ChangeDataModel
{
    public ChangeDataModel(string fileName)
    {
        FileName = fileName;
        MonthlyChanges = new List<MonthlyChange>().OrderByDescending(x => x.Month);
    }

    public string FileName { get; }
    public IOrderedEnumerable<MonthlyChange> MonthlyChanges { get; set; }
}