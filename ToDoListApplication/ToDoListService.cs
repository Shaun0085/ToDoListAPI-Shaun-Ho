using ToDoListDomain;

namespace ToDoListApplication
{
    public class ToDoListService : IToDoListService
    {
        private readonly IToDoListRepository _repository;

        public ToDoListService(IToDoListRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ToDoListItemDto>> GetAllItem()
        {
            var entities = await _repository.GetAllItem();
            return entities.Select(x => new ToDoListItemDto
            {
                ItemId = x.ItemId,
                ItemTitle = x.ItemTitle,
                IsCompleted = x.IsCompleted
            }).ToList();
        }

        public async Task<ToDoListItemDto?> GetItemById(int id)
        {
            var entity = await _repository.GetItemById(id);
            if (entity == null) return null;

            return new ToDoListItemDto
            {
                ItemId = entity.ItemId,
                ItemTitle = entity.ItemTitle,
                IsCompleted = entity.IsCompleted
            };
        }

        public async Task AddItem(ToDoListItemDto dto)
        {
            var entity = new ToDoListItem
            {
                ItemTitle = dto.ItemTitle,
                IsCompleted = dto.IsCompleted
            };
            await _repository.AddItem(entity);
            dto.ItemId = entity.ItemId;
        }

        public async Task UpdateItem(ToDoListItemDto dto)
        {
            var entity = new ToDoListItem
            {
                ItemId = dto.ItemId,
                ItemTitle = dto.ItemTitle,
                IsCompleted = dto.IsCompleted
            };
            await _repository.UpdateItem(entity);
        }

        public async Task DeleteItem(int id)
        {
            await _repository.DeleteItem(id);
        }
    }
}
