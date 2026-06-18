using System.Windows;
using GymDesk.Client.Services;
using Microsoft.Extensions.DependencyInjection;
using ClientModel = GymDesk.Client.Models.Client;

namespace GymDesk.Client;

public partial class ClientsView : Window
{
    private readonly ApiService _apiService;

    public ClientsView()
    {
        InitializeComponent();

        // Получаем сервис через DI контейнер приложения
        _apiService = ((App)Application.Current).Services.GetService<ApiService>()!;

        // Загружаем данные сразу при открытии окна
        Loaded += async (s, e) => await LoadClientsAsync();
    }

    private async Task LoadClientsAsync()
    {
        var clients = await _apiService.GetClientsAsync();
        ClientsDataGrid.ItemsSource = clients;
    }

    private async void AddClient_Click(object sender, RoutedEventArgs e)
    {
        // TODO: Открыть форму добавления клиента
        MessageBox.Show("Форма добавления клиента будет здесь!", "Инфо",
            MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private async void EditClient_Click(object sender, RoutedEventArgs e)
    {
        if (ClientsDataGrid.SelectedItem is not ClientModel selectedClient)
        {
            MessageBox.Show("Выберите клиента для редактирования!", "Внимание",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // TODO: Открыть форму редактирования с данными selectedClient
        MessageBox.Show($"Редактирование: {selectedClient.FirstName} {selectedClient.LastName}",
            "Инфо", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private async void DeleteClient_Click(object sender, RoutedEventArgs e)
    {
        if (ClientsDataGrid.SelectedItem is not ClientModel selectedClient)
        {
            MessageBox.Show("Выберите клиента для удаления!", "Внимание",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var result = MessageBox.Show(
            $"Вы уверены, что хотите удалить клиента {selectedClient.FirstName} {selectedClient.LastName}?",
            "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            // TODO: Вызвать метод удаления в ApiService
            MessageBox.Show("Удаление будет реализовано на следующем шаге!", "Инфо",
                MessageBoxButton.OK, MessageBoxImage.Information);

            // После успешного удаления нужно перезагрузить список:
            // await LoadClientsAsync();
        }
    }
}