using System;
using System.Linq;
using Avalonia.Controls;
using Calculator.Application.Views;
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

    [ObservableProperty]
    private int _windowHeight = 450;

    [ObservableProperty]
    private int _windowWidth = 450;

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
        //    
    }

    public void ShowHelp(Window parent)
    {
        var window = new HelpWindow
        {
            ShowInTaskbar = false
        };
        window.ShowDialog(parent);
    }
}