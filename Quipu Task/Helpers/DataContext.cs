using Microsoft.EntityFrameworkCore;
using Quipu_Task.Models;

namespace Quipu_Task.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
    : base(options)
        { }
        public DbSet<ClientInfo> clientInfo { get; set; }
    }
}
