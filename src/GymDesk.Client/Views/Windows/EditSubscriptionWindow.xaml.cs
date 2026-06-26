using System.Windows;
using System.Windows.Controls;
using GymDesk.Client.Models;
using GymDesk.Client.Services;
using Microsoft.Extensions.DependencyInjection;
using ClientModel = GymDesk.Client.Models.Client;

namespace GymDesk.Client.Views.Windows;

public partial class EditSubscriptionWindow : Window
{
    private readonly ApiService _apiService;
    private readonly Subscription _originalSub;
    public event Action? SubscriptionUpdatedSuccessfully;

    public EditSubscriptionWindow(Subscription subscription)
    {
        InitializeComponent();
        _apiService = ((App)Application.Current).Services.GetService<ApiService>()!;
        _originalSub = subscription;

        // Подписываемся на события ДО загрузки данных
        CmbType.SelectionChanged += CmbType_SelectionChanged;
        DpStartDate.SelectedDateChanged += DpStartDate_SelectedDateChanged;

        LoadClientsAndFillForm();
    }

    // Логика переключения дат (UI-часть)
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

    // Пересчитывает дату окончания, если пользователь изменил дату начала
    private void DpStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
        if (CmbType.SelectedItem is ComboBoxItem selectedItem &&
            DpStartDate.SelectedDate.HasValue)
        {
            string type = selectedItem.Content.ToString();

            if (type == "Месячный")
                DpEndDate.SelectedDate = DpStartDate.SelectedDate.Value.AddMonths(1);
            else if (type == "Годовой") //  НОВОЕ: пересчет для годового
                DpEndDate.SelectedDate = DpStartDate.SelectedDate.Value.AddYears(1);
        }
    }

    private async void LoadClientsAndFillForm()
    {
        var clients = await _apiService.GetClientsAsync();
        CmbClients.ItemsSource = clients;

        CmbClients.SelectedValue = _originalSub.ClientId;

        foreach (ComboBoxItem item in CmbType.Items)
        {
            if (item.Content.ToString() == _originalSub.Type)
            {
                CmbType.SelectedItem = item;
                break;
            }
        }

        DpStartDate.SelectedDate = _originalSub.StartDate.ToDateTime(TimeOnly.MinValue);
        DpEndDate.SelectedDate = _originalSub.EndDate.ToDateTime(TimeOnly.MinValue);
        TxtPrice.Text = _originalSub.Price.ToString();

        // Применяем логику блокировки сразу после заполнения формы
        if (_originalSub.Type == "Разовый")
        {
            DpEndDate.IsEnabled = false;
        }
        else
        {
            DpEndDate.IsEnabled = true;
        }
    }

    private async void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        // 1. Базовая проверка обязательных полей
        if (CmbClients.SelectedItem is not ClientModel selectedClient ||
            CmbType.SelectedItem is not ComboBoxItem selectedTypeItem ||
            !DpStartDate.SelectedDate.HasValue)
        {
            MessageBox.Show("Заполните все обязательные поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // 2. ЖЕСТКАЯ ЛОГИКА РАСЧЕТА ДАТ (не зависит от UI)
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

        // 3. Отправка данных
        BtnSave.IsEnabled = false;
        BtnSave.Content = "Сохранение...";

        try
        {
            var updatedSub = new Subscription
            {
                Id = _originalSub.Id,
                ClientId = selectedClient.Id,
                Type = type,
                StartDate = startDate,
                EndDate = endDate,
                Price = decimal.TryParse(TxtPrice.Text, out var price) ? price : 0
            };

            bool isSuccess = await _apiService.UpdateSubscriptionAsync(_originalSub.Id, updatedSub);

            if (isSuccess)
            {
                SubscriptionUpdatedSuccessfully?.Invoke();
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Не удалось обновить абонемент.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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