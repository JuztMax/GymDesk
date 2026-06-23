using System.Windows;
using GymDesk.Client.Models;
using GymDesk.Client.Services;
using Microsoft.Extensions.DependencyInjection;
using ClientModel = GymDesk.Client.Models.Client;

namespace GymDesk.Client.Views.Windows;

public partial class EditClientWindow : Window
{
    private readonly ApiService _apiService;
    private readonly ClientModel _originalClient; // Храним оригинал, чтобы знать ID

    public event Action? ClientUpdatedSuccessfully;

    // Конструктор принимает клиента для редактирования
    public EditClientWindow(ClientModel client)
    {
        InitializeComponent();
        _apiService = ((App)Application.Current).Services.GetService<ApiService>()!;
        _originalClient = client;

        // Заполняем поля текущими данными
        TxtFirstName.Text = client.FirstName;
        TxtLastName.Text = client.LastName;
        TxtPhone.Text = client.Phone;
        TxtEmail.Text = client.Email;
    }

    private async void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TxtFirstName.Text) ||
            string.IsNullOrWhiteSpace(TxtLastName.Text))
        {
            MessageBox.Show("Поля 'Имя' и 'Фамилия' обязательны!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        BtnSave.IsEnabled = false;
        BtnSave.Content = "Сохранение...";

        try
        {
            // Создаем обновленный объект (ID берем из оригинала!)
            var updatedClient = new ClientModel
            {
                Id = _originalClient.Id, // 👈 КРИТИЧНО ВАЖНО
                FirstName = TxtFirstName.Text.Trim(),
                LastName = TxtLastName.Text.Trim(),
                Phone = TxtPhone.Text.Trim(),
                Email = TxtEmail.Text.Trim()
            };

            bool isSuccess = await _apiService.UpdateClientAsync(_originalClient.Id, updatedClient);

            if (isSuccess)
            {
                ClientUpdatedSuccessfully?.Invoke();
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Не удалось обновить данные.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}