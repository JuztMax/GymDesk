using System.Windows;
using GymDesk.Client.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GymDesk.Client;

public partial class MainWindow : Window
{
    private readonly ApiService _apiService;

    public MainWindow()
    {
        InitializeComponent();

        // Получаем сервис через DI контейнер приложения
        _apiService = ((App)Application.Current).Services.GetService<ApiService>()!;

        // Сразу открываем окно клиентов
        var clientsView = new ClientsView();
        clientsView.Show();

        // Скрываем главное окно - оно нужно только как "хост" для DI
        this.Hide();
    }
}