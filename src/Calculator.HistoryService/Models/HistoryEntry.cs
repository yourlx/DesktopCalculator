using Calculator.HistoryService.Models.Enums;

namespace Calculator.HistoryService.Models;

public class HistoryEntry
{
    public string Expression { get; }

    public HistoryEntryType Type { get; }

    public MathEntry? MathEntry { get; }

    public GraphEntry? GraphEntry { get; }

    public HistoryEntry(
        string expression,
        HistoryEntryType type,
        MathEntry? mathEntry,
        GraphEntry? graphEntry
    )
    {
        Expression = expression;
        Type = type;
        MathEntry = mathEntry;
        GraphEntry = graphEntry;
    }
}