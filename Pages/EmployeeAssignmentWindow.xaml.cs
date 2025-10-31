using Microsoft.EntityFrameworkCore;
using PEOcleanWPFApp.Data;
using PEOcleanWPFApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace PEOcleanWPFApp.Pages;

public partial class EmployeeAssignmentWindow : Window, INotifyPropertyChanged
{
    private readonly ApplicationDbContext _context;
    private ObservableCollection<EmployeeAssignment> _assignments = new();
    
    public EmployeeAssignmentWindow()
    {
        InitializeComponent();
        _context = App.GetDbContext();
        LoadData();
    }

    private void LoadData()
    {
        LoadEmployees();
        LoadHouses();
        LoadAssignments();
    }

    private void LoadEmployees()
    {
        var employees = _context.Employees.OrderBy(e => e.FullName).ToList();
        EmployeeFilterComboBox.ItemsSource = employees;
        EmployeeFilterComboBox.DisplayMemberPath = "FullName";
    }

    private void LoadHouses()
    {
        var houses = _context.ServiceAddresses.OrderBy(sa => sa.Address).ToList();
        HouseFilterComboBox.ItemsSource = houses;
        HouseFilterComboBox.DisplayMemberPath = "Address";
    }

    private void LoadAssignments()
    {
        var assignments = _context.EmployeeAssignments
            .Include(ea => ea.Employee)
            .Include(ea => ea.ServiceAddress)
            .OrderBy(ea => ea.Employee.FullName)
            .ToList();

        _assignments = new ObservableCollection<EmployeeAssignment>(assignments);
        AssignmentsDataGrid.ItemsSource = _assignments;
    }

    private void FilterButton_Click(object sender, RoutedEventArgs e)
    {
        // TODO: Реализовать фильтрацию
    }

    private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
    {
        // TODO: Очистить фильтры
    }

    private void AddAssignmentButton_Click(object sender, RoutedEventArgs e)
    {
        // TODO: Открыть окно добавления назначения
    }

    private void EditAssignmentButton_Click(object sender, RoutedEventArgs e)
    {
        // TODO: Открыть окно редактирования назначения
    }

    private void DeleteAssignmentButton_Click(object sender, RoutedEventArgs e)
    {
        // TODO: Удалить назначение
    }

    private void ExportButton_Click(object sender, RoutedEventArgs e)
    {
        // TODO: Экспорт в Excel
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}