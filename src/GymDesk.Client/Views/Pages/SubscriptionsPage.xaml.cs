using System.Windows;
using System.Windows.Controls;
using GymDesk.Client.Models;
using GymDesk.Client.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GymDesk.Client.Views.Pages;

public partial class SubscriptionsPage : UserControl
{
    private readonly ApiService _apiService;

    public SubscriptionsPage()
    {
        InitializeComponent();
        _apiService = ((App)Application.Current).Services.GetService<ApiService>()!;
        LoadData();
    }

    private async void LoadData()
    {
        var data = await _apiService.GetSubscriptionsAsync();
        SubscriptionsDataGrid.ItemsSource = data;
    }

    private void BtnAdd_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Добавление (в разработке)");
    private void BtnEdit_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Редактирование (в разработке)");
    private void BtnDelete_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Удаление (в разработке)");
}