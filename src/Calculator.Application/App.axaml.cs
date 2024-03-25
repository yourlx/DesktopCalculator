using System;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Calculator.Application.ViewModels;
using Calculator.Application.Views;
using Calculator.Core.MathService;
using Calculator.HistoryService;
using Microsoft.Extensions.DependencyInjection;

namespace Calculator.Application;

public class App : Avalonia.Application
{
    private IServiceProvider _serviceProvider;
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        var services = new ServiceCollection();

        services.AddSingleton<MainWindowViewModel>();
        
        services.AddSingleton<IMathService, CppMathService>();
        services.AddSingleton<IHistoryService, JsonHistoryService>();

        _serviceProvider = services.BuildServiceProvider();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var viewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = new MainWindow
            {
                DataContext = viewModel
            };

        base.OnFrameworkInitializationCompleted();
    }
}