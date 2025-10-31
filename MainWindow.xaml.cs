using PEOcleanWPFApp.Data;
using PEOcleanWPFApp.Models;
using PEOcleanWPFApp.Pages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace PEOcleanWPFApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    private ApplicationDbContext _context;
    private DateTime _selectedDate;

    public DateTime SelectedDate
    {
        get => _selectedDate;
        set
        {
            _selectedDate = value;
            OnPropertyChanged(nameof(SelectedDate));
            OnPropertyChanged(nameof(WeeklyAttendanceTitle));
            LoadData();
        }
    }

    public string WeeklyAttendanceTitle
    {
        get
        {
            var startOfWeek = SelectedDate.AddDays(-(int)SelectedDate.DayOfWeek + 1);
            var endOfWeek = startOfWeek.AddDays(6);
            return $"Табель за неделю {startOfWeek:dd.MM.yyyy} - {endOfWeek:dd.MM.yyyy}";
        }
    }

    public ObservableCollection<WeeklyAttendanceItem> WeeklyAttendanceItems { get; set; }
    public ObservableCollection<AlertItem> AlertItems { get; set; }
    public ObservableCollection<HouseProgressItem> HouseProgressItems { get; set; }
    public ObservableCollection<EmployeeRankingItem> EmployeeRankingItems { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        _context = App.GetDbContext();

        WeeklyAttendanceItems = new ObservableCollection<WeeklyAttendanceItem>();
        AlertItems = new ObservableCollection<AlertItem>();
        HouseProgressItems = new ObservableCollection<HouseProgressItem>();
        EmployeeRankingItems = new ObservableCollection<EmployeeRankingItem>();

        DataContext = this;
        SelectedDate = DateTime.Today;
    }

    private void LoadData()
    {
        LoadWeeklyAttendance();
        LoadAlerts();
        LoadHouseProgress();
        LoadEmployeeRankings();
    }

    private void LoadWeeklyAttendance()
    {
        WeeklyAttendanceItems.Clear();
        var startOfWeek = SelectedDate.AddDays(-(int)SelectedDate.DayOfWeek + 1);
        var employees = _context.Employees.OrderBy(e => e.FullName).ToList();

        foreach (var employee in employees)
        {
            var item = new WeeklyAttendanceItem { EmployeeName = employee.FullName };
            for (int i = 0; i < 7; i++)
            {
                var date = startOfWeek.AddDays(i);
                var attendance = _context.AttendanceRecords
                    .FirstOrDefault(ar => ar.EmployeeId == employee.Id && ar.Date.Date == date.Date);

                string status = attendance == null ? "-" :
                    attendance.Status == AbsenceType.Worked ? (attendance.HasPhoto ? "✅" : "⚪") :
                    attendance.Status == AbsenceType.NotWorked ? "❌" : "🔄";

                switch (i)
                {
                    case 0: item.MondayStatus = status; break;
                    case 1: item.TuesdayStatus = status; break;
                    case 2: item.WednesdayStatus = status; break;
                    case 3: item.ThursdayStatus = status; break;
                    case 4: item.FridayStatus = status; break;
                    case 5: item.SaturdayStatus = status; break;
                    case 6: item.SundayStatus = status; break;
                }
            }
            WeeklyAttendanceItems.Add(item);
        }
    }

    private void LoadAlerts()
    {
        AlertItems.Clear();
        // Add sample alerts - in real app, this would be calculated based on data
        AlertItems.Add(new AlertItem { Icon = "❗", Message = "Дом Ленина, 10 — не проведена влажная уборка" });
        AlertItems.Add(new AlertItem { Icon = "⚠️", Message = "Дом Гагарина, 5 — 3 дня без отчёта" });
        AlertItems.Add(new AlertItem { Icon = "ℹ️", Message = "Сотрудник Иванов — 5 дней без фото" });
    }

    private void LoadHouseProgress()
    {
        HouseProgressItems.Clear();
        var houses = _context.ServiceAddresses.ToList();
        var currentMonth = SelectedDate.Month;
        var currentYear = SelectedDate.Year;

        foreach (var house in houses)
        {
            var workDays = _context.WorkReports
                .Count(wr => wr.ServiceAddressId == house.Id &&
                            wr.Date.Month == currentMonth &&
                            wr.Date.Year == currentYear &&
                            wr.IsCompleted);

            var progress = Math.Min(workDays / 27.0 * 100, 100);
            HouseProgressItems.Add(new HouseProgressItem
            {
                Address = house.Address,
                ProgressPercentage = progress,
                ProgressText = $"{workDays}/27 дней"
            });
        }
    }

    private void LoadEmployeeRankings()
    {
        EmployeeRankingItems.Clear();
        var employees = _context.Employees.ToList();
        var currentMonth = SelectedDate.Month;
        var currentYear = SelectedDate.Year;

        var rankings = employees.Select(e => new
        {
            Employee = e,
            WorkDays = _context.WorkReports
                .Count(wr => wr.EmployeeId == e.Id &&
                            wr.Date.Month == currentMonth &&
                            wr.Date.Year == currentYear &&
                            wr.IsCompleted)
        })
        .OrderByDescending(x => x.WorkDays)
        .Take(10)
        .ToList();

        foreach (var ranking in rankings)
        {
            EmployeeRankingItems.Add(new EmployeeRankingItem
            {
                DisplayText = $"{ranking.Employee.FullName} — {ranking.WorkDays} домо-дней"
            });
        }
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
        WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void TodayButton_Click(object sender, RoutedEventArgs e)
    {
        SelectedDate = DateTime.Today;
    }

    private void NewWorkReportButton_Click(object sender, RoutedEventArgs e)
    {
        var workReportWindow = new WorkReportWindow();
        workReportWindow.ShowDialog();
        LoadData(); // Refresh data after closing the window
    }

    private void WeeklyAttendanceGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        var dataGrid = sender as DataGrid;
        if (dataGrid?.SelectedItem is WeeklyAttendanceItem selectedItem)
        {
            var employee = _context.Employees.FirstOrDefault(emp => emp.FullName == selectedItem.EmployeeName);
            if (employee != null)
            {
                var workReportWindow = new WorkReportWindow(employee, SelectedDate);
                workReportWindow.ShowDialog();
                LoadData(); // Refresh data after closing the window
            }
        }
    }

    private void DictionariesButton_Click(object sender, RoutedEventArgs e)
    {
        var dictionaryWindow = new DictionaryWindow();
        dictionaryWindow.ShowDialog();
        LoadData(); // Refresh data after closing the window
    }

    private void GenerateReportButton_Click(object sender, RoutedEventArgs e)
    {
        // TODO: Open report generation window
        MessageBox.Show("Функция генерации отчётов будет реализована");
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class WeeklyAttendanceItem
{
    public string EmployeeName { get; set; } = string.Empty;
    public string MondayStatus { get; set; } = "-";
    public string TuesdayStatus { get; set; } = "-";
    public string WednesdayStatus { get; set; } = "-";
    public string ThursdayStatus { get; set; } = "-";
    public string FridayStatus { get; set; } = "-";
    public string SaturdayStatus { get; set; } = "-";
    public string SundayStatus { get; set; } = "-";
}

public class AlertItem
{
    public string Icon { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class HouseProgressItem
{
    public string Address { get; set; } = string.Empty;
    public double ProgressPercentage { get; set; }
    public string ProgressText { get; set; } = string.Empty;
}

public class EmployeeRankingItem
{
    public string DisplayText { get; set; } = string.Empty;
}
