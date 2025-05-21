namespace ToDoListApplication
{
    public interface IToDoListService
    {
        Task<List<ToDoListItemDto>> GetAllItem();
        Task<ToDoListItemDto?> GetItemById(int ItemId);
        Task AddItem(ToDoListItemDto item);
        Task UpdateItem(ToDoListItemDto item);
        Task DeleteItem(int ItemId);
    }
}
