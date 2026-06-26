using System.Windows;
using GymDesk.Client.Models;
using GymDesk.Client.Services;
using Microsoft.Extensions.DependencyInjection;
using ClientModel = GymDesk.Client.Models.Client;

namespace GymDesk.Client.Views.Windows;

public partial class AddTrainingSessionWindow : Window
{
    private readonly ApiService _apiService;
    public event Action? SessionAddedSuccessfully;

    public AddTrainingSessionWindow()
    {
        InitializeComponent();
        _apiService = ((App)Application.Current).Services.GetService<ApiService>()!;

        LoadDropdowns();
    }

    private async void LoadDropdowns()
    {
        // Загружаем оба списка параллельно или последовательно
        var clients = await _apiService.GetClientsAsync();
        CmbClients.ItemsSource = clients;
        if (clients.Any()) CmbClients.SelectedIndex = 0;

        var trainers = await _apiService.GetTrainersAsync();
        CmbTrainers.ItemsSource = trainers;
        if (trainers.Any()) CmbTrainers.SelectedIndex = 0;
    }

    private async void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        // Валидация
        if (CmbClients.SelectedItem is not ClientModel selectedClient ||
            CmbTrainers.SelectedItem is not Trainer selectedTrainer ||
            !DpSessionDate.SelectedDate.HasValue ||
            !TimeOnly.TryParse(TxtSessionTime.Text, out var sessionTime))
        {
            MessageBox.Show("Заполните все обязательные поля!\nВремя должно быть в формате ЧЧ:ММ (например, 18:30)",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        BtnSave.IsEnabled = false;
        BtnSave.Content = "Сохранение...";

        try
        {
            var newSession = new TrainingSession
            {
                ClientId = selectedClient.Id,
                TrainerId = selectedTrainer.Id,
                SessionDate = DateOnly.FromDateTime(DpSessionDate.SelectedDate.Value),
                SessionTime = sessionTime,
                Notes = TxtNotes.Text.Trim()
            };

            bool isSuccess = await _apiService.CreateTrainingSessionAsync(newSession);

            if (isSuccess)
            {
                SessionAddedSuccessfully?.Invoke();
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Не удалось создать тренировку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                BtnSave.IsEnabled = true;
                BtnSave.Content = "Сохранить";
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            BtnSave.IsEnabled = true;
            BtnSave.Content = "Сохранить";
        }
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
}