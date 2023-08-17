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

    public bool CheckValidAndConvert(string expression)
    {
        return CheckValidAndConvert(_calculator, expression);
    }

    public double Calculate(double x = 0)
    {
        return Calculate(_calculator, x);
    }

    [DllImport("calculatorcore")]
    private static extern bool CheckValidAndConvert(IntPtr ptr, string expression);

    [DllImport("calculatorcore")]
    private static extern double Calculate(IntPtr ptr, double x = 0);

    [DllImport("calculatorcore")]
    private static extern IntPtr CreateInstance();

    [DllImport("calculatorcore")]
    private static extern void FreeInstance(IntPtr ptr);
}