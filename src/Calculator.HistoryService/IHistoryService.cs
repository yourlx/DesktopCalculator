using System.Collections.ObjectModel;
using Calculator.HistoryService.Models;

namespace Calculator.HistoryService;

public interface IHistoryService
{
    public ObservableCollection<HistoryEntry> HistoryEntries { get; }
    
    public void SaveEntryToHistory(HistoryEntry historyService);

    public void ClearHistory();
}