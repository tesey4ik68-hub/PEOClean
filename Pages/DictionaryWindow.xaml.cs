using PEOcleanWPFApp.Data;
using PEOcleanWPFApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace PEOcleanWPFApp.Pages;

/// <summary>
/// Interaction logic for DictionaryWindow.xaml
/// </summary>
public partial class DictionaryWindow : Window, INotifyPropertyChanged
{
    private ApplicationDbContext _context;
    private string _currentDictionary = "Сотрудники";

    public ObservableCollection<Employee> Employees { get; set; }
    public ObservableCollection<ServiceAddress> ServiceAddresses { get; set; }
    public ObservableCollection<WorkType> WorkTypes { get; set; }

    public DictionaryWindow()
    {
        InitializeComponent();
        _context = App.GetDbContext();

        Employees = new ObservableCollection<Employee>();
        ServiceAddresses = new ObservableCollection<ServiceAddress>();
        WorkTypes = new ObservableCollection<WorkType>();

        DataContext = this;
        LoadData();
        DictionaryListBox.SelectedIndex = 0; // Выбрать сотрудников по умолчанию
    }

    private void LoadData()
    {
        Employees.Clear();
        foreach (var employee in _context.Employees.OrderBy(e => e.FullName).ToList())
        {
            Employees.Add(employee);
        }

        ServiceAddresses.Clear();
        foreach (var address in _context.ServiceAddresses.OrderBy(a => a.Address).ToList())
        {
            ServiceAddresses.Add(address);
        }

        WorkTypes.Clear();
        foreach (var workType in _context.WorkTypes.OrderBy(w => w.Name).ToList())
        {
            WorkTypes.Add(workType);
        }
    }

    private void DictionaryListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DictionaryListBox.SelectedItem is ListBoxItem item)
        {
            _currentDictionary = item.Content.ToString()!;
            UpdateDataGrid();
        }
    }

    private void UpdateDataGrid()
    {
        DataGrid.Columns.Clear();

        switch (_currentDictionary)
        {
            case "Сотрудники":
                DataGrid.ItemsSource = Employees;
                DataGrid.Columns.Add(new DataGridTextColumn { Header = "ФИО", Binding = new System.Windows.Data.Binding("FullName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
                DataGrid.Columns.Add(new DataGridTextColumn { Header = "Номер телефона", Binding = new System.Windows.Data.Binding("Phone"), Width = 150 });
                DataGrid.Columns.Add(new DataGridTextColumn { Header = "Тип сотрудника", Binding = new System.Windows.Data.Binding("EmployeeType"), Width = 150 });
                DataGrid.Columns.Add(new DataGridTextColumn { Header = "Примечание", Binding = new System.Windows.Data.Binding("Notes"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
                break;

            case "Адреса обслуживания":
                DataGrid.ItemsSource = ServiceAddresses;
                DataGrid.Columns.Add(new DataGridTextColumn { Header = "Адрес", Binding = new System.Windows.Data.Binding("Address"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
                DataGrid.Columns.Add(new DataGridTextColumn { Header = "Этажность", Binding = new System.Windows.Data.Binding("Floors"), Width = 100 });
                DataGrid.Columns.Add(new DataGridTextColumn { Header = "Подъездов", Binding = new System.Windows.Data.Binding("Entrances"), Width = 100 });
                DataGrid.Columns.Add(new DataGridTextColumn { Header = "Площадь двора", Binding = new System.Windows.Data.Binding("YardArea"), Width = 120 });
                DataGrid.Columns.Add(new DataGridTextColumn { Header = "Мусоропровод", Binding = new System.Windows.Data.Binding("GarbageChuteType"), Width = 120 });
                break;

            case "Виды работ":
                DataGrid.ItemsSource = WorkTypes;
                DataGrid.Columns.Add(new DataGridTextColumn { Header = "Название", Binding = new System.Windows.Data.Binding("Name"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
                DataGrid.Columns.Add(new DataGridTextColumn { Header = "Описание", Binding = new System.Windows.Data.Binding("Description"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
                break;
        }
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        switch (_currentDictionary)
        {
            case "Сотрудники":
                var employeeWindow = new EmployeeEditWindow();
                if (employeeWindow.ShowDialog() == true)
                {
                    LoadData();
                    UpdateDataGrid();
                }
                break;

            case "Адреса обслуживания":
                var addressWindow = new ServiceAddressEditWindow();
                if (addressWindow.ShowDialog() == true)
                {
                    LoadData();
                    UpdateDataGrid();
                }
                break;

            case "Виды работ":
                var workTypeWindow = new WorkTypeEditWindow();
                if (workTypeWindow.ShowDialog() == true)
                {
                    LoadData();
                    UpdateDataGrid();
                }
                break;
        }
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        if (DataGrid.SelectedItem != null)
        {
            switch (_currentDictionary)
            {
                case "Сотрудники":
                    if (DataGrid.SelectedItem is Employee employee)
                    {
                        var employeeWindow = new EmployeeEditWindow(employee);
                        if (employeeWindow.ShowDialog() == true)
                        {
                            LoadData();
                            UpdateDataGrid();
                        }
                    }
                    break;

                case "Адреса обслуживания":
                    if (DataGrid.SelectedItem is ServiceAddress address)
                    {
                        var addressWindow = new ServiceAddressEditWindow(address);
                        if (addressWindow.ShowDialog() == true)
                        {
                            LoadData();
                            UpdateDataGrid();
                        }
                    }
                    break;

                case "Виды работ":
                    if (DataGrid.SelectedItem is WorkType workType)
                    {
                        var workTypeWindow = new WorkTypeEditWindow(workType);
                        if (workTypeWindow.ShowDialog() == true)
                        {
                            LoadData();
                            UpdateDataGrid();
                        }
                    }
                    break;
            }
        }
        else
        {
            MessageBox.Show("Выберите элемент для редактирования");
        }
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (DataGrid.SelectedItem != null)
        {
            var result = MessageBox.Show("Вы уверены, что хотите удалить выбранный элемент?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                switch (_currentDictionary)
                {
                    case "Сотрудники":
                        if (DataGrid.SelectedItem is Employee employee)
                        {
                            Employees.Remove(employee);
                            if (employee.Id > 0)
                            {
                                _context.Employees.Remove(employee);
                            }
                        }
                        break;

                    case "Адреса обслуживания":
                        if (DataGrid.SelectedItem is ServiceAddress address)
                        {
                            ServiceAddresses.Remove(address);
                            if (address.Id > 0)
                            {
                                _context.ServiceAddresses.Remove(address);
                            }
                        }
                        break;

                    case "Виды работ":
                        if (DataGrid.SelectedItem is WorkType workType)
                        {
                            WorkTypes.Remove(workType);
                            if (workType.Id > 0)
                            {
                                _context.WorkTypes.Remove(workType);
                            }
                        }
                        break;
                }
            }
        }
        else
        {
            MessageBox.Show("Выберите элемент для удаления");
        }
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            _context.SaveChanges();
            MessageBox.Show("Изменения сохранены успешно");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        LoadData(); // Перезагрузить данные из базы
        UpdateDataGrid();
        MessageBox.Show("Изменения отменены");
    }

    private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
        {
            DragMove();
        }
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
        Close();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
