using System.Windows;
using System.Windows.Controls;
using GymDesk.Client.Models;
using GymDesk.Client.Services;
using GymDesk.Client.Views.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace GymDesk.Client.Views.Pages;

public partial class SubscriptionsPage : UserControl
{
    private readonly ApiService _apiService;

    public SubscriptionsPage()
    {
        InitializeComponent();
        _apiService = ((App)Application.Current).Services.GetService<ApiService>()!;
        LoadSubscriptions(); // 👈 Правильное имя метода
    }

    private async Task LoadSubscriptions()
    {
        var subs = await _apiService.GetSubscriptionsAsync();
        SubscriptionsDataGrid.ItemsSource = subs;
    }

    private void BtnAdd_Click(object sender, RoutedEventArgs e)
    {
        var addWindow = new AddSubscriptionWindow();
        addWindow.SubscriptionAddedSuccessfully += async () => await LoadSubscriptions();
        addWindow.ShowDialog();
    }

    private void BtnEdit_Click(object sender, RoutedEventArgs e)
    {
        if (SubscriptionsDataGrid.SelectedItem is not Subscription selectedSub)
        {
            MessageBox.Show("Выберите абонемент!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var editWindow = new EditSubscriptionWindow(selectedSub);
        editWindow.SubscriptionUpdatedSuccessfully += async () => await LoadSubscriptions();
        editWindow.ShowDialog();
    }

    private async void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (SubscriptionsDataGrid.SelectedItem is not Subscription selectedSub)
        {
            MessageBox.Show("Выберите абонемент!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var result = MessageBox.Show(
            $"Удалить абонемент #{selectedSub.Id}?",
            "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes) return;

        bool isSuccess = await _apiService.DeleteSubscriptionAsync(selectedSub.Id);

        if (!isSuccess)
            MessageBox.Show("Не удалось удалить.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

        await LoadSubscriptions();
    }
}