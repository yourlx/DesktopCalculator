Desktop Calculator
============

<div align="center">

![dotnet](https://img.shields.io/badge/.NET%20-8-512bd4)
![avalonia](https://img.shields.io/badge/AvaloniaUI-11.0.10-512bd4)
![livecharts](https://img.shields.io/badge/LiveCharts-2.0.0%20rc2-512bd4)
![cpp](https://img.shields.io/badge/C++%20-17-512bd4)

</div>

This is a project to get acquainted with Avalonia UI and the use of C++ dynamic libraries.

Calculator allows you to evaluate arithmetic expressions and build graphs.

![gif](./misc/calculator.gif)

## Features

All features that available to user are explained in "Help" tab in UI. You can see
content [here](./src/Calculator.Application/Assets/Help/Help.md).

![gif](./misc/help.gif)

## Running application

Before anything, you should compile C++ code (with clang or gcc). I recommend using CMake as it provided with code. Also, to run application you have to install .NET 8.

> cd src/Calculator.Core.MathService/CalculatorCore/ \
> cmake . \
> make

### Running from CLI:

> cd src/Calculator.Application \
> dotnet restore \
> dotnet run

### Running tests:

> cd src/Calculator.Core.Tests \
> dotnet restore \
> dotnet test

Of course you can do it with IDEs of your choice!

## Libraries and Frameworks used in this project

- Avalonia UI
- Markdown.Avalonia
- Community Toolkit MVVM
- Microsoft Dependency Injection
- LiveCharts 2
- NUnit

## Things I wish I could change

- Better code/architecture :D
- A way to automatically compile C++ source code to dll
- An installer for this app

Anyway, this is my study project and I think it's maybe even archived or hidden in private in future =)

Also, C++ part was written some years ago and I wanted to rewrite it for a long time, but I don't wanna do it.

## License

This repository code is under MIT License. See [LICENSE file](LICENSE) for more info.
