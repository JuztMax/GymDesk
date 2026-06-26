using System.Windows;
using System.Windows.Controls;
using GymDesk.Client.Models;
using GymDesk.Client.Services;
using Microsoft.Extensions.DependencyInjection;
using ClientModel = GymDesk.Client.Models.Client;

namespace GymDesk.Client.Views.Windows;

public partial class AddSubscriptionWindow : Window
{
    private readonly ApiService _apiService;
    public event Action? SubscriptionAddedSuccessfully;

    public AddSubscriptionWindow()
    {
        InitializeComponent();
        _apiService = ((App)Application.Current).Services.GetService<ApiService>()!;

        // Подписываемся на события
        CmbType.SelectionChanged += CmbType_SelectionChanged;
        DpStartDate.SelectedDateChanged += DpStartDate_SelectedDateChanged;

        LoadClientsIntoComboBox();
    }

    // Реагирует на выбор типа абонемента
    private void CmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (CmbType.SelectedItem is ComboBoxItem selectedItem)
        {
            string type = selectedItem.Content.ToString();

            if (type == "Разовый")
            {
                DpEndDate.IsEnabled = false;
                DpEndDate.SelectedDate = DpStartDate.SelectedDate;
            }
            else if (type == "Месячный")
            {
                DpEndDate.IsEnabled = true;
                if (DpStartDate.SelectedDate.HasValue)
                    DpEndDate.SelectedDate = DpStartDate.SelectedDate.Value.AddMonths(1);
            }
            else if (type == "Годовой") // 👇 НОВОЕ: автозаполнение для годового
            {
                DpEndDate.IsEnabled = true;
                if (DpStartDate.SelectedDate.HasValue)
                    DpEndDate.SelectedDate = DpStartDate.SelectedDate.Value.AddYears(1);
            }
            else
            {
                DpEndDate.IsEnabled = true;
            }
        }
    }

    // Пересчитывает дату окончания при изменении даты начала
    private void DpStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
        if (CmbType.SelectedItem is ComboBoxItem selectedItem &&
            DpStartDate.SelectedDate.HasValue)
        {
            string type = selectedItem.Content.ToString();

            if (type == "Месячный")
                DpEndDate.SelectedDate = DpStartDate.SelectedDate.Value.AddMonths(1);
            else if (type == "Годовой") // 👇 НОВОЕ: пересчет для годового
                DpEndDate.SelectedDate = DpStartDate.SelectedDate.Value.AddYears(1);
        }
    }

    private async void LoadClientsIntoComboBox()
    {
        var clients = await _apiService.GetClientsAsync();
        CmbClients.ItemsSource = clients;
        if (clients.Any()) CmbClients.SelectedIndex = 0;
    }

    private async void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        if (CmbClients.SelectedItem is not ClientModel selectedClient ||
            CmbType.SelectedItem is not ComboBoxItem selectedTypeItem ||
            !DpStartDate.SelectedDate.HasValue)
        {
            MessageBox.Show("Заполните все обязательные поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        DateOnly startDate = DateOnly.FromDateTime(DpStartDate.SelectedDate.Value);
        DateOnly endDate;
        string type = selectedTypeItem.Content.ToString();

        if (type == "Разовый")
        {
            endDate = startDate;
        }
        else
        {
            if (!DpEndDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Укажите дату окончания абонемента!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            endDate = DateOnly.FromDateTime(DpEndDate.SelectedDate.Value);
        }

        BtnSave.IsEnabled = false;
        BtnSave.Content = "Сохранение...";

        try
        {
            var newSub = new Subscription
            {
                ClientId = selectedClient.Id,
                Type = type,
                StartDate = startDate,
                EndDate = endDate,
                Price = decimal.TryParse(TxtPrice.Text, out var price) ? price : 0
            };

            bool isSuccess = await _apiService.CreateSubscriptionAsync(newSub);

            if (isSuccess)
            {
                SubscriptionAddedSuccessfully?.Invoke();
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Не удалось создать абонемент.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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