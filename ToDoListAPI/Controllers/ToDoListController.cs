using Microsoft.AspNetCore.Mvc;
using ToDoListApplication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ToDoListAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoListController : ControllerBase
    {
        private readonly IToDoListService _service;
        private readonly ILogger<ToDoListController> _logger;
        private readonly IConfiguration _configuration; 

        public ToDoListController(IToDoListService service, ILogger<ToDoListController> logger, IConfiguration configuration)
        {
            _service = service;
            _logger = logger;
            _configuration = configuration;
        }

        // Dummy login to get Token
        [HttpPost("token/v1")]
        [AllowAnonymous]
        public IActionResult GetToken()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll() 
        {
            _logger.LogInformation("Controller. Fetching all to-do-list items.");

            var todos = await _service.GetAllItem();

            _logger.LogInformation("Controller. Successfully fetched a total of {Count} items.", todos.Count());

            return Ok(todos);
        }

        [HttpGet("{ItemId}")]
        [Authorize]
        public async Task<IActionResult> GetbyId(int ItemId)
        {
            _logger.LogInformation("Controller: Fetching to-do-list items: {ItemId}", ItemId);

            var existingItem = await _service.GetItemById(ItemId);

            if (existingItem == null)
            {
                _logger.LogWarning("Controller: Item not found with item ID: {ItemId}", ItemId);
                return NotFound("Item not found.");
            }

            _logger.LogInformation("Controller: Item fetched with item ID: {ItemId}", ItemId);

            return Ok(existingItem);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] ToDoListItemDto todo)
        {
            _logger.LogInformation("Controller: Adding new item.");

            if (string.IsNullOrEmpty(todo.ItemTitle))
            {
                _logger.LogWarning("Controller: Title is missing.");
                return BadRequest("Title is missing.");
            }

            await _service.AddItem(todo);

            _logger.LogInformation("Controller: Item has been added. Item ID: {todo.ItemId}", todo.ItemId);

            return Ok();
        }

        [HttpPut("{ItemId}")]
        [Authorize]
        public async Task<IActionResult> Update(int ItemId, [FromBody] ToDoListItemDto todo)
        {
            _logger.LogInformation("Controller: Updating item with item ID: {itemId}", ItemId);

            try
            {
                await _service.UpdateItem(todo);
                _logger.LogInformation("Controller: Item successfully update. ItemId: {todo.ItemId}.", todo.ItemId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Controller: Item not found with ItemId: {todo.ItemId}", todo.ItemId);
                return NotFound();
            }
        }

        [HttpDelete("{ItemId}")]
        [Authorize]
        public async Task<IActionResult> Delete(int ItemId)
        {
            _logger.LogInformation("Contrller: Deleting Item: {ItemId}.", ItemId);

            var existingItem = await _service.GetItemById(ItemId);

            if (existingItem == null)
            {
                _logger.LogWarning("Controller: Item not found with ItemId: {ItemId}",ItemId);
                return NotFound();
            }

            await _service.DeleteItem(ItemId);

            _logger.LogInformation("Controller: Item Deleted with ItemId: {ItemId}.", ItemId);

            return Ok();
        }
    }
}
