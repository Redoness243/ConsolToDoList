using System;
using System.IO;
using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic;
using System.Linq;

partial class Program
{
    static void ViewTasks(TaskManager tasks)
    {
        var taskList = tasks.GetAll();
        foreach (var t in taskList.Where(t => !t.IsCompleted))
        {
            Console.WriteLine(t.Title);
        }
        Console.WriteLine(taskList.Count(t => t.IsCompleted));

        Console.WriteLine("Current tasks:");
        foreach (var task in taskList)
        {
            Console.WriteLine(task);
        }
        if (taskList.Count == 0)
        {
            Console.WriteLine("No tasks available.");
        }
    }
    static void AddTask(TaskManager tasks)
    {
        var taskList = tasks.GetAll();
        Console.WriteLine("Enter a task to add:");
        string? newTask = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(newTask))
        {
            Console.WriteLine("No task entered.");
            return;
        }


        Console.WriteLine("Enter a priority: \n1.Low \n2.Medium \n3.High");
        string? priorityInput = Console.ReadLine();
        TaskItem.Priority priority;

        switch (priorityInput)
        {
            case "1":
                priority = TaskItem.Priority.Low;
                break;
            case "2":
                priority = TaskItem.Priority.Medium;
                break;
            case "3":
                priority = TaskItem.Priority.High;
                break;
            default:
                Console.WriteLine("Invalid option. Please select a valid option (1-3).");
                return;
        }

        Console.WriteLine("Select a category: \n1.Business \n2.Personal \n3.Educational");
        string? categoryInput = Console.ReadLine();
        TaskItem.Categories category;
        switch (categoryInput)
        {
            case "1":
                category = TaskItem.Categories.Business;
                break;
            case "2":
                category = TaskItem.Categories.Personal;
                break;
            case "3":
                category = TaskItem.Categories.Educational;
                break;
            default:
                Console.WriteLine("Invalid option. Please select a valid option (1-3).");
                return;
        }

        tasks.AddTask(newTask);
        var addedTask = tasks.GetAll().Last();
        addedTask.PriorityOptions = priority;
        addedTask.CategoryOptions = category;
        ITaskRepository repository = new SqlTaskRepository();
        repository.Save(tasks.GetAll().ToList());
        Console.WriteLine($"Task '{newTask}' added, it is priority is: '{priority}' and it is category is: '{category}.");
    }
    static void RemoveTask(TaskManager tasks)
    {
        var taskList = tasks.GetAll();

        if (taskList.Count > 0)
        {
            Console.WriteLine("Enter the task number to remove:");
            for (int i = 1; i < taskList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {taskList[i].Title}");
            }
        }
        else
        {
            Console.WriteLine("No tasks available.");
            return;
        }

        string? taskNumberInput = Console.ReadLine();
        if (int.TryParse(taskNumberInput, out int taskNumber) && taskNumber > 0 && taskNumber <= taskList.Count)
        {
            TaskItem removedTask = taskList[taskNumber - 1];
            tasks.RemoveTask(taskNumber - 1);
            Console.WriteLine($"Task '{removedTask.Title}' removed.");
        }
        else
        {
            Console.WriteLine("Invalid task number.");
        }
        ITaskRepository repository = new SqlTaskRepository();
        repository.Save(tasks.GetAll().ToList());
    }
    static void MarkTaskAsCompleted(TaskManager tasks)
    {
        var taskList = tasks.GetAll();
        if (taskList.Count == 0)
        {
            Console.WriteLine("No tasks available.");
            return;
        }
        Console.WriteLine("Enter the task number to mark as completed:");
        for (int i = 0; i < taskList.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {taskList[i].Title} [{(taskList[i].IsCompleted ? "X" : " ")}]");
        }
        string? taskNumberInput = Console.ReadLine();
        if (int.TryParse(taskNumberInput, out int taskNumber) && taskNumber > 0 && taskNumber <= taskList.Count)
        {
            taskList[taskNumber - 1].IsCompleted = true;
            tasks.CompleteTask(taskNumber - 1);
            Console.WriteLine($"Task '{taskList[taskNumber - 1].Title}' marked as completed.");
        }
        else
        {
            Console.WriteLine("Invalid task number.");
        }
        ITaskRepository repository = new SqlTaskRepository();
        repository.Save(tasks.GetAll().ToList());
    }
    static void Sort_Filter(TaskManager tasks)
    {
        var taskList = tasks.GetAll();
        Console.WriteLine("Choose a sort/filter type: \n1.Date \n2.High Priority");
        string? sort_filter = Console.ReadLine();
        switch (sort_filter)
        {
            case "1":
                foreach (var task in taskList.OrderBy(t => t.CreatedAt))
                {
                    Console.WriteLine(task);
                }
                break;
            case "2":
                foreach (var task in taskList.Where(t => t.PriorityOptions == TaskItem.Priority.High))
                    Console.WriteLine(task);
                break;
            default:
                Console.WriteLine("Invalid option. Please select a valid option (1/2).");
                break;
        }
    }

    static void EditTask(TaskManager tasks)
    {
        var taskList = tasks.GetAll();
        if (taskList.Count > 0)
        {
            Console.WriteLine("Enter the task number to update:");
            for (int i = 0; i < taskList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {taskList[i].Title}");
            }
        }
        else
        {
            Console.WriteLine("No tasks available.");
            return;
        }

        string? taskNumberUpdate = Console.ReadLine();
        if (int.TryParse(taskNumberUpdate, out int taskNumber) && taskNumber > 0 && taskNumber <= taskList.Count)
        {
            TaskItem updatedTask = taskList[taskNumber - 1];
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
        repository.Save(tasks.GetAll().ToList());
    }

    static void Main(string[] args)
    {
        ITaskRepository repository = new SqlTaskRepository();
        TaskManager taskManager = new TaskManager();

        try
        {
            var loadedTasks = repository.Load();

            foreach (var item in loadedTasks)
            {
                taskManager.AddTask(item.Title);
                var currentTask = taskManager.GetAll().Last();
                currentTask.PriorityOptions = item.PriorityOptions;
                currentTask.CategoryOptions = item.CategoryOptions;
                currentTask.CreatedAt = item.CreatedAt;

                if (item.IsCompleted)
                {
                    taskManager.CompleteTask(taskManager.GetAll().Count - 1);
                }
            }
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
                    ViewTasks(taskManager);
                    break;
                case "2":
                    AddTask(taskManager);
                    break;
                case "3":
                    RemoveTask(taskManager);
                    break;
                case "4":
                    Console.WriteLine("Exiting the program.");
                    return;
                case "5":
                    MarkTaskAsCompleted(taskManager);
                    break;
                case "6":
                    Sort_Filter(taskManager);
                    break;
                case "7":
                    EditTask(taskManager);
                    break;
                default:
                    Console.WriteLine("Invalid option. Please select a valid option (1-7).");
                    break;
            }
        }
    }
}