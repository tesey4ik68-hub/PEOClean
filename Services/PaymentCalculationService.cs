using PEOcleanWPFApp.Data;
using PEOcleanWPFApp.Models;

namespace PEOcleanWPFApp.Services;

public class PaymentCalculationService
{
    private readonly ApplicationDbContext _context;

    public PaymentCalculationService(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Calculates the daily rate for a house (monthly rate / 27)
    /// </summary>
    public decimal CalculateDailyRate(ServiceAddress house)
    {
        return house.JanitorRate / 27;
    }

    /// <summary>
    /// Calculates total payment for an employee for a given period
    /// </summary>
    public decimal CalculateEmployeePayment(int employeeId, DateTime startDate, DateTime endDate)
    {
        var confirmedWorkDays = _context.WorkReports
            .Where(wr => wr.EmployeeId == employeeId &&
                        wr.Date >= startDate &&
                        wr.Date <= endDate &&
                        wr.IsCompleted)
            .GroupBy(wr => new { wr.Date, wr.ServiceAddressId })
            .Select(g => new
            {
                Date = g.Key.Date,
                ServiceAddressId = g.Key.ServiceAddressId,
                DailyRate = _context.ServiceAddresses
                    .Where(sa => sa.Id == g.Key.ServiceAddressId)
                    .Select(sa => sa.JanitorRate / 27)
                    .FirstOrDefault()
            })
            .ToList();

        return confirmedWorkDays.Sum(day => day.DailyRate);
    }

    /// <summary>
    /// Gets work summary for an employee in a period
    /// </summary>
    public EmployeeWorkSummary GetEmployeeWorkSummary(int employeeId, DateTime startDate, DateTime endDate)
    {
        var workReports = _context.WorkReports
            .Where(wr => wr.EmployeeId == employeeId &&
                        wr.Date >= startDate &&
                        wr.Date <= endDate)
            .ToList();

        var attendanceRecords = _context.AttendanceRecords
            .Where(ar => ar.EmployeeId == employeeId &&
                        ar.Date >= startDate &&
                        ar.Date <= endDate)
            .ToList();

        var confirmedDays = workReports
            .Where(wr => wr.IsCompleted)
            .GroupBy(wr => wr.Date)
            .Count();

        var daysWithPhoto = attendanceRecords.Count(ar => ar.HasPhoto && ar.Status == AbsenceType.Worked);
        var totalWorkedDays = attendanceRecords.Count(ar => ar.Status == AbsenceType.Worked);

        return new EmployeeWorkSummary
        {
            TotalWorkedDays = totalWorkedDays,
            ConfirmedDays = confirmedDays,
            DaysWithPhoto = daysWithPhoto,
            TotalPayment = CalculateEmployeePayment(employeeId, startDate, endDate)
        };
    }

    /// <summary>
    /// Calculates house completion percentage for a month
    /// </summary>
    public double CalculateHouseCompletionPercentage(int houseId, int year, int month)
    {
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        var completedDays = _context.WorkReports
            .Count(wr => wr.ServiceAddressId == houseId &&
                        wr.Date >= startDate &&
                        wr.Date <= endDate &&
                        wr.IsCompleted);

        // Assuming 27 working days per month
        return Math.Min((double)completedDays / 27 * 100, 100);
    }
}

public class EmployeeWorkSummary
{
    public int TotalWorkedDays { get; set; }
    public int ConfirmedDays { get; set; }
    public int DaysWithPhoto { get; set; }
    public decimal TotalPayment { get; set; }
}
