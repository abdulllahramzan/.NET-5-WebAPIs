using Microsoft.EntityFrameworkCore;
using myapp.Models;

namespace myapp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        public DbSet<Character> Characters { get; set; }
    }
}