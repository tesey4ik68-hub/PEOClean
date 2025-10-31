using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using PEOcleanWPFApp.Data;
using PEOcleanWPFApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PEOcleanWPFApp.Pages;

public partial class WorkReportWindow : Window, INotifyPropertyChanged
{
    private ApplicationDbContext _context;
    private DateTime _reportDate;
    private Employee? _selectedEmployee;

    public string WindowTitle => $"Запись о работе за {_reportDate:dd MMMM yyyy}";
    public string EmployeeInfo => _selectedEmployee != null ? $"Сотрудник: {_selectedEmployee.FullName}" : "Сотрудник не выбран";

    public ObservableCollection<HouseSelectionItem> AvailableHouses { get; set; }
    public ObservableCollection<HouseWorkItem> SelectedHouses { get; set; }
    public ObservableCollection<WorkTypeSelectionItem> WorkTypes { get; set; }

    public WorkReportWindow(Employee employee, DateTime date)
    {
        InitializeComponent();
        _context = App.GetDbContext();
        _selectedEmployee = employee;
        _reportDate = date;

        AvailableHouses = new ObservableCollection<HouseSelectionItem>();
        SelectedHouses = new ObservableCollection<HouseWorkItem>();
        WorkTypes = new ObservableCollection<WorkTypeSelectionItem>();

        DataContext = this;
        LoadData();
        OnPropertyChanged(nameof(WindowTitle));
        OnPropertyChanged(nameof(EmployeeInfo));
    }

    public WorkReportWindow()
    {
        InitializeComponent();
        _context = App.GetDbContext();
        _reportDate = DateTime.Today;

        AvailableHouses = new ObservableCollection<HouseSelectionItem>();
        SelectedHouses = new ObservableCollection<HouseWorkItem>();
        WorkTypes = new ObservableCollection<WorkTypeSelectionItem>();

        DataContext = this;
        LoadData();
    }

    private void LoadData()
    {
        // Load available houses for selection
        LoadAvailableHouses();

        // Load selected houses with work details
        LoadSelectedHouses();
    }

    private void LoadSelectedHouses()
    {
        SelectedHouses.Clear();

        // Get selected houses from AvailableHouses
        var selectedHouseItems = AvailableHouses.Where(h => h.IsSelected).ToList();

        foreach (var houseItem in selectedHouseItems)
        {
            var houseWorkItem = new HouseWorkItem
            {
                ServiceAddress = houseItem.ServiceAddress
            };

            // Load scheduled works for this house and day
            LoadScheduledWorksForHouse(houseWorkItem);

            // Load actual works (all work types)
            LoadActualWorksForHouse(houseWorkItem);

            SelectedHouses.Add(houseWorkItem);
        }
    }

    private void LoadScheduledWorksForHouse(HouseWorkItem houseWorkItem)
    {
        var dayOfWeek = _reportDate.DayOfWeek;
        var scheduledWorks = _context.HouseWorkSchedules
            .Where(hws => hws.ServiceAddressId == houseWorkItem.ServiceAddress.Id && hws.DayOfWeek == dayOfWeek)
            .Include(hws => hws.WorkType)
            .ToList();

        foreach (var scheduledWork in scheduledWorks)
        {
            houseWorkItem.ScheduledWorks.Add($"{scheduledWork.WorkType.Name}");
        }
    }

    private void LoadActualWorksForHouse(HouseWorkItem houseWorkItem)
    {
        var workTypes = _context.WorkTypes.ToList();
        foreach (var workType in workTypes)
        {
            houseWorkItem.ActualWorks.Add(new WorkTypeSelectionItem { WorkType = workType });
        }
    }

