using Calculator.Core.Math;

namespace Calculator.Core.Tests;

public class MathCalculatorWrapperTests
{
    private readonly MathCalculatorWrapper _calculator = new();

    [Test]
    public void CheckValid_CorrectExpressionList_True()
    {
        List<string> correctExpressions = new()
        {
            "-16-3+9*cos(15mod9)+(-9-2)",
            "(x*ln(sin(x)))/sqrt(x^2+4)+cos(x)*sqrt(x^2+4)/sin(x)",
            "(x^2+tan(x)+15)^(1/3)",
            "-5/((x+ln(x))^(1/5))",
            "100.235+x-(x+10)",
            "5-(x+10)",
            "-(3)*(-x-(7*(-(-(-(-(-7)))))))",
            "(8+2*5)/(1+3*2-4)",
            "(-8+x*5)/(3^2-0.4^(-x))",
            "(1+2)*4*cos(x*7-2)+sin(2*x)",
            "4^acos(x/4)/tan(2*x)",
            "ln(55/(2+x))",
            "-sqrt(x^log(5-x))+ln(55/(2+x))",
            "(+1+2)*4*(cos(x*7-2)+sin(2*x))*70^(-10)+5*(-3)",
            "asin(2/x)mod(x)+atan(+x)",
            "1/2*3",
            "2^3^2",
            "10.00",
            ".11",
            "+16-16",
            "-16+16"
        };

        foreach (var expression in correctExpressions)
        {
            Assert.That(_calculator.CheckValid(expression), Is.True);
        }
    }

    [Test]
    public void CheckValid_IncorrectExpressionList_False()
    {
        List<string> incorrectExpressions = new()
        {
            "(3-)2",
            "(x*ln(sin(x)))/sqrt(x^2+4)+cos(x)d*sqrt(x^2+4)/sin(x)",
            "16..",
            "(16+15",
            "+",
            "-",
            "mod55+3",
            "-----5",
            "++3",
            "aboba!",
            "yourlx",
            "-(3)(-x-(7(-(-(-(--7))))))",
            "1+2)4(cos(x*7-2)+sin(2*x))70^(-10)+5(-3)",
            "(8+2*5)/(1+3)*2)-((4)"
        };
        
        foreach (var expression in incorrectExpressions)
        {
            Assert.That(_calculator.CheckValid(expression), Is.False);
        }
    }
}