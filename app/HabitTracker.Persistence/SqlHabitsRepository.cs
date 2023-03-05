using System.Data.Common;
using Microsoft.Data.Sqlite;
using HabitTracker.Domain;

namespace HabitTracker.Persistence;

public class SqlHabitsRepository : HabitsRepository
{
    private readonly string connectionString;

    public SqlHabitsRepository(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public async Task Delete(Guid id)
    {
        using var connection = new SqliteConnection(this.connectionString);
        await connection.OpenAsync().ConfigureAwait(false);

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Habit WHERE Id = $id";
        SqliteParameter _ = command.Parameters.AddWithValue("$id", id);
        int unused = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
    }

    public async Task<IEnumerable<Habit>> GetAll()
    {
        using var connection = new SqliteConnection(this.connectionString);
        await connection.OpenAsync().ConfigureAwait(false);

        SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            @"SELECT Id, Title, CreatedAt, ModifiedAt, Description
                FROM Habit";
        SqliteDataReader reader = await command.ExecuteReaderAsync()
            .ConfigureAwait(false);

        var habits = new List<Habit>();
        while (await reader.ReadAsync().ConfigureAwait(false))
        {
            Habit habit = await HabitReconstitutionFactory.Create(reader)
                            .ConfigureAwait(false);
            habits.Add(habit);
        }
        await reader.CloseAsync().ConfigureAwait(false);
        return habits;
    }

    public async Task<Habit?> GetById(Guid id)
    {
        Habit? habit = null;
        using var connection = new SqliteConnection(this.connectionString);
        await connection.OpenAsync().ConfigureAwait(false);

        SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            @"SELECT Id, Title, CreatedAt, ModifiedAt, Description
                FROM Habit WHERE Id = $id";
        SqliteParameter _ = command.Parameters.AddWithValue("$id", id);
        SqliteDataReader reader = await command.ExecuteReaderAsync()
            .ConfigureAwait(false);

        if (await reader.ReadAsync().ConfigureAwait(false))
        {
            habit = await HabitReconstitutionFactory.Create(reader)
                                .ConfigureAwait(false);
        }
        await reader.CloseAsync().ConfigureAwait(false);
        return habit;
    }

    public async Task Save(Habit newHabit)
    {
        ArgumentNullException.ThrowIfNull(newHabit);
        using var connection = new SqliteConnection(this.connectionString);
        await connection.OpenAsync().ConfigureAwait(false);

        SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            @"INSERT INTO Habit(Id, Title, Description, CreatedAt, ModifiedAt)
                VALUES($id, $title, $description, $createdAt, $modifiedAt)";
        SqliteParameter _ = command.Parameters.AddWithValue("$id", newHabit.Id);
        SqliteParameter unused = command.Parameters.AddWithValue("$title", newHabit.Title);
        SqliteParameter descriptionParam = newHabit.Description != null
            ? command.Parameters.AddWithValue("$description", newHabit.Description)
            : command.Parameters.AddWithValue("$description", DBNull.Value);
        SqliteParameter createdAtParam = command.Parameters.AddWithValue("$createdAt", newHabit.CreatedAt);
        SqliteParameter modifiedAtParam = command.Parameters.AddWithValue("modifiedAt", newHabit.ModifiedAt);

        int affected = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
    }
}

internal static class HabitReconstitutionFactory
{
    public static async Task<Habit> Create(DbDataReader reader)
    {
        Guid id = await reader.GetFieldValueAsync<Guid>(0).ConfigureAwait(false);
        DateTimeOffset createdAt = await reader.GetFieldValueAsync<DateTimeOffset>(2)
            .ConfigureAwait(false);
        DateTimeOffset modifiedAt = await reader.GetFieldValueAsync<DateTimeOffset>(3)
            .ConfigureAwait(false);
        string title = await reader.GetFieldValueAsync<string>(1).ConfigureAwait(false);
        bool isDescriptionNull = await reader.IsDBNullAsync(4).ConfigureAwait(false);
        string? description = isDescriptionNull
            ? null
            : await reader.GetFieldValueAsync<string>(4).ConfigureAwait(false);
        return new Habit(id, title, createdAt)
        {
            Description = description,
            ModifiedAt = modifiedAt
        };
    }
}

