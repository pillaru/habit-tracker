using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using HabitTracker.Persistence.Migrations;
using HabitTracker.Domain;
using HabitTracker.Persistence;

const string connectionString = "Data Source=habit_tracker_dev.db";

var serviceProvider = CreateServices();

using (var scope = serviceProvider.CreateScope())
{
    UpdateDatabase(scope.ServiceProvider);
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<HabitsRepository>(_ => new SqlHabitsRepository(connectionString));

var app = builder.Build();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Habits}/{action=Index}/{id?}");

app.Run();

static IServiceProvider CreateServices()
{
    return new ServiceCollection()
        .AddFluentMigratorCore()
        .ConfigureRunner(rb => rb
                .AddSQLite()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(AddHabitTable).Assembly).For.Migrations())
        .AddLogging(lb => lb.AddFluentMigratorConsole())
        .BuildServiceProvider(false);
}

static void UpdateDatabase(IServiceProvider serviceProvider)
{
    var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();
}
