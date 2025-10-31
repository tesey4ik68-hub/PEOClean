using PEOcleanWPFApp.Data;
using PEOcleanWPFApp.Models;
using System.Windows;
using System.Windows.Input;

namespace PEOcleanWPFApp.Pages;

/// <summary>
/// Логика взаимодействия для EmployeeEditWindow.xaml
/// </summary>
public partial class EmployeeEditWindow : Window
{
    private ApplicationDbContext _context;
    private Employee _employee;
    private bool _isNew;

    public EmployeeEditWindow(Employee? employee = null)
    {
        InitializeComponent();
        _context = App.GetDbContext();

        _isNew = employee == null;
        _employee = employee ?? new Employee();

        LoadData();
    }

    private void LoadData()
    {
        FullNameTextBox.Text = _employee.FullName;
        PhoneTextBox.Text = _employee.Phone;
        NotesTextBox.Text = _employee.Notes;
        IsActiveCheckBox.IsChecked = _employee.IsActive;
        IsJanitorCheckBox.IsChecked = _employee.IsJanitor;
        IsCleanerCheckBox.IsChecked = _employee.IsCleaner;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(FullNameTextBox.Text))
        {
            MessageBox.Show("Пожалуйста, введите ФИО сотрудника", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        _employee.FullName = FullNameTextBox.Text.Trim();
        _employee.Phone = PhoneTextBox.Text.Trim();
        _employee.Notes = NotesTextBox.Text.Trim();
        _employee.IsActive = IsActiveCheckBox.IsChecked ?? false;
        _employee.IsJanitor = IsJanitorCheckBox.IsChecked ?? false;
        _employee.IsCleaner = IsCleanerCheckBox.IsChecked ?? false;

        try
        {
            if (_isNew)
            {
                _context.Employees.Add(_employee);
            }
            _context.SaveChanges();
            DialogResult = true;
            Close();
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
