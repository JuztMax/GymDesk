using GymDesk.Client.Services;
using GymDesk.Client.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using ClientModel = GymDesk.Client.Models.Client;

namespace GymDesk.Client.Views.Pages;

public partial class ClientsPage : UserControl
{
    private readonly ApiService _apiService;

    public ClientsPage()
    {
        InitializeComponent();

        // Получаем сервис через глобальный DI контейнер
        _apiService = ((App)Application.Current).Services.GetService<ApiService>()!;

        // Загружаем данные сразу при открытии страницы
        LoadClients();
    }

    private async Task LoadClients()
    {
        var clients = await _apiService.GetClientsAsync();
        ClientsDataGrid.ItemsSource = clients;
    }

    // Заглушки для кнопок
    private void BtnAdd_Click(object sender, RoutedEventArgs e)
    {
        // Создаем экземпляр окна добавления
        var addWindow = new AddClientWindow();

        // Подписываемся на событие успеха: как только клиент добавлен, грузим список заново
        addWindow.ClientAddedSuccessfully += async () => await LoadClients();

        // ShowDialog открывает окно модально (блокирует основное окно)
        addWindow.ShowDialog();
    }

    private void BtnEdit_Click(object sender, RoutedEventArgs e)
    {
        // Проверяем, выбран ли клиент
        if (ClientsDataGrid.SelectedItem is not ClientModel selectedClient)
        {
            MessageBox.Show("Выберите клиента для редактирования!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        // Передаем выбранного клиента в конструктор окна
        var editWindow = new EditClientWindow(selectedClient);

        // Подписываемся на обновление списка после сохранения
        editWindow.ClientUpdatedSuccessfully += async () => await LoadClients();

        editWindow.ShowDialog();
    }

    private async void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        // 1. Проверяем, выбран ли клиент
        if (ClientsDataGrid.SelectedItem is not ClientModel selectedClient)
        {
            MessageBox.Show("Выберите клиента для удаления!", "Внимание",
                            MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        // 2. Диалог подтверждения
        var result = MessageBox.Show(
            $"Вы действительно хотите удалить клиента: {selectedClient.FirstName} {selectedClient.LastName}?\nЭто действие нельзя отменить.",
            "Подтверждение удаления",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes)
            return;

        // 3. Отправляем запрос на удаление
        bool isSuccess = await _apiService.DeleteClientAsync(selectedClient.Id);

        if (!isSuccess)
        {
            // Показываем ошибку ТОЛЬКО если что-то пошло не так
            MessageBox.Show("Не удалось удалить клиента. Проверьте подключение к API.",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        // Если успех — просто обновляем таблицу без лишних окон
        await LoadClients();
    }
}