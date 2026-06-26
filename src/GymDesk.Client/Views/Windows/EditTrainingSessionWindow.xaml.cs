using System.Windows;
using GymDesk.Client.Models;
using GymDesk.Client.Services;
using Microsoft.Extensions.DependencyInjection;
using ClientModel = GymDesk.Client.Models.Client;

namespace GymDesk.Client.Views.Windows;

public partial class EditTrainingSessionWindow : Window
{
    private readonly ApiService _apiService;
    private readonly TrainingSession _originalSession;
    public event Action? SessionUpdatedSuccessfully;

    public EditTrainingSessionWindow(TrainingSession session)
    {
        InitializeComponent();
        _apiService = ((App)Application.Current).Services.GetService<ApiService>()!;
        _originalSession = session;

        LoadDropdownsAndFillForm();
    }

    private async void LoadDropdownsAndFillForm()
    {
        var clients = await _apiService.GetClientsAsync();
        CmbClients.ItemsSource = clients;
        CmbClients.SelectedValue = _originalSession.ClientId;

        var trainers = await _apiService.GetTrainersAsync();
        CmbTrainers.ItemsSource = trainers;
        CmbTrainers.SelectedValue = _originalSession.TrainerId;

        DpSessionDate.SelectedDate = _originalSession.SessionDate.ToDateTime(TimeOnly.MinValue);
        TxtSessionTime.Text = _originalSession.SessionTime.ToString("HH:mm");
        TxtNotes.Text = _originalSession.Notes;
    }

    private async void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        if (CmbClients.SelectedItem is not ClientModel selectedClient ||
            CmbTrainers.SelectedItem is not Trainer selectedTrainer ||
            !DpSessionDate.SelectedDate.HasValue ||
            !TimeOnly.TryParse(TxtSessionTime.Text, out var sessionTime))
        {
            MessageBox.Show("Заполните все обязательные поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        BtnSave.IsEnabled = false;
        BtnSave.Content = "Сохранение...";

        try
        {
            var updatedSession = new TrainingSession
            {
                Id = _originalSession.Id,
                ClientId = selectedClient.Id,
                TrainerId = selectedTrainer.Id,
                SessionDate = DateOnly.FromDateTime(DpSessionDate.SelectedDate.Value),
                SessionTime = sessionTime,
                Notes = TxtNotes.Text.Trim()
            };

            bool isSuccess = await _apiService.UpdateTrainingSessionAsync(_originalSession.Id, updatedSession);

            if (isSuccess)
            {
                SessionUpdatedSuccessfully?.Invoke();
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Не удалось обновить тренировку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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