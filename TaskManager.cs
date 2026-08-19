public class TaskManager
{
    private readonly List<TaskItem> taskM = new();

    public IReadOnlyList<TaskItem> GetAll()
    {
        return taskM.AsReadOnly();
    }
    public void AddTask(string title)
    {
        if (title is null)
        {
            throw new ArgumentNullException(nameof(title), "Görev başlığı null olamaz.");
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Görev başlığı boş olamaz.", nameof(title));
        }
        
        taskM.Add(new TaskItem(title));
    }
    public void RemoveTask(int index)
    {
         if (index < 0 || index >= taskM.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Geçersiz görev indeksi.");
        }

        taskM.RemoveAt(index);
    }
    public void CompleteTask(int index)
    {
        if (index < 0 || index >= taskM.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Geçersiz görev indeksi.");
        }

        taskM[index].IsCompleted = true;
    }
}

