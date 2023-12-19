using Microsoft.EntityFrameworkCore;

namespace Platinum.Infrastructure.Data.EntityFramework
{
    public class Anhny010920AdministratorContextFactory : DesignTimeDbContextFactoryBase<Anhny010920AdministratorContext>//IDesignTimeDbContextFactory<Anhny010920AdministratorContext>
    {
        //public Anhny010920AdministratorContext CreateDbContext(string[] args)
        //{
        //    var optionsBuilder = new DbContextOptionsBuilder<Anhny010920AdministratorContext>();
        //    optionsBuilder.UseSqlServer(@"Server=DESKTOP-HEOSBPL\NGUYENHAANH2021;Database=Anhny010920Administrator;User Id= sa;Password=Ap010920;");

        //    return new Anhny010920AdministratorContext(optionsBuilder.Options);
        //}
        protected override Anhny010920AdministratorContext CreateNewInstance(DbContextOptions<Anhny010920AdministratorContext> options)
        {
            return Create(ConnectionStringNames.Administrator);
        }
    }
}
