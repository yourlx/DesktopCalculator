using Calculator.Core.MathService;

namespace Calculator.Core.Tests;

public class CppMathServiceTests
{
    private readonly IMathService _mathService = new CppMathService();

    [OneTimeTearDown]
    public void Cleanup()
    {
        _mathService.Dispose();
    }

    [TestCase("-16-3+9*cos(15mod9)+(-9-2)")]
    [TestCase("(x*ln(sin(x)))/sqrt(x^2+4)+cos(x)*sqrt(x^2+4)/sin(x)")]
    [TestCase("(x^2+tan(x)+15)^(1/3)")]
    [TestCase("-5/((x+ln(x))^(1/5))")]
    [TestCase("100.235+x-(x+10)")]
    [TestCase("5-(x+10)")]
    [TestCase("-(3)*(-x-(7*(-(-(-(-(-7)))))))")]
    [TestCase("(8+2*5)/(1+3*2-4)")]
    [TestCase("(-8+x*5)/(3^2-0.4^(-x))")]
    [TestCase("(1+2)*4*cos(x*7-2)+sin(2*x)")]
    [TestCase("4^acos(x/4)/tan(2*x)")]
    [TestCase("ln(55/(2+x))")]
    [TestCase("-sqrt(x^log(5-x))+ln(55/(2+x))")]
    [TestCase("(+1+2)*4*(cos(x*7-2)+sin(2*x))*70^(-10)+5*(-3)")]
    [TestCase("asin(2/x)mod(x)+atan(+x)")]
    [TestCase("1/2*3")]
    [TestCase("2^3^2")]
    [TestCase("10.00")]
    [TestCase(".11")]
    [TestCase("+16-16")]
    [TestCase("-16+16")]
    public void CheckValid_CorrectExpression_True(string expression)
    {
        _mathService.SetExpression(expression);
        Assert.That(_mathService.CheckExpressionValid, Is.True);
    }

    [TestCase("(3-)2")]
    [TestCase("(x*ln(sin(x)))/sqrt(x^2+4)+cos(x)d*sqrt(x^2+4)/sin(x)")]
    [TestCase("16..")]
    [TestCase("(16+15")]
    [TestCase("+")]
    [TestCase("-")]
    [TestCase("mod55+3")]
    [TestCase("-----5")]
    [TestCase("++3")]
    [TestCase("aboba!")]
    [TestCase("yourlx")]
    [TestCase("-(3)(-x-(7(-(-(-(--7))))))")]
    [TestCase("1+2)4(cos(x*7-2)+sin(2*x))70^(-10)+5(-3)")]
    [TestCase("(8+2*5)/(1+3)*2)-((4)")]
    public void CheckValid_IncorrectExpression_False(string expression)
    {
        _mathService.SetExpression(expression);
        Assert.That(_mathService.CheckExpressionValid, Is.False);
    }

    [TestCase("0", null, 0)]
    [TestCase("x", 0, 0)]
    [TestCase("(x^2+tan(x)+15)^(1/3)", 5, 3.3207594)]
    [TestCase("2^3^2", null, 512)]
    [TestCase("sqrt(25)", null, 5)]
    [TestCase("sin(asin(x))", 1, 1)]
    [TestCase("3+4*2/(1-5)^2", null, 3.5)]
    [TestCase("2*(15+x-3*cos(15-6)+5-9-13.7334+2^3^2)/1024", 0, 1)]
    [TestCase("-3^2", null, -9)]
    [TestCase("1e+6-9", null, 999991)]
    [TestCase("100.235+x-(x+10)", null, 90.2350)]
    [TestCase("-(3)*(-x-(7*(-(-(-(-(-7)))))))", 6, -129)]
    [TestCase("2.5+2.45", null, 4.95)]
    [TestCase("-0.910*3", null, -2.73)]
    [TestCase("5mod4", null, 1)]
    public void Calculate_ExpressionAndVariable_Result(string expression, double? variable, double result)
    {
        _mathService.SetExpression(expression);
        Assert.Multiple(() =>
        {
            Assert.That(_mathService.CheckExpressionValid, Is.True);
            Assert.That(_mathService.Calculate(variable ?? 0), Is.EqualTo(result).Within(0.0000001));
        });
    }
}