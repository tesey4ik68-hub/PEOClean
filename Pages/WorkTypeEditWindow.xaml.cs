using PEOcleanWPFApp.Data;
using PEOcleanWPFApp.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PEOcleanWPFApp.Pages;

/// <summary>
/// Логика взаимодействия для WorkTypeEditWindow.xaml
/// </summary>
public partial class WorkTypeEditWindow : Window
{
    private ApplicationDbContext _context;
    private WorkType _workType;
    private bool _isNew;

    public WorkTypeEditWindow(WorkType? workType = null)
    {
        InitializeComponent();
        _context = App.GetDbContext();

        _isNew = workType == null;
        _workType = workType ?? new WorkType();

        LoadData();
    }

    private void LoadData()
    {
        NameTextBox.Text = _workType.Name;
        CodeTextBox.Text = _workType.Code;
        DescriptionTextBox.Text = _workType.Description;
        RequiresPhotoCheckBox.IsChecked = _workType.RequiresPhoto;

        // Set UnitOfMeasure
        UnitOfMeasureComboBox.SelectedIndex = (int)_workType.UnitOfMeasure;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameTextBox.Text))
        {
            MessageBox.Show("Пожалуйста, введите название вида работ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (string.IsNullOrWhiteSpace(CodeTextBox.Text))
        {
            MessageBox.Show("Пожалуйста, введите код вида работ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        _workType.Name = NameTextBox.Text.Trim();
        _workType.Code = CodeTextBox.Text.Trim().ToUpper();
        _workType.Description = DescriptionTextBox.Text.Trim();
        _workType.RequiresPhoto = RequiresPhotoCheckBox.IsChecked ?? false;
        _workType.UnitOfMeasure = (UnitOfMeasure)UnitOfMeasureComboBox.SelectedIndex;

        try
        {
            if (_isNew)
            {
                _context.WorkTypes.Add(_workType);
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
