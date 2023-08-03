using ReactiveUI;

namespace Calculator.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private string _expression = string.Empty;
    private bool _isGraphSelected;
    private bool _isHistorySelected;
    private bool _isMathSelected = true;

    private int _windowHeight = 450;
    private int _windowWidth = 450;

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
}