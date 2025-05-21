using Microsoft.EntityFrameworkCore;
using ToDoListDomain;

namespace ToDoListInfrastructure
{
    public class ToDoListRepository : IToDoListRepository
    {
        private readonly ApiDbContext _context;

        public ToDoListRepository(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<List<ToDoListItem>> GetAllItem()
        {
            return await _context.ToDoListItems.ToListAsync();
        }

        public async Task<ToDoListItem?> GetItemById(int id)
        {
            return await _context.ToDoListItems.FindAsync(id);
        }

        public async Task AddItem(ToDoListItem item)
        {
            _context.ToDoListItems.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateItem(ToDoListItem item)
        {
            _context.ToDoListItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItem(int id)
        {
            var item = await _context.ToDoListItems.FindAsync(id);
            if (item != null)
            {
                _context.ToDoListItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
