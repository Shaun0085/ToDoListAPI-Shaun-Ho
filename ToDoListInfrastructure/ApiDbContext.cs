using Microsoft.EntityFrameworkCore;
using ToDoListDomain;

namespace ToDoListInfrastructure
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        public DbSet<ToDoListItem> ToDoListItems { get; set; }
    }
}
