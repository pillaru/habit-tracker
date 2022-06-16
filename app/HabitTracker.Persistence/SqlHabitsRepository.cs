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

    public async Task<IEnumerable<Habit>> GetAll()
    {
        using (var connection = new SqliteConnection(this.connectionString))
        {
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText =
                @"SELECT Id, Title, CreatedAt, ModifiedAt, Description
                FROM Habit";
            var reader = await command.ExecuteReaderAsync();

            var habits = new List<Habit>();
            while (reader.Read())
            {
                var habit = await new HabitReconstitutionFactory
                    ().Create(reader);
                habits.Add(habit);
            }
            reader.Close();
            return habits;
        }
    }

    public async Task Save(Habit newHabit)
    {
        using (var connection = new SqliteConnection(this.connectionString))
        {
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText =
                @"INSERT INTO Habit(Id, Title, Description, CreatedAt, ModifiedAt)
                VALUES($id, $title, $description, $createdAt, $modifiedAt)";
            command.Parameters.AddWithValue("$id", newHabit.Id);
            command.Parameters.AddWithValue("$title", newHabit.Title);
            if (newHabit.Description != null)
            {
                command.Parameters.AddWithValue("$description", newHabit.Description);
            }
            else
            {
                command.Parameters.AddWithValue("$description", DBNull.Value);
            }
            command.Parameters.AddWithValue("$createdAt", newHabit.CreatedAt);
            command.Parameters.AddWithValue("modifiedAt", newHabit.ModifiedAt);

            await command.ExecuteNonQueryAsync();
        }
    }
}

internal class HabitReconstitutionFactory
{
    public async Task<Habit> Create(DbDataReader reader)
    {
        var id = await reader.GetFieldValueAsync<Guid>(0);
        var createdAt = await reader.GetFieldValueAsync<DateTimeOffset>(2);
        var modifiedAt = await reader.GetFieldValueAsync<DateTimeOffset>(3);
        var title = await reader.GetFieldValueAsync<string>(1);
        var isDescriptionNull = await reader.IsDBNullAsync(4);
        var description = isDescriptionNull
            ? null
            : await reader.GetFieldValueAsync<string>(4);
        return new Habit(id, title, createdAt)
        {
            ModifiedAt = modifiedAt
        };
    }
}

