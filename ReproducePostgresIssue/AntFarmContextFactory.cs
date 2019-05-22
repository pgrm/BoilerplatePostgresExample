using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ReproducePostgresIssue
{
    public class AntFarmContextFactory : IDesignTimeDbContextFactory<AntFarmContext>
    {
        public AntFarmContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AntFarmContext>();
            optionsBuilder.UseNpgsql("User Id=postgres;Password=mysecretpassword;Server=127.0.0.1;Port=5432;Database=apaleo;Pooling=true;Max Auto Prepare=5;");

            return new AntFarmContext(optionsBuilder.Options);
        }
    }
}