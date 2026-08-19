using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Xunit;
using consoleApp;

namespace consoleApp.Tests;

public class SqlTaskRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly SqlTaskRepository _repository;

    public SqlTaskRepositoryTests()
    {
        var dbName = Guid.NewGuid().ToString();
        var connectionString = $"Data Source=file:{dbName}?mode=memory&cache=shared";

        _connection = new SqliteConnection(connectionString);
        _connection.Open();
        _repository = new SqlTaskRepository(connectionString);
    }

    [Fact]
    public void Load_WhenDatabaseIsEmpty_ShouldReturnEmptyList()
    {
        var tasks = _repository.Load();

        Assert.Empty(tasks);
    }

    [Fact]
    public void SaveAndLoad_TwoTasks_ShouldReturnTwoTasks()
    {
        var tasksToSave = new List<TaskItem>
        {
            new TaskItem("Görev 1"),
            new TaskItem("Görev 2")
        };

        _repository.Save(tasksToSave);
        var loadedTasks = _repository.Load();
        Assert.Equal(2, loadedTasks.Count);
    }

    [Fact]
    public void SaveAndLoad_ShouldPreserveTaskProperties()
    {
        var originalTask = new TaskItem("Kritik İş")
        {
            IsCompleted = true,
            PriorityOptions = TaskItem.Priority.High,
            CategoryOptions = TaskItem.Categories.Business
        };

        _repository.Save(new List<TaskItem> { originalTask });
        var loadedTasks = _repository.Load();

        Assert.Single(loadedTasks);
        Assert.Equal("Kritik İş", loadedTasks[0].Title);
        Assert.True(loadedTasks[0].IsCompleted);
        Assert.Equal(TaskItem.Priority.High, loadedTasks[0].PriorityOptions);
        Assert.Equal(TaskItem.Categories.Business, loadedTasks[0].CategoryOptions);
    }

    [Fact]
    public void Save_ShouldOverwriteExistingRecords()
    {
        var initialTasks = new List<TaskItem> { new TaskItem("Task 1"), new TaskItem("Task 2") };
        _repository.Save(initialTasks);
        var newTasks = new List<TaskItem> { new TaskItem("New Task") };
        _repository.Save(newTasks);
        var loadedTasks = _repository.Load();
        Assert.Single(loadedTasks);
        Assert.Equal("New Task", loadedTasks[0].Title);
    }

    [Fact]
    public void Load_WhenPriorityIsInvalidString_ShouldThrowArgumentException()
    {
        _repository.Save(new List<TaskItem>());
        using var command = _connection.CreateCommand();
        command.CommandText = @"
        INSERT INTO Tasks (Title, IsCompleted, Priority, CreatedAt, CategoryId)
        VALUES ('Wrong Task', 0, 'InvalidPriority', '2026-01-01 10:00:00', 1)";
        command.ExecuteNonQuery();
        Assert.Throws<ArgumentException>(() => _repository.Load());
    }

    [Fact]
    public void Load_WhenCategoryIdIsOutOfRange_ShouldCastWithoutValidation_CurrentBehavior()
    {
        _repository.Save(new List<TaskItem>());
        using var command = _connection.CreateCommand();
        command.CommandText = @"
        INSERT INTO Tasks (Title, IsCompleted, Priority, CreatedAt, CategoryId)
        VALUES ('Out of Range Category', 0, 'Low', '2026-01-01 10:00:00', 999)";
        command.ExecuteNonQuery();
        var tasks = _repository.Load();
        Assert.Single(tasks);
        Assert.Equal((TaskItem.Categories)999, tasks[0].CategoryOptions);
    }

    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
    }
}