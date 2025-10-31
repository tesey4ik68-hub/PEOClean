using Microsoft.EntityFrameworkCore;
using PEOcleanWPFApp.Data;
using PEOcleanWPFApp.Models;
using System.Windows;

namespace PEOcleanWPFApp.Pages;

public partial class EmployeeAssignmentEditWindow : Window
{
    private readonly ApplicationDbContext _context;
    private EmployeeAssignment? _assignment;
    private bool _isEdit;

    public EmployeeAssignmentEditWindow()
    {
        InitializeComponent();
        _context = App.GetDbContext();
        _isEdit = false;
        LoadData();
        InitializeControls();
    }

    public EmployeeAssignmentEditWindow(EmployeeAssignment assignment) : this()
    {
        _assignment = assignment;
        _isEdit = true;
        LoadAssignmentData();
    }

    private void LoadData()
    {
        var employees = _context.Employees.OrderBy(e => e.FullName).ToList();
        EmployeeComboBox.ItemsSource = employees;

        var houses = _context.ServiceAddresses.OrderBy(sa => sa.Address).ToList();
        HouseComboBox.ItemsSource = houses;
    }

    private void InitializeControls()
    {
        StartDatePicker.SelectedDate = DateTime.Today;
    }

    private void LoadAssignmentData()
    {
        if (_assignment == null) return;

        EmployeeComboBox.SelectedItem = _assignment.Employee;
        HouseComboBox.SelectedItem = _assignment.ServiceAddress;
        
        JanitorCheckBox.IsChecked = _assignment.Role == EmployeeRole.Janitor;
        CleanerCheckBox.IsChecked = _assignment.Role == EmployeeRole.Cleaner;
        
        StartDatePicker.SelectedDate = _assignment.StartDate;
        EndDatePicker.SelectedDate = _assignment.EndDate;
        IsPrimaryCheckBox.IsChecked = _assignment.IsPrimary;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        // Проверка заполнения обязательных полей
        if (EmployeeComboBox.SelectedItem == null)
        {
            MessageBox.Show("Пожалуйста, выберите сотрудника", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (HouseComboBox.SelectedItem == null)
        {
            MessageBox.Show("Пожалуйста, выберите дом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (StartDatePicker.SelectedDate == null)
        {
            MessageBox.Show("Пожалуйста, укажите дату начала", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!(JanitorCheckBox.IsChecked ?? false) && !(CleanerCheckBox.IsChecked ?? false))
        {
            MessageBox.Show("Пожалуйста, выберите хотя бы одну роль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Создание или обновление назначения
        if (!_isEdit || _assignment == null)
        {
            // Создаем новые назначения для каждой выбранной роли
            if (JanitorCheckBox.IsChecked == true)
            {
                CreateAssignment(EmployeeRole.Janitor);
            }

            if (CleanerCheckBox.IsChecked == true)
            {
                CreateAssignment(EmployeeRole.Cleaner);
            }
        }
        else
        {
            // Обновляем существующее назначение
            UpdateAssignment();
        }

        DialogResult = true;
        Close();
    }

    private void CreateAssignment(EmployeeRole role)
    {
        var employee = (Employee)EmployeeComboBox.SelectedItem;
        var house = (ServiceAddress)HouseComboBox.SelectedItem;

        // Проверка на дубликаты
        var existingAssignment = _context.EmployeeAssignments
            .FirstOrDefault(ea => ea.EmployeeId == employee.Id && 
                                 ea.ServiceAddressId == house.Id && 
                                 ea.Role == role &&
                                 ea.EndDate == null);

        if (existingAssignment != null)
        {
            MessageBox.Show($"Сотрудник уже назначен в роли '{(role == EmployeeRole.Janitor ? "Дворник" : "Уборщик")}' на этот дом", 
                           "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var assignment = new EmployeeAssignment
        {
            EmployeeId = employee.Id,
            ServiceAddressId = house.Id,
            Role = role,
            StartDate = StartDatePicker.SelectedDate ?? DateTime.Today,
            EndDate = EndDatePicker.SelectedDate,
            IsPrimary = IsPrimaryCheckBox.IsChecked ?? false
        };

        _context.EmployeeAssignments.Add(assignment);
        _context.SaveChanges();
    }

    private void UpdateAssignment()
    {
        if (_assignment == null) return;

        _assignment.EmployeeId = ((Employee)EmployeeComboBox.SelectedItem).Id;
        _assignment.ServiceAddressId = ((ServiceAddress)HouseComboBox.SelectedItem).Id;
        _assignment.StartDate = StartDatePicker.SelectedDate ?? DateTime.Today;
        _assignment.EndDate = EndDatePicker.SelectedDate;
        _assignment.IsPrimary = IsPrimaryCheckBox.IsChecked ?? false;

        // Определяем роль на основе выбранного чекбокса
        if (JanitorCheckBox.IsChecked == true)
            _assignment.Role = EmployeeRole.Janitor;
        else if (CleanerCheckBox.IsChecked == true)
            _assignment.Role = EmployeeRole.Cleaner;

        _context.SaveChanges();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}