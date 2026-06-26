using System.Globalization;
using System.Threading;
using System.Windows;
using GymDesk.Client.Services;
using GymDesk.Client.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GymDesk.Client;

public partial class App : Application
{
    private IHost? _host;

    // Свойство для доступа к сервисам из MainWindow
    public IServiceProvider Services => _host!.Services;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // 👇 ДОБАВЛЕНО: Принудительная русская локаль для всего приложения
        // Исправляет формат дат (dd.MM.yyyy), времени и чисел везде
        var culture = new CultureInfo("ru-RU");
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;

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