    private void LoadAvailableHouses()
    {
        AvailableHouses.Clear();
        var houses = _context.ServiceAddresses.ToList();

        // Get assigned houses for the employee
        var assignedHouseIds = new List<int>();
        if (_selectedEmployee != null)
        {
            assignedHouseIds = _context.EmployeeServiceAddresses
                .Where(esa => esa.EmployeeId == _selectedEmployee.Id &&
                             (esa.StartDate == null || esa.StartDate <= _reportDate) &&
                             (esa.EndDate == null || esa.EndDate >= _reportDate))
                .Select(esa => esa.ServiceAddressId)
                .ToList();
        }

        foreach (var house in houses)
        {
            var isAssigned = assignedHouseIds.Contains(house.Id);
            AvailableHouses.Add(new HouseSelectionItem
            {
                ServiceAddress = house,
                IsAssigned = isAssigned,
                DisplayName = isAssigned ? $"{house.Address} (закреплён)" : $"{house.Address} (не закреплён)",
                TextColor = isAssigned ? Brushes.Black : Brushes.Gray
            });
        }
    }

    private void LoadWorkTypes()
    {
        WorkTypes.Clear();
        var workTypes = _context.WorkTypes.ToList();
        foreach (var workType in workTypes)
        {
            WorkTypes.Add(new WorkTypeSelectionItem { WorkType = workType });
        }
    }

    private void LoadWorkSchedule()
    {
        if (_selectedEmployee == null)
            return;

        // Get houses assigned to this employee
        var assignedHouseIds = _context.EmployeeServiceAddresses
            .Where(esa => esa.EmployeeId == _selectedEmployee.Id &&
                         (esa.StartDate == null || esa.StartDate <= _reportDate) &&
                         (esa.EndDate == null || esa.EndDate >= _reportDate))
            .Select(esa => esa.ServiceAddressId)
            .ToList();

        // Get scheduled work for this day
        var dayOfWeek = _reportDate.DayOfWeek;
        var scheduledWork = _context.HouseWorkSchedules
            .Where(hws => assignedHouseIds.Contains(hws.ServiceAddressId) && hws.DayOfWeek == dayOfWeek)
            .Select(hws => hws.WorkTypeId)
            .Distinct()
            .ToList();

        // Update work types based on schedule
        foreach (var workTypeItem in WorkTypes)
        {
            workTypeItem.IsScheduled = scheduledWork.Contains(workTypeItem.WorkType.Id);
        }
    }

    private void AttachPhotoButton_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var houseWorkItem = button?.CommandParameter as HouseWorkItem;
        if (houseWorkItem == null) return;

        var openFileDialog = new OpenFileDialog
        {
            Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png",
            Multiselect = true
        };

        if (openFileDialog.ShowDialog() == true)
        {
            // Create Photos directory if it doesn't exist
            var photosDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Photos", _reportDate.ToString("yyyy"), _reportDate.ToString("MM"));
            Directory.CreateDirectory(photosDir);

            foreach (var sourcePath in openFileDialog.FileNames)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(sourcePath)}";
                var destPath = Path.Combine(photosDir, fileName);
                File.Copy(sourcePath, destPath);

