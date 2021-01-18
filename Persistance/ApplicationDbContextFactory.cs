using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Persistance
{
    public class ApplicationDbContextFactory 
    {
        private readonly string _connectionString;

        public ApplicationDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }
        public ApplicationDbContext CreateDbContext(string[] args = null)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            optionsBuilder.UseSqlServer(_connectionString);
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
