using System.Text.Json.Serialization;

namespace Calculator.HistoryService.Models;

public class HistoryEntry
{
    [JsonPropertyName("expression")]
    public string Expression { get; }

    public HistoryEntry(string expression)
    {
        Expression = expression;
    }
}