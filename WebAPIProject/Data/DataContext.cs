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
        public City Cities { get; set; }

        public Photo Photos { get; set; }

        public User Users { get; set; }

    }
}
