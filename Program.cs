using System.IO;
using Microsoft.Data.Sqlite;

partial class Program
{
    static void ViewTasks(List<TaskItem> tasks)
    {
        foreach (var t in tasks.Where(t => !t.IsCompleted))
        {
            Console.WriteLine(t.Title);
        }
        Console.WriteLine(tasks.Count(t => t.IsCompleted));

        Console.WriteLine("Current tasks:");
        foreach (var task in tasks)
        {
            Console.WriteLine(task);
        }
        if (tasks.Count == 0)
        {
            Console.WriteLine("No tasks available.");
        }
    }

    static void AddTask(List<TaskItem> tasks)
    {
        Console.WriteLine("Enter a task to add:");
        string? newTask = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(newTask))
        {
            Console.WriteLine("No task entered.");
            return;
        }

        TaskItem taskItem = new TaskItem(newTask);
        Console.WriteLine("Enter a priority: \n1.Low \n2.Medium \n3.High");
        string? priorityInput = Console.ReadLine();

        switch (priorityInput)
        {
            case "1":
                taskItem.PriorityOptions = TaskItem.Priority.Low;
                break;
            case "2":
                taskItem.PriorityOptions = TaskItem.Priority.Medium;
                break;
            case "3":
                taskItem.PriorityOptions = TaskItem.Priority.High;
                break;
            default:
                Console.WriteLine("Invalid option. Please select a valid option (1-3).");
                return;
        }

        Console.WriteLine("Select a category: \n1.Business \n2.Personal \n3.Educational");
        string? categoryInput = Console.ReadLine();
        switch (categoryInput)
        {
            case "1":
                taskItem.CategoryOptions = TaskItem.Categories.Business;
                break;
            case "2":
                taskItem.CategoryOptions = TaskItem.Categories.Personal;
                break;
            case "3":
                taskItem.CategoryOptions = TaskItem.Categories.Educational;
                break;
            default:
                Console.WriteLine("Invalid option. Please select a valid option (1-3).");
                return;
        }

