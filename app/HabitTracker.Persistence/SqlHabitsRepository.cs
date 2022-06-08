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

