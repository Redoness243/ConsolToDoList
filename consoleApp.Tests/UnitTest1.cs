using Xunit;

namespace consoleApp.Tests;

public class UnitTest1
{
    [Fact]
    public void NewTask_ShouldNotBeCompleted()
    {
        var task = new TaskItem("Test görevi");
        Assert.False(task.IsCompleted);
    }

    [Fact]
    public void NewTask_ShouldHaveCorrectTitle()
    {
        var task = new TaskItem("Süt al");
        Assert.Equal("Süt al", task.Title);
    }

    [Fact]
    public void MarkAsCompleted()
    {
        var task = new TaskItem("Raporu Hazırla");
        task.IsCompleted = true;
        Assert.True(task.IsCompleted);
    }

    [Fact]
    public void List_ShouldHaveOneItem_AfterAdding()
    {
        var tasks = new List<TaskItem>();
        tasks.Add(new TaskItem("Yeni görev"));
        Assert.Single(tasks);
    }

    [Fact]
    public void RemoveAt_ShouldThrow_WhenIndexOutOfRange()
    {
        var tasks = new List<TaskItem> { new TaskItem("Görev 1") };
        Assert.Throws<ArgumentOutOfRangeException>(() => tasks.RemoveAt(5));
    }
    
    [Fact]
    public void RemoveAt_ShouldThrow_WhenIndexIsNegative()
    {
        var tasks = new List<TaskItem> { new TaskItem("Görev 1") };
        Assert.Throws<ArgumentOutOfRangeException>(() => tasks.RemoveAt(-1));
    }

    [Fact]
    public void RemoveAt_ShouldThrow_WhenListIsEmpty()
    {
        var tasks = new List<TaskItem>();
        Assert.Throws<ArgumentOutOfRangeException>(() => tasks.RemoveAt(0));
    }
}