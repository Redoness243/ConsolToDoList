using System.ComponentModel;
using System.Xml;

public class TaskItem
{
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
    public enum Priority { Low, Medium, High }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public Priority PriorityOptions { get; set; }
    public enum Categories { Business, Personal, Educational }
    public Categories CategoryOptions { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public override string ToString()
    {
        return $"{PriorityOptions} - {Title} [{(IsCompleted ? "X" : " ")}] (Created: {CreatedAt:g})";
    }

    public TaskItem(string title)
    {
        Title = title;
        IsCompleted = false;
        CreatedAt = DateTime.Now;
    }
}

// Task model representing a single item.