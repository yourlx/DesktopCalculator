using System.Collections.ObjectModel;
using Calculator.HistoryService.Models;

namespace Calculator.HistoryService;

public interface IHistoryService
{
    ObservableCollection<HistoryEntry> HistoryEntries { get; }

    void SaveToHistory(HistoryEntry historyService);

    void ClearHistory();
}