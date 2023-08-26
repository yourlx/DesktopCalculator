using System.Text.Json.Serialization;

namespace Calculator.HistoryService.Models;

public class GraphVisibleArea
{
    [JsonPropertyName("xmin")]
    public double? XMin { get; }

    [JsonPropertyName("xmax")]
    public double? XMax { get; }

    [JsonPropertyName("ymin")]
    public double? YMin { get; }

    [JsonPropertyName("ymax")]
    public double? YMax { get; }

    public GraphVisibleArea(double? xMin, double? xMax, double? yMin, double? yMax)
    {
        XMin = xMin;
        XMax = xMax;
        YMin = yMin;
        YMax = yMax;
    }
}