                houseWorkItem.PhotoFiles.Add(new PhotoFileItem
                {
                    FileName = Path.GetFileName(sourcePath),
                    FullPath = destPath
                });
            }

            // Trigger property changed for status
            houseWorkItem.OnPropertyChanged(nameof(HouseWorkItem.PhotoStatus));
            houseWorkItem.OnPropertyChanged(nameof(HouseWorkItem.PhotoStatusColor));
        }
    }

    private void NoPhotoButton_Click(object sender, RoutedEventArgs e)
    {
        // This method seems unused in the current implementation
        // It was likely from an older version where photo handling was different
    }

    private void RemovePhotoButton_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var photoFileItem = button?.CommandParameter as PhotoFileItem;
        if (photoFileItem == null) return;

        // Find the house work item that contains this photo
        var houseWorkItem = SelectedHouses.FirstOrDefault(h => h.PhotoFiles.Contains(photoFileItem));
        if (houseWorkItem != null)
        {
            // Remove from collection
            houseWorkItem.PhotoFiles.Remove(photoFileItem);

            // Delete file from disk if it exists
            if (File.Exists(photoFileItem.FullPath))
            {
                File.Delete(photoFileItem.FullPath);
            }

            // Trigger property changed for status
            houseWorkItem.OnPropertyChanged(nameof(HouseWorkItem.PhotoStatus));
            houseWorkItem.OnPropertyChanged(nameof(HouseWorkItem.PhotoStatusColor));
        }
    }

    private void SaveAndCloseButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedEmployee == null)
        {
            MessageBox.Show("Сотрудник не выбран");
            return;
        }

        var selectedHouses = AvailableHouses.Where(h => h.IsSelected).ToList();
        if (!selectedHouses.Any())
        {
            MessageBox.Show("Выберите хотя бы один дом");
            return;
        }

        using var transaction = _context.Database.BeginTransaction();
        try
        {
            // Create attendance record
            var attendanceRecord = new AttendanceRecord
            {
                EmployeeId = _selectedEmployee.Id,
                Date = _reportDate,
                Status = AbsenceType.Worked,
                IsConfirmed = true,
                HasPhoto = SelectedHouses.Any(h => h.PhotoFiles.Any())
            };

            _context.AttendanceRecords.Add(attendanceRecord);
            _context.SaveChanges();

            // Add house associations
            foreach (var houseItem in selectedHouses)
            {
                _context.AttendanceRecordServiceAddresses.Add(new AttendanceRecordServiceAddress
                {
                    AttendanceRecordId = attendanceRecord.Id,
                    ServiceAddressId = houseItem.ServiceAddress.Id
                });
            }

            // Create work reports for each selected house
            foreach (var houseWorkItem in SelectedHouses)
            {
                var houseItem = selectedHouses.First(h => h.ServiceAddress.Id == houseWorkItem.ServiceAddress.Id);

                // Get completed work types for this house
                var completedWorkTypes = houseWorkItem.ActualWorks.Where(wt => wt.IsCompleted).ToList();

                foreach (var workTypeItem in completedWorkTypes)
                {
                    _context.WorkReports.Add(new WorkReport
                    {
                        EmployeeId = _selectedEmployee.Id,
                        ServiceAddressId = houseWorkItem.ServiceAddress.Id,
                        Date = _reportDate,
                        WorkTypeId = workTypeItem.WorkType.Id,
                        IsCompleted = true,
                        Comment = houseWorkItem.HouseComment,
                        PhotoPath = houseWorkItem.PhotoFiles.Any() ? string.Join(";", houseWorkItem.PhotoFiles.Select(p => p.FullPath)) : null
                    });
                }
            }

            _context.SaveChanges();
            transaction.Commit();

            MessageBox.Show("Отчёт сохранён успешно");
            DialogResult = true;
            Close();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
        }
    }

    private void MarkAbsentButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedEmployee == null)
        {
            MessageBox.Show("Сотрудник не выбран");
            return;
        }

        var attendanceRecord = new AttendanceRecord
        {
            EmployeeId = _selectedEmployee.Id,
            Date = _reportDate,
            Status = AbsenceType.NotWorked,
            IsConfirmed = true,
            HasPhoto = false
        };

        _context.AttendanceRecords.Add(attendanceRecord);
        _context.SaveChanges();

        MessageBox.Show("Неявка отмечена");
        DialogResult = true;
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void SaveAndContinueButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedEmployee == null)
        {
            MessageBox.Show("Сотрудник не выбран");
            return;
        }

        var selectedHouses = AvailableHouses.Where(h => h.IsSelected).ToList();
        if (!selectedHouses.Any())
        {
            MessageBox.Show("Выберите хотя бы один дом");
            return;
        }

        using var transaction = _context.Database.BeginTransaction();
        try
        {
            // Create attendance record
            var attendanceRecord = new AttendanceRecord
            {
                EmployeeId = _selectedEmployee.Id,
                Date = _reportDate,
                Status = AbsenceType.Worked,
                IsConfirmed = true,
                HasPhoto = SelectedHouses.Any(h => h.PhotoFiles.Any())
            };

            _context.AttendanceRecords.Add(attendanceRecord);
            _context.SaveChanges();

            // Add house associations
            foreach (var houseItem in selectedHouses)
            {
                _context.AttendanceRecordServiceAddresses.Add(new AttendanceRecordServiceAddress
                {
                    AttendanceRecordId = attendanceRecord.Id,
                    ServiceAddressId = houseItem.ServiceAddress.Id
                });
            }

            // Create work reports for each selected house
            foreach (var houseWorkItem in SelectedHouses)
            {
                var houseItem = selectedHouses.First(h => h.ServiceAddress.Id == houseWorkItem.ServiceAddress.Id);

                // Get completed work types for this house
                var completedWorkTypes = houseWorkItem.ActualWorks.Where(wt => wt.IsCompleted).ToList();

                foreach (var workTypeItem in completedWorkTypes)
                {
                    _context.WorkReports.Add(new WorkReport
                    {
                        EmployeeId = _selectedEmployee.Id,
                        ServiceAddressId = houseWorkItem.ServiceAddress.Id,
                        Date = _reportDate,
                        WorkTypeId = workTypeItem.WorkType.Id,
                        IsCompleted = true,
                        Comment = houseWorkItem.HouseComment,
                        PhotoPath = houseWorkItem.PhotoFiles.Any() ? string.Join(";", houseWorkItem.PhotoFiles.Select(p => p.FullPath)) : null
                    });
                }
            }

            _context.SaveChanges();
            transaction.Commit();

            MessageBox.Show("Отчёт сохранён успешно. Окно остаётся открытым для дальнейшего редактирования.");
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
        }
    }

    private void EmployeeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        LoadWorkSchedule();
    }

    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ButtonState == MouseButtonState.Pressed)
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
        WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void AddTemporaryHouseButton_Click(object sender, RoutedEventArgs e)
    {
        // TODO: Implement adding temporary house functionality
        MessageBox.Show("Функция добавления временного дома пока не реализована.");
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    internal virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class HouseSelectionItem : INotifyPropertyChanged
{
    private bool _isSelected;

    public ServiceAddress ServiceAddress { get; set; } = null!;
    public bool IsAssigned { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public Brush TextColor { get; set; } = Brushes.Black;

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            OnPropertyChanged(nameof(IsSelected));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class HouseWorkItem : INotifyPropertyChanged
{
    private string _houseComment = string.Empty;

    public ServiceAddress ServiceAddress { get; set; } = null!;
    public string HouseName => ServiceAddress.Address;
    public ObservableCollection<string> ScheduledWorks { get; set; } = new();
    public ObservableCollection<WorkTypeSelectionItem> ActualWorks { get; set; } = new();
    public ObservableCollection<PhotoFileItem> PhotoFiles { get; set; } = new();

    public string HouseComment
    {
        get => _houseComment;
        set
        {
            _houseComment = value;
            OnPropertyChanged(nameof(HouseComment));
        }
    }

    public string PhotoStatus => PhotoFiles.Any() ? $"Фото: {PhotoFiles.Count}" : "Фото не прикреплено";
    public Brush PhotoStatusColor => PhotoFiles.Any() ? Brushes.Green : Brushes.Gray;

    public event PropertyChangedEventHandler? PropertyChanged;

    internal virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class PhotoFileItem : INotifyPropertyChanged
{
    public string FileName { get; set; } = string.Empty;
    public string FullPath { get; set; } = string.Empty;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class WorkTypeSelectionItem : INotifyPropertyChanged
{
    private bool _isCompleted;
    private bool _isScheduled;

    public WorkType WorkType { get; set; } = null!;

    public bool IsCompleted
    {
        get => _isCompleted;
        set
        {
            _isCompleted = value;
            OnPropertyChanged(nameof(IsCompleted));
        }
    }

    public bool IsScheduled
    {
        get => _isScheduled;
        set
        {
            _isScheduled = value;
            OnPropertyChanged(nameof(IsScheduled));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
