using Microsoft.AspNetCore.Mvc;
using ToDoListApplication;

namespace ToDoListAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoListController : ControllerBase
    {
        private readonly IToDoListService _service;

        public ToDoListController(IToDoListService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllItem());

        [HttpGet("{ItemId}")]
        public async Task<IActionResult> GetbyId(int ItemId)
        {
            var todo = await _service.GetItemById(ItemId);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ToDoListItemDto todo)
        {
            if (string.IsNullOrEmpty(todo.ItemTitle))
                return BadRequest("Please Enter A Title");

            await _service.AddItem(todo);
            return Ok();
        }

        [HttpPut("{ItemId}")]
        public async Task<IActionResult> Update(int ItemId, [FromBody] ToDoListItemDto todo)
        {
            await _service.UpdateItem(todo);
            return Ok();
        }

        [HttpDelete("{ItemId}")]
        public async Task<IActionResult> Delete(int ItemId)
        {
            await _service.DeleteItem(ItemId);
            return Ok();
        }
    }
}
