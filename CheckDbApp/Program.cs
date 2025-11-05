using Microsoft.EntityFrameworkCore;
using PEOcleanWPFApp.Data;
using PEOcleanWPFApp.Models;
using System.IO;

class Program
{
    static void Main()
    {
        var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "janitor.db");
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite($"Data Source={dbPath}")
            .Options;

        using var context = new ApplicationDbContext(options);

        Console.WriteLine("Виды работ в базе данных:");
        var workTypes = context.WorkTypes.OrderBy(wt => wt.Name).ToList();

        foreach (var wt in workTypes)
        {
            Console.WriteLine($"{wt.Name}: {wt.Periodicity}");
        }
    }
}
