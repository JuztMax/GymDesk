using System.Windows;
using GymDesk.Client.Models;
using GymDesk.Client.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GymDesk.Client.Views.Windows;

public partial class AddTrainerWindow : Window
{
    private readonly ApiService _apiService;
    public event Action? TrainerAddedSuccessfully;

    public AddTrainerWindow()
    {
        InitializeComponent();
        _apiService = ((App)Application.Current).Services.GetService<ApiService>()!;
    }

    private async void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TxtFirstName.Text) ||
            string.IsNullOrWhiteSpace(TxtLastName.Text) ||
            string.IsNullOrWhiteSpace(TxtSpecialization.Text))
        {
            MessageBox.Show("Поля 'Имя', 'Фамилия' и 'Специализация' обязательны!",
                            "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        BtnSave.IsEnabled = false;
        BtnSave.Content = "Сохранение...";

        try
        {
            var newTrainer = new Trainer
            {
                FirstName = TxtFirstName.Text.Trim(),
                LastName = TxtLastName.Text.Trim(),
                Specialization = TxtSpecialization.Text.Trim(),
                Phone = TxtPhone.Text.Trim()
            };

            bool isSuccess = await _apiService.CreateTrainerAsync(newTrainer);

            if (isSuccess)
            {
                TrainerAddedSuccessfully?.Invoke();
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Не удалось сохранить тренера.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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