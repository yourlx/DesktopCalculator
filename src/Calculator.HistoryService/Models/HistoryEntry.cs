using System.Text.Json.Serialization;

namespace Calculator.HistoryService.Models;

public class HistoryEntry
{
    [JsonPropertyName("expression")]
    public string Expression { get; }

    [JsonPropertyName("answer")]
    public string? Answer { get; }

    [JsonPropertyName("variable")]
    public double? Variable { get; }

    [JsonPropertyName("graphVisibleArea")]
    public GraphVisibleArea? GraphVisibleArea { get; }

    public HistoryEntry(string expression,
        double? variable = null,
        string? answer = null,
        GraphVisibleArea? graphVisibleArea = null)
    {
        Expression = expression;
        Variable = variable;
        Answer = answer;
        GraphVisibleArea = graphVisibleArea;
    }
}