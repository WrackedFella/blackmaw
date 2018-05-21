using Blackmaw.Dal.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Blackmaw.UnitTests.Core
{
    [Collection("AutoMapper")]
    public class TestBed
    {
        protected AutoMapperFixture Fixture;
        protected TestBed(AutoMapperFixture fixture)
        {
            this.Fixture = fixture;
        }

        protected virtual BmDbContext GenerateContext()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            DbContextOptions<BmDbContext> options = new DbContextOptionsBuilder<BmDbContext>()
                .UseInMemoryDatabase("MockBlackmawDb")
                // Without this line, data will persist between tests.
                //  May be desirable, under certain conditions.
                .UseInternalServiceProvider(serviceProvider)
                .EnableSensitiveDataLogging()
                .Options;

            return new BmDbContext(options);
        }
    }
}
