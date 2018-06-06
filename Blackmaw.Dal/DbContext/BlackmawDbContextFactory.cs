using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;

namespace Blackmaw.Dal.DbContext
{
    class BlackmawDbContextFactory : IDesignTimeDbContextFactory<BmDbContext>
    {
        public BmDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<BmDbContext>();
            builder.UseSqlServer("server=.;database=blackmaw.dev;Integrated Security=True;Persist Security Info=True",
                optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(BmDbContext).GetTypeInfo().Assembly.GetName().Name));

            return new BmDbContext(builder.Options);
        }
    }
}
