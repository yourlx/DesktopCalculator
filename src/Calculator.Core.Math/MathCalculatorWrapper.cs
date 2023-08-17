using System.Runtime.InteropServices;

namespace Calculator.Core.Math;

public class MathCalculatorWrapper : IDisposable
{
    private readonly IntPtr _calculator = CreateInstance();

    ~MathCalculatorWrapper()
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

    public bool CheckValid(string expression)
    {
        return CheckValid(_calculator, expression);
    }

    public void ConvertToPolish()
    {
        ConvertToPolish(_calculator);
    }

    public double Calculate(double x = 0)
    {
        return Calculate(_calculator, x);
    }

    [DllImport("calculatorcore")]
    private static extern bool CheckValid(IntPtr ptr, string expression);

    [DllImport("calculatorcore")]
    private static extern bool ConvertToPolish(IntPtr ptr);

    [DllImport("calculatorcore")]
    private static extern double Calculate(IntPtr ptr, double x = 0);

    [DllImport("calculatorcore")]
    private static extern IntPtr CreateInstance();

    [DllImport("calculatorcore")]
    private static extern void FreeInstance(IntPtr ptr);
}