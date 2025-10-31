using PEOcleanWPFApp.Data;
using PEOcleanWPFApp.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PEOcleanWPFApp.Pages;

/// <summary>
/// Логика взаимодействия для ServiceAddressEditWindow.xaml
/// </summary>
public partial class ServiceAddressEditWindow : Window
{
    private ApplicationDbContext _context;
    private ServiceAddress _serviceAddress;
    private bool _isNew;

    public ServiceAddressEditWindow(ServiceAddress? serviceAddress = null)
    {
        InitializeComponent();
        _context = App.GetDbContext();

        _isNew = serviceAddress == null;
        _serviceAddress = serviceAddress ?? new ServiceAddress();

        LoadData();
    }

    private void LoadData()
    {
        AddressTextBox.Text = _serviceAddress.Address;
        FloorsTextBox.Text = _serviceAddress.Floors.ToString();
        EntrancesTextBox.Text = _serviceAddress.Entrances.ToString();
        ApartmentsTextBox.Text = _serviceAddress.Apartments.ToString();
        HouseAreaTextBox.Text = _serviceAddress.HouseArea.ToString();
        YardAreaTextBox.Text = _serviceAddress.YardArea.ToString();
        MonthlyRateJanitorTextBox.Text = _serviceAddress.JanitorRate.ToString();
        MonthlyRateCleanerTextBox.Text = _serviceAddress.CleanerRate.ToString();
        ConstructionYearTextBox.Text = _serviceAddress.ConstructionYear.ToString();

        ObjectTypeComboBox.Text = _serviceAddress.ObjectType ?? "Многоквартирный дом";
        BuildingTypeComboBox.Text = _serviceAddress.BuildingType ?? "панельный";
        GarbageChuteTypeComboBox.Text = _serviceAddress.GarbageChuteType ?? "отсутствует";
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(AddressTextBox.Text))
        {
            MessageBox.Show("Пожалуйста, введите адрес", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        try
        {
            _serviceAddress.Address = AddressTextBox.Text.Trim();
            _serviceAddress.Floors = int.Parse(FloorsTextBox.Text);
            _serviceAddress.Entrances = int.Parse(EntrancesTextBox.Text);
            _serviceAddress.Apartments = int.Parse(ApartmentsTextBox.Text);
            _serviceAddress.HouseArea = decimal.Parse(HouseAreaTextBox.Text);
            _serviceAddress.YardArea = decimal.Parse(YardAreaTextBox.Text);
            _serviceAddress.JanitorRate = decimal.Parse(MonthlyRateJanitorTextBox.Text);
            _serviceAddress.CleanerRate = decimal.Parse(MonthlyRateCleanerTextBox.Text);
            _serviceAddress.ConstructionYear = int.Parse(ConstructionYearTextBox.Text);

            _serviceAddress.ObjectType = ObjectTypeComboBox.Text;
            _serviceAddress.BuildingType = BuildingTypeComboBox.Text;
            _serviceAddress.GarbageChuteType = GarbageChuteTypeComboBox.Text;

            if (_isNew)
            {
                _context.ServiceAddresses.Add(_serviceAddress);
            }
            _context.SaveChanges();
            DialogResult = true;
            Close();
        }
        catch (FormatException)
        {
            MessageBox.Show("Пожалуйста, проверьте правильность введенных числовых значений", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void MinimizeButton_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void MaximizeButton_Click(object sender, RoutedEventArgs e)
    {
        if (WindowState == WindowState.Maximized)
        {
            WindowState = WindowState.Normal;
        }
        else
        {
            WindowState = WindowState.Maximized;
        }
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ButtonState == MouseButtonState.Pressed)
        {
            DragMove();
        }
    }
}
