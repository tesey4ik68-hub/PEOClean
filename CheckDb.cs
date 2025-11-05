using Microsoft.EntityFrameworkCore;
using PEOcleanWPFApp.Data;
using PEOcleanWPFApp.Models;

class Program
{
    static void Main()
    {
        using var context = new ApplicationDbContext();

        Console.WriteLine("Виды работ в базе данных:");
        var workTypes = context.WorkTypes.OrderBy(wt => wt.Name).ToList();

        foreach (var wt in workTypes)
        {
            Console.WriteLine($"{wt.Name}: {wt.Periodicity}");
        }
    }
}
