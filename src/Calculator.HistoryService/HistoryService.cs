using System.Collections.ObjectModel;
using System.Text.Json;
using Calculator.HistoryService.Models;

namespace Calculator.HistoryService;

public class HistoryService : IHistoryService
{
    public ObservableCollection<HistoryEntry> HistoryEntries { get; set; } = new();

    private readonly string _fileName = "history.json";

    public HistoryService()
    {
        if (!File.Exists(_fileName))
        {
            File.Create(_fileName);
        }
        else
        {
            LoadEntriesFromHistory();
        }
    }

    private void LoadEntriesFromHistory()
    {
        var json = File.ReadAllText(_fileName);
        if (!string.IsNullOrWhiteSpace(json))
        {
            HistoryEntries = JsonSerializer.Deserialize<ObservableCollection<HistoryEntry>>(json)!;
        }
    }

    public void SaveEntryToHistory(HistoryEntry historyEntry)
    {
        HistoryEntries.Insert(0, historyEntry);
        var json = JsonSerializer.Serialize(HistoryEntries);
        File.WriteAllText(_fileName, json);
    }

    public void ClearHistory()
    {
        HistoryEntries.Clear();
        File.WriteAllText(_fileName, string.Empty);
    }
}