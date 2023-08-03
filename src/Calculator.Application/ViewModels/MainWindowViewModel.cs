using Avalonia.Controls;
using Calculator.Application.Views;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using ReactiveUI;

namespace Calculator.Application.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private string _expression = string.Empty;
    private bool _isGraphSelected;
    private bool _isHistorySelected;
    private bool _isMathSelected = true;

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

    private int _windowHeight = 450;
    private int _windowWidth = 450;

    public ISeries[] Series
    {
        get => _series;
        set => this.RaiseAndSetIfChanged(ref _series, value);
    }

    public int MaxExpressionLength { get; init; } = 255;

    public string Expression
    {
        get => _expression;
        set => this.RaiseAndSetIfChanged(ref _expression, value);
    }

    public int WindowHeight
    {
        get => _windowHeight;
        set => this.RaiseAndSetIfChanged(ref _windowHeight, value);
    }

    public int WindowWidth
    {
        get => _windowWidth;
        set => this.RaiseAndSetIfChanged(ref _windowWidth, value);
    }

    public bool IsMathSelected
    {
        get => _isMathSelected;
        set
        {
            this.RaiseAndSetIfChanged(ref _isMathSelected, value);
            ResizeWindow();
        }
    }

    public bool IsGraphSelected
    {
        get => _isGraphSelected;
        set
        {
            this.RaiseAndSetIfChanged(ref _isGraphSelected, value);
            ResizeWindow();
        }
    }

    public bool IsHistorySelected
    {
        get => _isHistorySelected;
        set
        {
            this.RaiseAndSetIfChanged(ref _isHistorySelected, value);
            ResizeWindow();
        }
    }

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

    private void ResizeWindow()
    {
        WindowWidth = IsMathSelected ? 450 : 900;
    }

    public void Calculate()
    {
        if (IsGraphSelected)
        {
            //
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