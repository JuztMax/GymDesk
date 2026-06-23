using System.Windows;
using System.Windows.Controls;
using GymDesk.Client.Services;
using Microsoft.Extensions.DependencyInjection;
using ClientModel = GymDesk.Client.Models.Client; 

namespace GymDesk.Client.Views;

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

    private async void LoadClients()
    {
        var clients = await _apiService.GetClientsAsync();
        ClientsDataGrid.ItemsSource = clients;
    }

    // Заглушки для кнопок
    private void BtnAdd_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Здесь будет окно добавления клиента!");
    }

    private void BtnEdit_Click(object sender, RoutedEventArgs e)
    {
        // 👇 Исправлено: Client заменен на ClientModel
        if (ClientsDataGrid.SelectedItem is ClientModel selected)
            MessageBox.Show($"Редактируем: {selected.FirstName}");
        else
            MessageBox.Show("Выберите клиента!");
    }

    private void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        // 👇 Исправлено: Client заменен на ClientModel
        if (ClientsDataGrid.SelectedItem is ClientModel selected)
            MessageBox.Show($"Удаляем: {selected.FirstName}? (Пока заглушка)");
        else
            MessageBox.Show("Выберите клиента!");
    }
}