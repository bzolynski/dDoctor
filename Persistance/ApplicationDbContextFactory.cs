using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {        
        public ApplicationDbContext CreateDbContext(string[] args = null)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // TODO: Change this
            optionsBuilder.UseSqlServer("Server=.;Database=dDoctor;Trusted_Connection=True;");
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
