using System.Runtime.InteropServices;

namespace Calculator.Core.MathService;

public class CppMathService : IMathService
{
    private readonly IntPtr _calculator = CreateInstance();

    private string _expression = string.Empty;

    ~CppMathService()
    {
        ReleaseUnmanagedResources();
    }

    private void ReleaseUnmanagedResources()
    {
        FreeInstance(_calculator);
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    public void SetExpression(string expression)
    {
        _expression = expression;
    }

    public bool CheckExpressionValid()
    {
        var valid = CheckValid(_calculator, _expression);

        if (valid)
        {
            ConvertToPolish(_calculator);
        }

        return valid;
    }

    public double Calculate(double x = 0)
    {
        return Calculate(_calculator, x);
    }

    [DllImport("calculatorcore")]
    private static extern bool CheckValid(IntPtr ptr, string expression);

    [DllImport("calculatorcore")]
    private static extern void ConvertToPolish(IntPtr ptr);

    [DllImport("calculatorcore")]
    private static extern double Calculate(IntPtr ptr, double x = 0);

    [DllImport("calculatorcore")]
    private static extern IntPtr CreateInstance();

    [DllImport("calculatorcore")]
    private static extern void FreeInstance(IntPtr ptr);
}