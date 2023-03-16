using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebAPIProject.Models;

namespace WebAPIProject.Data
{
    public class DataContext:DbContext
    {

        public DataContext(DbContextOptions<DbContext> options) : base(options)
        {

        }
        public DbSet<City> Cities { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public DbSet<User> Users { get; set; }

    }
}
