using Microsoft.EntityFrameworkCore;
using Tutorial.Web.Model;

namespace Tutorial.Web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
                : base(options)
        {

        }
        public DbSet<Student> Students { get; set; }
    }
}
