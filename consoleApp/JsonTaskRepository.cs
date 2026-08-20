using System.Text.Json.Serialization;
using System.Text.Json;
class JsonTaskRepository : ITaskRepository
{
    public List<TaskItem> Load()
    {
        if (File.Exists("tasks.json"))
        {
            string json = File.ReadAllText("tasks.json");
            return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
        }
        return new List<TaskItem>();
    }

    public void Save(List<TaskItem> tasks)
    {
        File.WriteAllText("tasks.json", JsonSerializer.Serialize(tasks));
    }
}
