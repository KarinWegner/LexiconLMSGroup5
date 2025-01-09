using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace LMS.Infrastructure.Data
{
    public class DesignTimeLmsContextFactory :IDesignTimeDbContextFactory<LmsContext>
    {
        public LmsContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LmsContext>();

            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LMSDB;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new LmsContext(optionsBuilder.Options);
        }
    }
}
