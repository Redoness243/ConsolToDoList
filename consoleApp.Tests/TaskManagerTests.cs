using System.Collections.Immutable;
using Microsoft.VisualBasic;

namespace consoleApp.Tests;

public class TaskManagerTests
{

    [Fact]
    public void AddTask_ShouldReturnEmptyString()
    {
        var manager = new TaskManager();
        Assert.Empty(manager.GetAll());
    }

    [Fact]
    public void AddTask_ShouldReturnNull()
    {
        var manager = new TaskManager();
        Assert.Throws<ArgumentNullException>(() => manager.AddTask(null!));
    }

    [Fact]
    public void RemoveTask_ValidIndex_ShouldRemoveItemFromList()
    {
        var manager = new TaskManager();
        manager.AddTask("1");
        manager.AddTask("2");
        manager.RemoveTask(0);
        var tasks = manager.GetAll();
        Assert.Single(tasks);
        Assert.Equal("2", tasks[0].Title);
    }

    [Fact]
    public void CompleteTask_ShouldDeclareRightItem()
    {
        var manager = new TaskManager();
        manager.AddTask("1");
        manager.CompleteTask(0);
        var tasks = manager.GetAll();
        Assert.True(tasks[0].IsCompleted);
    }

    [Fact]
    public void GetAll_ShouldReturnEmptyListAtFirst()
    {
        var manager = new TaskManager();
        var tasks = manager.GetAll();
        Assert.Empty(manager.GetAll());
    }

    [Fact]
    public void AddThreeTask_ShouldHaveThreeTask()
    {
        var manager = new TaskManager();
        manager.AddTask("1");
        manager.AddTask("2");
        manager.AddTask("3");
        var tasks = manager.GetAll();
        Assert.Equal(3, manager.GetAll().Count);
    }

    [Fact]
    public void RemoveTask_ShouldWorkRightWithLargeIndex()
    {
        var manager = new TaskManager();
        manager.AddTask("Task");
        Assert.Throws<ArgumentOutOfRangeException>(() => manager.RemoveTask(99999));
    }

    [Fact]
    public void AddTask_ShouldAllowDuplicateTitles_CurrentBehaviorTaskManager()
    {
        var manager = new TaskManager();
        manager.AddTask("Aynı Görev");
        manager.AddTask("Aynı Görev");
        var tasks = manager.GetAll();
        Assert.Equal(2, manager.GetAll().Count);
        Assert.Equal(tasks[0].Title, tasks[1].Title);
    }

    [Fact]
    public void ComleteTask_CalledTwoSameIndex_ShouldKeepCompleted()
    {
        var manager = new TaskManager();
        manager.AddTask("Task");
        manager.CompleteTask(0);
        manager.CompleteTask(0);
        Assert.True(manager.GetAll()[0].IsCompleted);
    }

    [Fact]
    public void RemoveTask_MiddleTask_ShouldKeepOrder()
    {
        var manager = new TaskManager();
        manager.AddTask("1");
        manager.AddTask("2");
        manager.AddTask("3");
        manager.RemoveTask(1);
        var tasks = manager.GetAll();
        Assert.Equal(2, tasks.Count);
        Assert.Equal("1", tasks[0].Title);
        Assert.Equal("3", tasks[1].Title);
    }

    [Fact]
    public void AddTask_ShouldAutomaticallySetCreatedAt()
    {
        var manager = new TaskManager();
        var before = DateTime.Now;
        manager.AddTask("Task");
        var after = DateTime.Now;
        var task = manager.GetAll()[0];
        Assert.InRange(task.CreatedAt, before, after);
    }

    [Fact]
    public void EmptyStringAddTask_ShouldThrowException()
    {
        var manager = new TaskManager();
        Assert.Throws<ArgumentException>(() => manager.AddTask(" "));
    }

    [Fact]
    public void AddTask_Add50Task_ShouldAdd50()
    {
        var manager = new TaskManager();
        for (int i = 0; i < 50; i++)
        {
            manager.AddTask("Görev" + i);
        }
        Assert.Equal(50, manager.GetAll().Count);
    }
}