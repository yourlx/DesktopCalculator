using System.Collections.ObjectModel;
using System.Text.Json;
using Calculator.HistoryService.Models;

namespace Calculator.HistoryService;

public class HistoryService : IHistoryService
{
    public ObservableCollection<HistoryEntry> HistoryEntries { get; private set; } = new();

    private const string FileName = "history.json";

    private const int NumberOfEntriesLimit = 100;

    public HistoryService()
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

    public void SaveEntryToHistory(HistoryEntry historyEntry)
    {
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