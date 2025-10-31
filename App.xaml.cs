using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PEOcleanWPFApp.Data;
using System.Configuration;
using System.Data;
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

        // Configure DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlite("Data Source=janitor.db");
        });

        _serviceProvider = services.BuildServiceProvider();

        // Ensure database is created and migrated
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
        }
    }

    public static ApplicationDbContext GetDbContext()
    {
        var app = (App)Current;
        return app._serviceProvider.GetRequiredService<ApplicationDbContext>();
    }
}

