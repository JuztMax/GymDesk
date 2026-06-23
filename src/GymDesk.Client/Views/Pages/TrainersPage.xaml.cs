using System.Windows;
using System.Windows.Controls;
using GymDesk.Client.Models;
using GymDesk.Client.Services;
using GymDesk.Client.Views.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace GymDesk.Client.Views.Pages;

public partial class TrainersPage : UserControl
{
    private readonly ApiService _apiService;

    public TrainersPage()
    {
        InitializeComponent();
        _apiService = ((App)Application.Current).Services.GetService<ApiService>()!;
        LoadTrainers();
    }

    private async Task LoadTrainers()
    {
        var trainers = await _apiService.GetTrainersAsync();
        TrainersDataGrid.ItemsSource = trainers;
    }

    private void BtnAdd_Click(object sender, RoutedEventArgs e)
    {
        var addWindow = new AddTrainerWindow();
        addWindow.TrainerAddedSuccessfully += async () => await LoadTrainers();
        addWindow.ShowDialog();
    }

    private void BtnEdit_Click(object sender, RoutedEventArgs e)
    {
        if (TrainersDataGrid.SelectedItem is not Trainer selectedTrainer)
        {
            MessageBox.Show("Выберите тренера для редактирования!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        // Примечание: EditTrainerWindow создается по аналогии с AddTrainerWindow
        // Если ты еще не создал окно редактирования, эта кнопка покажет ошибку компиляции.
        // Создай EditTrainerWindow по шаблону AddTrainerWindow, передавая trainer в конструктор.
        var editWindow = new EditTrainerWindow(selectedTrainer);
        editWindow.TrainerUpdatedSuccessfully += async () => await LoadTrainers();
        editWindow.ShowDialog();
    }

    private async void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (TrainersDataGrid.SelectedItem is not Trainer selectedTrainer)
        {
            MessageBox.Show("Выберите тренера для удаления!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var result = MessageBox.Show(
            $"Удалить тренера: {selectedTrainer.FirstName} {selectedTrainer.LastName}?",
            "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes) return;

        bool isSuccess = await _apiService.DeleteTrainerAsync(selectedTrainer.Id);

        if (!isSuccess)
            MessageBox.Show("Не удалось удалить тренера.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

        await LoadTrainers();
    }
}