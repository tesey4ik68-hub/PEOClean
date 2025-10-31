using PEOcleanWPFApp.Models;
using System.Text;

namespace PEOcleanWPFApp.Data;

public class DatabaseInitializer
{
    public static void SeedDatabase(ApplicationDbContext context)
    {
        // Проверим, есть ли уже данные в базе
        if (context.Employees.Any() || context.ServiceAddresses.Any())
        {
            // Создаем связи между уже существующими данными
            CreateEmployeeAssignmentsForExistingData(context);
            return;
        }
    }

    private static void CreateEmployeeAssignmentsForExistingData(ApplicationDbContext context)
    {
        // Проверим, есть ли уже назначения
        if (context.EmployeeAssignments.Any())
        {
            return; // Назначения уже созданы
        }

        var employees = context.Employees.ToList();
        var addresses = context.ServiceAddresses.ToList();
        var assignments = new List<EmployeeAssignment>();

        // Создаем назначения на основе записей в поле Notes сотрудников
        foreach (var employee in employees)
        {
            if (!string.IsNullOrEmpty(employee.Notes))
            {
                var lines = employee.Notes.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    var trimmedLine = line.Trim();
                    if (string.IsNullOrEmpty(trimmedLine)) continue;

                    // Определяем роль по суффиксу
                    EmployeeRole? role = null;
                    string addressText = trimmedLine;

                    if (trimmedLine.EndsWith("(д)"))
                    {
                        role = EmployeeRole.Janitor;
                        addressText = trimmedLine.Substring(0, trimmedLine.Length - 3).Trim();
                    }
                    else if (trimmedLine.EndsWith("(с.д.)"))
                    {
                        role = EmployeeRole.Cleaner;
                        addressText = trimmedLine.Substring(0, trimmedLine.Length - 6).Trim();
                    }
                    else if (trimmedLine.Contains("с. Лошица"))
                    {
                        // Предполагаем, что это дворник
                        role = EmployeeRole.Janitor;
                    }
                    else if (trimmedLine.Contains("переулок"))
                    {
                        // Предполагаем, что это уборщица
                        role = EmployeeRole.Cleaner;
                    }

                    // Ищем адрес
                    var address = addresses.FirstOrDefault(a => a.Address == addressText);
                    if (address != null && role.HasValue)
                    {
                        assignments.Add(new EmployeeAssignment
                        {
                            EmployeeId = employee.Id,
                            ServiceAddressId = address.Id,
                            Role = role.Value,
                            StartDate = DateTime.Today,
                            IsPrimary = true
                        });
                    }
                }
            }
        }

        context.EmployeeAssignments.AddRange(assignments);
        context.SaveChanges();
    }
}