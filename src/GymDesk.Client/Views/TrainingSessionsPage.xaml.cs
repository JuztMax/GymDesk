using System.Windows;
using System.Windows.Controls;
using GymDesk.Client.Models;
using GymDesk.Client.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GymDesk.Client.Views;

public partial class TrainingSessionsPage : UserControl
{
    private readonly ApiService _apiService;

    public TrainingSessionsPage()
    {
        InitializeComponent();
        _apiService = ((App)Application.Current).Services.GetService<ApiService>()!;
        LoadData();
    }

    private async void LoadData()
    {
        var data = await _apiService.GetTrainingSessionsAsync();
        SessionsDataGrid.ItemsSource = data;
    }

    private void BtnAdd_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Запись (в разработке)");
    private void BtnEdit_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Изменение (в разработке)");
    private void BtnDelete_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Отмена (в разработке)");
}