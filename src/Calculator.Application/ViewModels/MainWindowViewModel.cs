using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Calculator.Core.Math;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;

namespace Calculator.Application.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly MathCalculatorWrapper _calculator = new();

    #region Observable properties

    [ObservableProperty]
    private string _expression = string.Empty;

    [ObservableProperty]
    private bool _isGraphSelected = false;

    [ObservableProperty]
    private ObservableCollection<ISeries> _series = new();

    [ObservableProperty]
    private bool _isExpressionCorrect = false;

    [ObservableProperty]
    private int _maxExpressionLength = 255;

    #endregion

    #region Events

    partial void OnExpressionChanged(string value)
    {
        IsExpressionCorrect = _calculator.CheckValidAndCovert(value);
    }

    #endregion

    #region Commands

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

        if (IsGraphSelected) CalculateGraph();
        else CalculateExpression();
    }

    #endregion

    #region Math

    private void CalculateExpression()
    {
        Expression = _calculator.Calculate().ToString("0.#######", CultureInfo.InvariantCulture);
    }

    #endregion

    #region Graph

    private void CalculateGraph()
    {
        Series.Clear();

        var values = new List<ObservablePoint>();

        Series.Add(new LineSeries<ObservablePoint>
        {
            Values = values.ToArray(),
        });
    }

    #endregion
}