        tasks.Add(taskItem);
        ITaskRepository repository = new SqlTaskRepository();
        repository.Save(tasks);
        Console.WriteLine($"Task '{newTask}' added, it is priority is: '{taskItem.PriorityOptions}' and it is category is: '{taskItem.CategoryOptions}.");
    }

    static void RemoveTask(List<TaskItem> tasks)
    {
        if (tasks.Count > 0)
        {
            Console.WriteLine("Enter the task number to remove:");
            for (int i = 0; i < tasks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {tasks[i].Title}");
            }
        }
        else
        {
            Console.WriteLine("No tasks available.");
            return;
        }

        string? taskNumberInput = Console.ReadLine();
        if (int.TryParse(taskNumberInput, out int taskNumber) && taskNumber > 0 && taskNumber <= tasks.Count)
        {
            TaskItem removedTask = tasks[taskNumber - 1];
            tasks.RemoveAt(taskNumber - 1);
            Console.WriteLine($"Task '{removedTask.Title}' removed.");
        }
        else
        {
            Console.WriteLine("Invalid task number.");
        }
        ITaskRepository repository = new SqlTaskRepository();
        repository.Save(tasks);
    }

    static void MarkTaskAsCompleted(List<TaskItem> tasks)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("No tasks available.");
            return;
        }
        Console.WriteLine("Enter the task number to mark as completed:");
        for (int i = 0; i < tasks.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {tasks[i].Title} [{(tasks[i].IsCompleted ? "X" : " ")}]");
        }
        string? taskNumberInput = Console.ReadLine();
        if (int.TryParse(taskNumberInput, out int taskNumber) && taskNumber > 0 && taskNumber <= tasks.Count)
        {
            tasks[taskNumber - 1].IsCompleted = true;
            Console.WriteLine($"Task '{tasks[taskNumber - 1].Title}' marked as completed.");
        }
        else
        {
            Console.WriteLine("Invalid task number.");
        }
        ITaskRepository repository = new SqlTaskRepository();
        repository.Save(tasks);
    }
    static void Sort_Filter(List<TaskItem> tasks)
    {
        Console.WriteLine("Choose a sort/filter type: \n1.Date \n2.High Priority");
        string? sort_filter = Console.ReadLine();
        switch (sort_filter)
        {
            case "1":
                foreach (var task in tasks.OrderBy(t => t.CreatedAt))
                {
                    Console.WriteLine(task);
                }
                break;
            case "2":
                foreach (var task in tasks.Where(t => t.PriorityOptions == TaskItem.Priority.High))
                    Console.WriteLine(task);
                break;
            default:
                Console.WriteLine("Invalid option. Please select a valid option (1/2).");
                break;
        }
    }

    static void EditTask(List<TaskItem> tasks)
    {
        if (tasks.Count > 0)
        {
            Console.WriteLine("Enter the task number to update:");
            for (int i = 0; i < tasks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {tasks[i].Title}");
            }
        }
        else
        {
            Console.WriteLine("No tasks available.");
            return;
        }

        string? taskNumberUpdate = Console.ReadLine();
        if (int.TryParse(taskNumberUpdate, out int taskNumber) && taskNumber > 0 && taskNumber <= tasks.Count)
        {
            TaskItem updatedTask = tasks[taskNumber - 1];
            Console.WriteLine($"Current title: {updatedTask.Title}");
            Console.WriteLine("Enter new value (leave empty to keep current):");
            string? newTitle = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newTitle))
            {
                updatedTask.Title = newTitle;
            }
            Console.WriteLine("Enter a priority: \n1.Low \n2.Medium \n3.High (leave empty to keep current)");
            string? priorityInput = Console.ReadLine();
            switch (priorityInput)
            {
                case "1":
                    updatedTask.PriorityOptions = TaskItem.Priority.Low;
                    break;
                case "2":
                    updatedTask.PriorityOptions = TaskItem.Priority.Medium;
                    break;
                case "3":
                    updatedTask.PriorityOptions = TaskItem.Priority.High;
                    break;
                default:
                    break;
            }
            Console.WriteLine("Select a category: \n1.Business \n2.Personal \n3.Educational (leave empty to keep current)");
            string? categoryInput = Console.ReadLine();
            switch (categoryInput)
            {
                case "1":
                    updatedTask.CategoryOptions = TaskItem.Categories.Business;
                    break;
                case "2":
                    updatedTask.CategoryOptions = TaskItem.Categories.Personal;
                    break;
                case "3":
                    updatedTask.CategoryOptions = TaskItem.Categories.Educational;
                    break;
                default:
                    break;
            }
        }
        else
        {
            Console.WriteLine("Invalid task number.");
        }
        ITaskRepository repository = new SqlTaskRepository();
        repository.Save(tasks);
    }

    static void Main(string[] args)
    {
        ITaskRepository repository = new SqlTaskRepository();
        List<TaskItem> taskList = new List<TaskItem>();
        try
        {
            taskList = repository.Load();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured! {ex.Message}");
        }
        finally
        {
            Console.WriteLine("try catch finished");
        }

        while (true)
        {
            Console.WriteLine("Task List Options:");
            Console.WriteLine("1. View tasks");
            Console.WriteLine("2. Add a task");
            Console.WriteLine("3. Remove a task");
            Console.WriteLine("4. Exit");
            Console.WriteLine("5. Mark as Completed");
            Console.WriteLine("6. Sort/Filter");
            Console.WriteLine("7. Edit a task");
            Console.Write("Select an option (1-7): ");
            string? taskListOptions = Console.ReadLine();

            switch (taskListOptions)
            {
                case "1":
                    ViewTasks(taskList);
                    break;
                case "2":
                    AddTask(taskList);
                    break;
                case "3":
                    RemoveTask(taskList);
                    break;
                case "4":
                    Console.WriteLine("Exiting the program.");
                    return;
                case "5":
                    MarkTaskAsCompleted(taskList);
                    break;
                case "6":
                    Sort_Filter(taskList);
                    break;
                case "7":
                    EditTask(taskList);
                    break;
                default:
                    Console.WriteLine("Invalid option. Please select a valid option (1-7).");
                    break;
            }
        }
    }
}