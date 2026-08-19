interface ITaskRepository
{
    List<TaskItem> Load();
    void Save(List<TaskItem> tasks);
    
}