using Microsoft.Extensions.Logging;
using ToDoListDomain;

namespace ToDoListApplication
{
    public class ToDoListService : IToDoListService
    {
        private readonly IToDoListRepository _repository;
        private readonly ILogger<ToDoListService> _logger;

        public ToDoListService(IToDoListRepository repository, ILogger<ToDoListService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<ToDoListItemDto>> GetAllItem()
        {
            _logger.LogInformation("Service: Retrieving all to-do list items.");
            try
            {
                var entities = await _repository.GetAllItem();
                if (entities == null)
                {
                    _logger.LogWarning("Service: No to-do list items found.");
                    return new List<ToDoListItemDto>();
                }
                _logger.LogInformation("Service: Retrieved {Count} to-do list items.", entities.Count);
                return entities.Select(x => new ToDoListItemDto
                {
                    ItemId = x.ItemId,
                    ItemTitle = x.ItemTitle,
                    IsCompleted = x.IsCompleted
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service: Error occurred while retrieving to-do list items.");
                throw;
            }
        }

        public async Task<ToDoListItemDto?> GetItemById(int id)
        {
            _logger.LogInformation("Service: Getting item with item ID: {id}.", id);
            try
            { 
                var entity = await _repository.GetItemById(id);
                if (entity == null)
                {
                    _logger.LogInformation("Service: Unable to find item ID: {id}.", id);
                    return null;
                }
                _logger.LogInformation("Service: Item retrieved with item ID: {id}.", id);
                return new ToDoListItemDto
                {
                    ItemId = entity.ItemId,
                    ItemTitle = entity.ItemTitle,
                    IsCompleted = entity.IsCompleted
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Serivce: Error occured while retreiving item with item ID: {id}", id);
                throw;
            }
        }

        public async Task AddItem(ToDoListItemDto dto)
        {
            _logger.LogInformation("Service: Adding new item.");
            var entity = new ToDoListItem
            {
                ItemTitle = dto.ItemTitle,
                IsCompleted = dto.IsCompleted
            };
            try
            {
                await _repository.AddItem(entity);
                dto.ItemId = entity.ItemId;
                _logger.LogInformation("Service: Item added successfully with ItemId: {ItemId}.", dto.ItemId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service: Error occurred while adding new item.");
                throw;
            }

        }

        public async Task UpdateItem(ToDoListItemDto dto)
        {
            _logger.LogInformation("Service: Updating item with item ID: {itemId}", dto.ItemId);
            try
            {
                var entity = new ToDoListItem
                {
                    ItemId = dto.ItemId,
                    ItemTitle = dto.ItemTitle,
                    IsCompleted = dto.IsCompleted
                };
                await _repository.UpdateItem(entity);
                _logger.LogInformation("Service: Successfully updated item with item ID: {itemId}", dto.ItemId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service: Error occured while updating item with item ID: {itemId}", dto.ItemId);
                throw;
            }
        }

        public async Task DeleteItem(int id)
        {
            _logger.LogInformation("Service: Deleting item with item ID: {itemId}", id);
            try
            {
                await _repository.DeleteItem(id);
                _logger.LogInformation("Service: Sucessfully deleted item with item ID: {itemId}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service: Error occured while deleteing item with item ID: {itemId}", id);
                throw;
            }
        }
    }
}
