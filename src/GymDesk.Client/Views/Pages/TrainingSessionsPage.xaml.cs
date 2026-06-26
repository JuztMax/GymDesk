using System.Windows;
using System.Windows.Controls;
using GymDesk.Client.Models;
using GymDesk.Client.Services;
using GymDesk.Client.Views.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace GymDesk.Client.Views.Pages;

public partial class TrainingSessionsPage : UserControl
{
    private readonly ApiService _apiService;

    public TrainingSessionsPage()
    {
        InitializeComponent();
        _apiService = ((App)Application.Current).Services.GetService<ApiService>()!;
        LoadSessions();
    }

    private async Task LoadSessions()
    {
        var sessions = await _apiService.GetTrainingSessionsAsync();
        SessionsDataGrid.ItemsSource = sessions;
    }

    private void BtnAdd_Click(object sender, RoutedEventArgs e)
    {
        var addWindow = new AddTrainingSessionWindow();
        addWindow.SessionAddedSuccessfully += async () => await LoadSessions();
        addWindow.ShowDialog();
    }

    private void BtnEdit_Click(object sender, RoutedEventArgs e)
    {
        if (SessionsDataGrid.SelectedItem is not TrainingSession selectedSession)
        {
            MessageBox.Show("Выберите тренировку для редактирования!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var editWindow = new EditTrainingSessionWindow(selectedSession);
        editWindow.SessionUpdatedSuccessfully += async () => await LoadSessions();
        editWindow.ShowDialog();
    }

    private async void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (SessionsDataGrid.SelectedItem is not TrainingSession selectedSession)
        {
            MessageBox.Show("Выберите тренировку для удаления!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var result = MessageBox.Show(
            $"Удалить запись о тренировке #{selectedSession.Id}?",
            "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes) return;

        bool isSuccess = await _apiService.DeleteTrainingSessionAsync(selectedSession.Id);

        if (!isSuccess)
            MessageBox.Show("Не удалось удалить тренировку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

        await LoadSessions();
    }
}