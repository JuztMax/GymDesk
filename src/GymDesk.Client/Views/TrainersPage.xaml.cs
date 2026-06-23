using System.Windows;
using System.Windows.Controls;
using GymDesk.Client.Models;
using GymDesk.Client.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GymDesk.Client.Views;

public partial class TrainersPage : UserControl
{
    private readonly ApiService _apiService;

    public TrainersPage()
    {
        InitializeComponent();
        _apiService = ((App)Application.Current).Services.GetService<ApiService>()!;
        LoadData();
    }

    private async void LoadData()
    {
        var data = await _apiService.GetTrainersAsync();
        TrainersDataGrid.ItemsSource = data; // Убедись, что в XAML есть x:Name="TrainersDataGrid"
    }

    private void BtnAdd_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Добавление (в разработке)");
    private void BtnEdit_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Редактирование (в разработке)");
    private void BtnDelete_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Удаление (в разработке)");
}