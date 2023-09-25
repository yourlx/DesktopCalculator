namespace Calculator.HistoryService.Models;

public class MathEntry
{
    public string Answer { get; }

    public double? Variable { get; }

    public MathEntry(
        string answer,
        double? variable
    )
    {
        Answer = answer;
        Variable = variable;
    }
}