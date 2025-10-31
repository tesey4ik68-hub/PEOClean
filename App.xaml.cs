using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PEOcleanWPFApp.Data;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace PEOcleanWPFApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private ServiceProvider _serviceProvider;

    public App()
    {
        var services = new ServiceCollection();

        // Configure DbContext with database file in the application root directory
        var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "janitor.db");
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlite($"Data Source={dbPath}");
        });

        _serviceProvider = services.BuildServiceProvider();

        // Ensure database is created and migrated
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
            
            // Initialize database with seed data
            DatabaseInitializer.SeedDatabase(dbContext);
        }
    }

    public static ApplicationDbContext GetDbContext()
    {
        var app = (App)Current;
        return app._serviceProvider.GetRequiredService<ApplicationDbContext>();
    }
}