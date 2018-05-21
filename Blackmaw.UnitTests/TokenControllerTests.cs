using Blackmaw.Dal.DbContext;
using Blackmaw.UnitTests.Core;
using System.Threading.Tasks;
using Blackmaw.Api.Controllers;
using Blackmaw.Api.Models;
using Xunit;

namespace Blackmaw.UnitTests
{
    public class TokenControllerTests : TestBed
    {
        protected override BmDbContext GenerateContext()
        {
            var context = base.GenerateContext();

            // context.Games.AddRange();
            context.SaveChanges();
            return context;
        }

        public TokenControllerTests(AutoMapperFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Create_GivenOneNewRecord_ShouldInsertMemberAndMemberHistoryRecords()
        {
            //// Arrange
            //var context = this.GenerateContext();
            //var controller = new TokenController(context, null);
            //var model = new GameModel();

            //// Act
            //var result = await controller.SignIn(model);

            //// Assert
            //Assert.Single(context.Member.Where(x => x.MemberId == result));
            //Assert.Single(context.MemberLocalHistory.Where(x => x.MemberId == result));
        }

    }
}
