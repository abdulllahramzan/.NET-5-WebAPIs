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
        public DbSet<User> Users { get; set; }

        public DbSet<Weapon>  Weapons{ get; set; }

        public DbSet<Skill> Skills{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>().HasData(
                new Skill {Id = 1, Name = "Sword", Damage =20},
                new Skill {Id = 2, Name = "FireBall", Damage =40},
                new Skill {Id = 3, Name = "Archery", Damage =30}    
            );
        }
    }
}