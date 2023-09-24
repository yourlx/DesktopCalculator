using System.Collections.ObjectModel;
using System.Text.Json;
using Calculator.HistoryService.Models;

namespace Calculator.HistoryService;

public class JsonHistoryService : IHistoryService
{
    public ObservableCollection<HistoryEntry> HistoryEntries { get; private set; } = new();

    private const string FileName = "history.json";

    private const int NumberOfEntriesLimit = 100;

    public JsonHistoryService()
    {
        if (!File.Exists(FileName))
        {
            File.Create(FileName);
        }
        else
        {
            LoadEntriesFromHistory();
        }
    }

    private void LoadEntriesFromHistory()
    {
        var json = File.ReadAllText(FileName);
        if (!string.IsNullOrWhiteSpace(json))
        {
            HistoryEntries = JsonSerializer.Deserialize<ObservableCollection<HistoryEntry>>(json)!;
        }
    }

    public void SaveToHistory(HistoryEntry historyEntry)
    {
        if (HistoryEntries.Count > 0)
        {
            var firstEntry = HistoryEntries.First();
            if (firstEntry.Expression == historyEntry.Expression &&
                firstEntry.Variable == historyEntry.Variable) return;
        }

        if (HistoryEntries.Count >= NumberOfEntriesLimit)
        {
            HistoryEntries.Remove(HistoryEntries.Last());
        }

        HistoryEntries.Insert(0, historyEntry);
        var json = JsonSerializer.Serialize(HistoryEntries);
        File.WriteAllText(FileName, json);
    }

    public void ClearHistory()
    {
        HistoryEntries.Clear();
        File.WriteAllText(FileName, string.Empty);
    }
}