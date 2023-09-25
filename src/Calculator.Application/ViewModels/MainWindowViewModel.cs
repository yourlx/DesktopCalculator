using System.Collections.ObjectModel;
using System.Globalization;
using Calculator.Core.MathService;
using Calculator.HistoryService;
using Calculator.HistoryService.Models;
using Calculator.HistoryService.Models.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;

namespace Calculator.Application.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private IHistoryService _historyService;

    private readonly IMathService _mathService;

    [ObservableProperty]
    private NumberFormatInfo _numberStyle = NumberFormatInfo.InvariantInfo;

    [ObservableProperty]
    private int _maxExpressionLength = 255;

    [ObservableProperty]
    private string _expression = string.Empty;

    [ObservableProperty]
    private bool _isExpressionCorrect = false;

    [ObservableProperty]
    private double? _variable = null;

    [ObservableProperty]
    private bool _isExpressionWithVariable = false;

    [ObservableProperty]
    private bool _isGraphSelected = false;

    [ObservableProperty]
    private ObservableCollection<ISeries> _series = new()
    {
        new LineSeries<ObservablePoint>
        {
            GeometryFill = null,
            GeometryStroke = null,
            Fill = null
        }
    };

    [ObservableProperty]
    private ObservableCollection<Axis> _xAxes = new(new[] { new Axis() });

    [ObservableProperty]
    private ObservableCollection<Axis> _yAxes = new(new[] { new Axis() });

    [ObservableProperty]
    private double? _xMin = -5;

    [ObservableProperty]
    private double? _xMax = 5;

    [ObservableProperty]
    private double? _yMin = -1;

    [ObservableProperty]
    private double? _yMax = 1;

    [ObservableProperty]
    private HistoryEntry? _selectedHistoryEntry;

    [ObservableProperty]
    private int _currentTabIndex = 1;

    private bool _saveToHistory = true;

    public MainWindowViewModel(
        IMathService mathService,
        IHistoryService historyService
    )
    {
        _mathService = mathService;
        _historyService = historyService;
    }

    partial void OnExpressionChanged(string value)
    {
        Expression = value.ToLower();

        _mathService.SetExpression(Expression);

        IsExpressionCorrect = _mathService.CheckExpressionValid();
        IsExpressionWithVariable = IsExpressionCorrect && value.Contains('x');
    }

    partial void OnSelectedHistoryEntryChanged(HistoryEntry? value)
    {
        if (value == null)
        {
            return;
        }

        SelectedHistoryEntry = null;
        Expression = value.Expression;
        CurrentTabIndex = value.Type == HistoryEntryType.Math ? 1 : 2;

        if (value.Type == HistoryEntryType.Math)
        {
            Variable = value.MathEntry!.Variable;
        }
        else if (value.Type == HistoryEntryType.Graph)
        {
            _saveToHistory = false;
            XMin = value.GraphEntry!.XMin;
            XMax = value.GraphEntry!.XMax;
            YMin = value.GraphEntry!.YMin;
            YMax = value.GraphEntry!.YMax;
            Calculate();
        }
    }

    public void AddTokenToExpression(string token)
    {
        if (Expression.Length + token.Length <= MaxExpressionLength)
        {
            Expression += token;
        }
    }

    public void RemoveLastSymbolFromExpression()
    {
        Expression = string.IsNullOrWhiteSpace(Expression) ? string.Empty : Expression[..^1];
    }

    public void ClearExpression()
    {
        Expression = string.Empty;
    }

    public void Calculate()
    {
        if (!IsExpressionCorrect)
        {
            return;
        }

        if (!IsGraphSelected && IsExpressionWithVariable && Variable is null)
        {
            return;
        }

        if (XMin == null || XMax == null || YMin == null || YMax == null)
        {
            return;
        }

        if (!IsExpressionWithVariable && Variable is not null)
        {
            Variable = null;
        }

        var originalExpression = Expression;

        if (IsGraphSelected)
        {
            CalculateGraph();
        }
        else
        {
            CalculateExpression();
        }

        if (_saveToHistory)
        {
            SaveToHistory(originalExpression);
        }

        _saveToHistory = true;
    }

    private void SaveToHistory(string originalExpression)
    {
        var entryType = IsGraphSelected ? HistoryEntryType.Graph : HistoryEntryType.Math;

        var mathEntry = IsGraphSelected ? null : new MathEntry(Expression, Variable);
        var graphEntry = IsGraphSelected ? new GraphEntry(XMin, XMax, YMin, YMax) : null;

        var historyEntry = new HistoryEntry(
            expression: originalExpression,
            entryType,
            mathEntry,
            graphEntry);

        HistoryService.SaveToHistory(historyEntry);
    }

    private void CalculateExpression()
    {
        Expression = _mathService.Calculate(Variable ?? 0).ToString("0.#######", CultureInfo.InvariantCulture);
    }

    private void CalculateGraph()
    {
        const int numberOfPoints = 2500;
        var step = (XMax!.Value - XMin!.Value) / numberOfPoints;

        var values = new ObservablePoint[numberOfPoints];

        for (var i = 0; i < numberOfPoints; ++i)
        {
            var x = XMin.Value + step * i;
            double? y = _mathService.Calculate(x);
            if (y < YMin * 3 || y > YMax * 3)
            {
                y = null;
            }

            values[i] = new ObservablePoint(x, y);
        }

        XAxes[0].MinLimit = XMin;
        XAxes[0].MaxLimit = XMax;

        YAxes[0].MinLimit = YMin;
        YAxes[0].MaxLimit = YMax;

        Series[0].Values = values;
    }
}