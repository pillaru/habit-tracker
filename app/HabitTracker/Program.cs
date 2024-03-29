using FluentMigrator.Runner;
using HabitTracker.Persistence.Migrations;
using HabitTracker.Domain;
using HabitTracker.Persistence;

const string connectionString = "Data Source=habit_tracker_dev.db";

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
            .AddSQLite()
            .WithGlobalConnectionString(connectionString)
            .ScanIn(typeof(AddHabitTable).Assembly).For.Migrations())
    .AddLogging(lb => lb.AddFluentMigratorConsole());

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<HabitsRepository>(_ => new SqlHabitsRepository(connectionString));

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    UpdateDatabase(scope.ServiceProvider);
}

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Habits}/{action=Index}/{id?}");

app.Run();

static void UpdateDatabase(IServiceProvider serviceProvider)
{
    IMigrationRunner runner = serviceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();
}
