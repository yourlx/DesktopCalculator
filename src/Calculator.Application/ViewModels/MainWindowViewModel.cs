using System;
using System.Globalization;
using System.Linq;
using Calculator.Core.Math;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;

namespace Calculator.Application.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _expression = string.Empty;

    [ObservableProperty]
    private bool _isGraphSelected;

    [ObservableProperty]
    private ISeries[] _series =
    {
        new LineSeries<double>
        {
            Values = new double[] { 4, 6, 5, 3, -3, -1, 2 }
        },
        new ColumnSeries<double>
        {
            Values = new double[] { 2, 5, 4, -2, 4, -3, 5 }
        }
    };

    private readonly MathCalculatorWrapper _calculator = new();

    [ObservableProperty]
    private bool _isExpressionCorrect;

    partial void OnExpressionChanged(string value)
    {
        IsExpressionCorrect = _calculator.CheckValidAndCovert(value);
    }

    public int MaxExpressionLength { get; init; } = 255;

    public void AddTokenToExpression(string token)
    {
        if (Expression.Length + token.Length <= MaxExpressionLength) Expression += token;
    }

    public void RemoveLastSymbolFromExpression()
    {
        Expression = string.IsNullOrEmpty(Expression) ? string.Empty : Expression[..^1];
    }

    public void ClearExpression()
    {
        Expression = string.Empty;
    }

    public void Calculate()
    {
        if (!IsExpressionCorrect) return;

        if (IsGraphSelected)
        {
            var numbers = new double[] { -3, -2, -1, 0, 1, 2, 3 };
            Series = new ISeries[]
            {
                new LineSeries<ObservablePoint>
                {
                    Values = numbers.Select(x => new ObservablePoint(x, Math.Sin(x))).ToArray()
                },
            };
        }
        else
        {
            Expression = _calculator.Calculate().ToString("0.#######", CultureInfo.InvariantCulture);
        }
    }
}