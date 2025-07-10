using Microsoft.AspNetCore.Mvc;
using ToDoListApplication;
using Microsoft.Extensions.Logging;

namespace ToDoListAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoListController : ControllerBase
    {
        private readonly IToDoListService _service;
        private readonly ILogger<ToDoListController> _logger;

        public ToDoListController(IToDoListService service, ILogger<ToDoListController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            _logger.LogInformation("Fetching all to-do-list items.");
            var todos = await _service.GetAllItem();

            _logger.LogInformation("Successfully fetched a total of {Count} items.", todos.Count());
            return Ok(todos);
        }

        [HttpGet("{ItemId}")]
        public async Task<IActionResult> GetbyId(int ItemId)
        {
            _logger.LogInformation("Fetching to-do-list items: {@ItemId}", ItemId);
            var todo = await _service.GetItemById(ItemId);

            if (todo == null)
            {
                _logger.LogWarning("The Item cannot be found {@ItemId}", ItemId);
                return NotFound("Item not found.");
            }

            _logger.LogInformation("Item has been fetched {@ItemId}", ItemId);
            return Ok(todo);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ToDoListItemDto todo)
        {
            _logger.LogInformation("Adding new item.");

            if (string.IsNullOrEmpty(todo.ItemTitle))
            {
                _logger.LogWarning("Title is missing.");
                return BadRequest("Title is missing.");
            }
                
            await _service.AddItem(todo);
            _logger.LogInformation("The Item has been added {@todo.ItemId}", todo.ItemId);
            return Ok();
        }

        [HttpPut("{ItemId}")]
        public async Task<IActionResult> Update(int ItemId, [FromBody] ToDoListItemDto todo)
        {
            try
            {
                await _service.UpdateItem(todo);
                _logger.LogInformation("Item successfully update. ItemId: {@todo.ItemId}.", todo.ItemId);
                return Ok();
            }
            catch (Exception ex) 
            {
                _logger.LogInformation(ex,"Item not found with ItemId: {@todo.ItemId}", todo.ItemId);
                return NotFound();
            }
        }

        [HttpDelete("{ItemId}")]
        public async Task<IActionResult> Delete(int ItemId)
        {
            var existingItem = await _service.GetItemById(ItemId);

            if (existingItem == null)
            {
                _logger.LogWarning("Unable to delete. Item not found with ItemId: {@ItemId}",ItemId);
                return NotFound();
            }
            _logger.LogInformation("Delete Item: {@ItemId}.", ItemId);
            await _service.DeleteItem(ItemId);

            _logger.LogInformation("Item Delete with ItemId: {@ItemId}.", ItemId);
            return Ok();
        }
    }
}
