using System.Windows;
using GymDesk.Client.Models;
using GymDesk.Client.Services;
using Microsoft.Extensions.DependencyInjection;
using ClientModel = GymDesk.Client.Models.Client;

namespace GymDesk.Client.Views.Windows;

public partial class AddClientWindow : Window
{
    private readonly ApiService _apiService;

    // Событие для уведомления родительского окна об успешном добавлении
    public event Action? ClientAddedSuccessfully;

    public AddClientWindow()
    {
        InitializeComponent();

        // Получаем сервис через глобальный контейнер App
        _apiService = ((App)Application.Current).Services.GetService<ApiService>()!;
    }

    private async void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        // 1. Простая валидация обязательных полей
        if (string.IsNullOrWhiteSpace(TxtFirstName.Text) ||
            string.IsNullOrWhiteSpace(TxtLastName.Text))
        {
            MessageBox.Show("Поля 'Имя' и 'Фамилия' обязательны для заполнения!",
                            "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // 2. Блокируем интерфейс на время запроса
        BtnSave.IsEnabled = false;
        BtnSave.Content = "Сохранение...";

        try
        {
            // 3. Создаем объект модели
            var newClient = new ClientModel
            {
                FirstName = TxtFirstName.Text.Trim(),
                LastName = TxtLastName.Text.Trim(),
                Phone = TxtPhone.Text.Trim(),
                Email = TxtEmail.Text.Trim()
                // RegistrationDate обычно ставится на бэкенде автоматически
            };

            // 4. Отправляем POST-запрос
            bool isSuccess = await _apiService.CreateClientAsync(newClient);

            if (isSuccess)
            {
                // Уведомляем подписчиков (ClientsPage), что нужно обновить список
                ClientAddedSuccessfully?.Invoke();

                // Закрываем окно с результатом "ОК"
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Не удалось сохранить клиента. Проверьте подключение к API.",
                                "Ошибка сети", MessageBoxButton.OK, MessageBoxImage.Error);
                BtnSave.IsEnabled = true;
                BtnSave.Content = "Сохранить";
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Произошла непредвиденная ошибка: {ex.Message}",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            BtnSave.IsEnabled = true;
            BtnSave.Content = "Сохранить";
        }
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false; // Закрываем окно с результатом "Отмена"
    }
}