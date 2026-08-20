using System.IO;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Net.Mail;

public class SqlTaskRepository : ITaskRepository
{
    private readonly string _connectionString;

    public SqlTaskRepository(string connectionString = "Data Source=tasks.db")
    {
        _connectionString = connectionString;
    }

    public List<TaskItem> Load()
    {
        using var connection = new SqliteConnection(_connectionString);
        List<TaskItem> taskList = new List<TaskItem>();
        connection.Open();

        var createCommand = connection.CreateCommand();
        createCommand.CommandText = @"
        CREATE TABLE IF NOT EXISTS Tasks (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Title TEXT NOT NULL,
        IsCompleted INTEGER NOT NULL,
        Priority TEXT NOT NULL,
        CreatedAt TEXT NOT NULL,
        CategoryId INTEGER NOT NULL
        )";
        createCommand.ExecuteNonQuery();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Tasks";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var task = new TaskItem(reader["Title"]?.ToString() ?? string.Empty)
            {
                IsCompleted = Convert.ToBoolean(reader["IsCompleted"]),
                PriorityOptions = Enum.Parse<TaskItem.Priority>(reader["Priority"].ToString() ?? string.Empty),
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                CategoryOptions = reader["CategoryId"] != DBNull.Value ? (TaskItem.Categories)Convert.ToInt32(reader["CategoryId"]) : TaskItem.Categories.Business
            };
            taskList.Add(task);
        }
        return taskList;
    }

    public void Save(List<TaskItem> tasks)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var createCommand = connection.CreateCommand();
        createCommand.CommandText = @"
        CREATE TABLE IF NOT EXISTS Tasks (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Title TEXT NOT NULL,
        IsCompleted INTEGER NOT NULL,
        Priority TEXT NOT NULL,
        CreatedAt TEXT NOT NULL,
        CategoryId INTEGER NOT NULL
        )";
        createCommand.ExecuteNonQuery();

        using var transaction = connection.BeginTransaction();
        var deleteCommand = connection.CreateCommand();
        deleteCommand.Transaction = transaction;
        deleteCommand.CommandText = "DELETE FROM Tasks";
        deleteCommand.ExecuteNonQuery();

        foreach (var task in tasks)
        {
            var insertCommand = connection.CreateCommand();
            insertCommand.Transaction = transaction;
            insertCommand.CommandText = @"
            INSERT INTO Tasks (Title, IsCompleted, Priority, CreatedAt, CategoryId)
            VALUES (@Title, @IsCompleted, @Priority, @CreatedAt, @CategoryId)";

            insertCommand.Parameters.AddWithValue("@Title", task.Title);
            insertCommand.Parameters.AddWithValue("@IsCompleted", task.IsCompleted ? 1 : 0);
            insertCommand.Parameters.AddWithValue("@Priority", task.PriorityOptions.ToString());
            insertCommand.Parameters.AddWithValue("@CreatedAt", task.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
            insertCommand.Parameters.AddWithValue("@CategoryId", (int)task.CategoryOptions);

            insertCommand.ExecuteNonQuery();
        }
        transaction.Commit();
    }
}