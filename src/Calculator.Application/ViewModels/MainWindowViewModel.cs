using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using Calculator.Core.Math;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;

namespace Calculator.Application.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    #region Observable properties

    [ObservableProperty]
    private string _expression = string.Empty;

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
    private bool _isExpressionCorrect = false;

    [ObservableProperty]
    private int _maxExpressionLength = 255;

    [ObservableProperty]
    private double? _xMin = -5;

    [ObservableProperty]
    private double? _xMax = 5;

    [ObservableProperty]
    private double? _yMin = -1;

    [ObservableProperty]
    private double? _yMax = 1;

    [ObservableProperty]
    private double? _variable = 0;

    [ObservableProperty]
    private bool _isExpressionWithVariable = false;

    [ObservableProperty]
    private NumberFormatInfo _numberStyle = NumberFormatInfo.InvariantInfo;

    #endregion

    private readonly MathCalculatorWrapper _calculator = new();

    public MainWindowViewModel()
    {
        PropertyChanged += OnPropertyChangedEventHandler;
    }

    private void OnPropertyChangedEventHandler(object? sender, PropertyChangedEventArgs args)
    {
        if (args.PropertyName is nameof(XMin) or nameof(XMax) or nameof(YMin) or nameof(YMax))
        {
            Calculate();
        }
    }

    #region Events

    partial void OnExpressionChanged(string value)
    {
        Expression = value.ToLower();
        IsExpressionCorrect = _calculator.CheckValid(value);
        if (IsExpressionCorrect)
        {
            IsExpressionWithVariable = value.Contains('x');
            _calculator.ConvertToPolish();
        }
    }

    #endregion

    #region Commands

    public void AddTokenToExpression(string token)
    {
        if (Expression.Length + token.Length <= MaxExpressionLength) Expression += token;
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
        if (!IsExpressionCorrect) return;

        if (IsGraphSelected) CalculateGraph();
        else CalculateExpression();
    }

    #endregion

    #region Math

    private void CalculateExpression()
    {
        Expression = _calculator.Calculate(Variable!.Value).ToString("0.#######", CultureInfo.InvariantCulture);
    }

    #endregion

    #region Graph

    private void CalculateGraph()
    {
        const int numberOfPoints = 500;
        var step = (XMax!.Value - XMin!.Value) / numberOfPoints;

        var values = new ObservablePoint[numberOfPoints];

        for (var i = 0; i < numberOfPoints; ++i)
        {
            var x = XMin.Value + step * i;
            double? y = _calculator.Calculate(x);
            if (y < YMin || y > YMax) y = null;
            values[i] = new ObservablePoint(x, y);
        }

        XAxes[0].MinLimit = XMin;
        XAxes[0].MaxLimit = XMax;

        YAxes[0].MinLimit = YMin;
        YAxes[0].MaxLimit = YMax;

        Series[0].Values = values;
    }

    #endregion
}