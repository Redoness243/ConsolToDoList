using System.Formats.Asn1;
using Microsoft.VisualBasic;
using Xunit;

namespace consoleApp.Tests;

public class TaskListTests
{
    [Fact]
    public void AddTask_ShouldIncreaseCount()
    {
        var tasks = new List<TaskItem>();
        int initialCount = tasks.Count;
        tasks.Add(new TaskItem("Spora git"));
        Assert.Equal(initialCount + 1, tasks.Count);
    }

    [Fact]
    public void AddTask_ShouldAllowDuplicateTitles_CurrentBehavior()
    {
        var tasks = new List<TaskItem>();
        tasks.Add(new TaskItem("Aynı Görev"));
        tasks.Add(new TaskItem("Aynı Görev"));
        Assert.Equal(2, tasks.Count);
        Assert.Equal(tasks[0].Title, tasks[1].Title);
    }
}