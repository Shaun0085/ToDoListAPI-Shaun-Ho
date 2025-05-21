namespace ToDoListDomain
{
    public interface IToDoListRepository
    {
        Task<List<ToDoListItem>> GetAllItem();
        Task<ToDoListItem?> GetItemById(int id);
        Task AddItem(ToDoListItem item);
        Task UpdateItem(ToDoListItem item);
        Task DeleteItem(int id);
    }
}
