namespace Calculator.Core.Math;

public interface IMathService
{
    void SetExpression(string expression);

    bool CheckExpressionValid();

    double Calculate(double x = 0);
}