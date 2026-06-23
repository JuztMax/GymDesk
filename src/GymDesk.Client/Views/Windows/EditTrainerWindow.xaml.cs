using System.Windows;
using GymDesk.Client.Models;
using GymDesk.Client.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GymDesk.Client.Views.Windows;

public partial class EditTrainerWindow : Window
{
    private readonly ApiService _apiService;
    private readonly Trainer _originalTrainer;

    // Событие для уведомления родительской страницы об успехе
    public event Action? TrainerUpdatedSuccessfully;

    public EditTrainerWindow(Trainer trainer)
    {
        InitializeComponent();

        _apiService = ((App)Application.Current).Services.GetService<ApiService>()!;
        _originalTrainer = trainer;

        // Заполняем поля текущими данными тренера
        TxtFirstName.Text = trainer.FirstName;
        TxtLastName.Text = trainer.LastName;
        TxtSpecialization.Text = trainer.Specialization;
        TxtPhone.Text = trainer.Phone;
    }

    private async void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        // Валидация обязательных полей
        if (string.IsNullOrWhiteSpace(TxtFirstName.Text) ||
            string.IsNullOrWhiteSpace(TxtLastName.Text) ||
            string.IsNullOrWhiteSpace(TxtSpecialization.Text))
        {
            MessageBox.Show("Поля 'Имя', 'Фамилия' и 'Специализация' обязательны!",
                            "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Блокируем кнопку на время запроса
        BtnSave.IsEnabled = false;
        BtnSave.Content = "Сохранение...";

        try
        {
            // Создаем объект с обновленными данными (ID сохраняем оригинальный!)
            var updatedTrainer = new Trainer
            {
                Id = _originalTrainer.Id,
                FirstName = TxtFirstName.Text.Trim(),
                LastName = TxtLastName.Text.Trim(),
                Specialization = TxtSpecialization.Text.Trim(),
                Phone = TxtPhone.Text.Trim()
            };

            // Отправляем PUT-запрос
            bool isSuccess = await _apiService.UpdateTrainerAsync(_originalTrainer.Id, updatedTrainer);

            if (isSuccess)
            {
                TrainerUpdatedSuccessfully?.Invoke(); // Обновляем таблицу
                DialogResult = true; // Закрываем окно успешно
            }
            else
            {
                MessageBox.Show("Не удалось обновить данные тренера.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                BtnSave.IsEnabled = true;
                BtnSave.Content = "Сохранить";
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            BtnSave.IsEnabled = true;
            BtnSave.Content = "Сохранить";
        }
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false; // Закрываем окно без сохранения
    }
}