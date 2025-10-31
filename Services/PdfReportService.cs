using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using Microsoft.EntityFrameworkCore;
using PEOcleanWPFApp.Data;
using PEOcleanWPFApp.Models;

namespace PEOcleanWPFApp.Services;

public class PdfReportService
{
    private readonly ApplicationDbContext _context;
    private readonly PaymentCalculationService _paymentService;

    public PdfReportService(ApplicationDbContext context, PaymentCalculationService paymentService)
    {
        _context = context;
        _paymentService = paymentService;
    }

    /// <summary>
    /// Generates a payment memo PDF
    /// </summary>
    public void GeneratePaymentMemo(int employeeId, DateTime startDate, DateTime endDate, string filePath)
    {
        var employee = _context.Employees.Find(employeeId);
        if (employee == null) throw new ArgumentException("Employee not found");

        var summary = _paymentService.GetEmployeeWorkSummary(employeeId, startDate, endDate);

        using var writer = new PdfWriter(filePath);
        using var pdf = new PdfDocument(writer);
        var document = new Document(pdf);

        // Title
        var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        var title = new Paragraph("СЛУЖЕБНАЯ ЗАПИСКА")
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(16)
            .SetFont(boldFont);
        document.Add(title);

        document.Add(new Paragraph($"На выплату вознаграждения {employee.FullName}")
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(14)
            .SetMarginBottom(20));

        // Period
        document.Add(new Paragraph($"Период: {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}")
            .SetMarginBottom(15));

        // Work summary
        var table = new Table(UnitValue.CreatePercentArray(new float[] { 3, 2, 2, 2 }))
            .UseAllAvailableWidth();

        table.AddHeaderCell("Показатель");
        table.AddHeaderCell("Количество");
        table.AddHeaderCell("Ед. изм.");
        table.AddHeaderCell("Примечание");

        table.AddCell("Отработано дней");
        table.AddCell(summary.TotalWorkedDays.ToString());
        table.AddCell("дней");
        table.AddCell("");

        table.AddCell("Подтверждено с фото");
        table.AddCell(summary.DaysWithPhoto.ToString());
        table.AddCell("дней");
        table.AddCell("");

        table.AddCell("Сумма к выплате");
        table.AddCell($"{summary.TotalPayment:N2}");
        table.AddCell("руб.");
        table.AddCell("");

        document.Add(table);
        var total = new Paragraph($"Итого к выплате: {summary.TotalPayment:N2} рублей")
            .SetMarginTop(20)
            .SetFont(boldFont);
        document.Add(total);

        document.Close();
    }

    /// <summary>
    /// Generates an act of completed works PDF
    /// </summary>
    public void GenerateWorkCompletionAct(int employeeId, DateTime startDate, DateTime endDate, string filePath)
    {
        var employee = _context.Employees.Find(employeeId);
        if (employee == null) throw new ArgumentException("Employee not found");

        var workReports = _context.WorkReports
            .Where(wr => wr.EmployeeId == employeeId &&
                        wr.Date >= startDate &&
                        wr.Date <= endDate &&
                        wr.IsCompleted)
            .Include(wr => wr.ServiceAddress)
            .Include(wr => wr.WorkType)
            .OrderBy(wr => wr.Date)
            .ToList();

        using var writer = new PdfWriter(filePath);
        using var pdf = new PdfDocument(writer);
        var document = new Document(pdf);

        // Title
        var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        var title = new Paragraph("АКТ ВЫПОЛНЕННЫХ РАБОТ")
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(16)
            .SetFont(boldFont);
        document.Add(title);

        document.Add(new Paragraph($"Исполнитель: {employee.FullName}")
            .SetMarginBottom(10));

        document.Add(new Paragraph($"Период: {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}")
            .SetMarginBottom(20));

        // Work details table
        var table = new Table(UnitValue.CreatePercentArray(new float[] { 2, 3, 3, 2 }))
            .UseAllAvailableWidth();

        table.AddHeaderCell("Дата");
        table.AddHeaderCell("Адрес");
        table.AddHeaderCell("Вид работы");
        table.AddHeaderCell("Фото");

        foreach (var report in workReports)
        {
            table.AddCell(report.Date.ToString("dd.MM.yyyy"));
            table.AddCell(report.ServiceAddress.Address);
            table.AddCell(report.WorkType.Name);
            table.AddCell(string.IsNullOrEmpty(report.PhotoPath) ? "Нет" : "Есть");
        }

        document.Add(table);

        // Summary
        var summary = new Paragraph($"Всего выполнено работ: {workReports.Count}")
            .SetMarginTop(20)
            .SetFont(boldFont);
        document.Add(summary);

        document.Close();
    }

    /// <summary>
    /// Generates monthly attendance report PDF
    /// </summary>
    public void GenerateMonthlyAttendanceReport(int year, int month, string filePath)
    {
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        var employees = _context.Employees.ToList();

        using var writer = new PdfWriter(filePath);
        using var pdf = new PdfDocument(writer);
        var document = new Document(pdf);

        // Title
        var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        var title = new Paragraph($"ТАБЕЛЬ УЧЁТА РАБОТЫ ЗА {month:00}.{year}")
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(16)
            .SetFont(boldFont);
        document.Add(title);
        document.Add(new Paragraph("").SetMarginBottom(20));

        // Create table with days as columns
        var daysInMonth = DateTime.DaysInMonth(year, month);
        var columnWidths = new float[daysInMonth + 1];
        columnWidths[0] = 3; // Employee name column
        for (int i = 1; i <= daysInMonth; i++) columnWidths[i] = 1;

        var table = new Table(UnitValue.CreatePercentArray(columnWidths))
            .UseAllAvailableWidth();

        // Header row
        table.AddHeaderCell("Сотрудник");
        for (int day = 1; day <= daysInMonth; day++)
        {
            table.AddHeaderCell(day.ToString());
        }

        // Data rows
        foreach (var employee in employees)
        {
            table.AddCell(employee.FullName);

            for (int day = 1; day <= daysInMonth; day++)
            {
                var date = new DateTime(year, month, day);
                var attendance = _context.AttendanceRecords
                    .FirstOrDefault(ar => ar.EmployeeId == employee.Id && ar.Date.Date == date);

                string status = attendance == null ? "-" :
                    attendance.Status == AbsenceType.Worked ?
                        (attendance.HasPhoto ? "✅" : "⚪") :
                    attendance.Status == AbsenceType.NotWorked ? "❌" : "🔄";

                table.AddCell(status);
            }
        }

        document.Add(table);
        document.Close();
    }
}
