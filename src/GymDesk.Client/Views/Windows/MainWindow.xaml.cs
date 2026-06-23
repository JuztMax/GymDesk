using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using GymDesk.Client.Services;
using Microsoft.Extensions.DependencyInjection;
using GymDesk.Client.Views.Pages;

namespace GymDesk.Client.Views.Windows;

public partial class MainWindow : Window
{
    private readonly ApiService _apiService;

    public MainWindow()
    {
        InitializeComponent();

        // Получаем сервис через DI контейнер приложения
        _apiService = ((App)Application.Current).Services.GetService<ApiService>()!;

        // Загружаем страницу клиентов в Frame при старте
        ContentFrame.Navigate(new ClientsPage());
    }

    // Обработчик кликов по кнопкам меню
    private void NavButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is string pageName)
        {
            UserControl? page = pageName switch
            {
                "ClientsPage" => new ClientsPage(),
                "TrainersPage" => new TrainersPage(),
                "SubscriptionsPage" => new SubscriptionsPage(),
                "TrainingSessionsPage" => new TrainingSessionsPage(),
                _ => null
            };

            if (page != null)
                ContentFrame.Navigate(page);
        }
    }
}