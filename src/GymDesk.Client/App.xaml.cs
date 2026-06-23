using System.Windows;
using GymDesk.Client.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GymDesk.Client.Views.Windows;

namespace GymDesk.Client;

public partial class App : Application
{
    private IHost? _host;

    // Свойство для доступа к сервисам из MainWindow
    public IServiceProvider Services => _host!.Services;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddHttpClient<ApiService>();
            })
            .Build();

        await _host.StartAsync();

        // Создаем и показываем главное окно вручную
        var mainWindow = new MainWindow();
        mainWindow.Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        if (_host != null)
            await _host.StopAsync();

        base.OnExit(e);
    }
}