using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackmaw.Api.Controllers;
using Blackmaw.Api.Models;
using Blackmaw.Dal.DbContext;
using Blackmaw.UnitTests.Core;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Blackmaw.UnitTests.ControllerTests
{
    public class GameControllerTests : TestBed
    {
        protected override BmDbContext GenerateContext()
        {
            var context = base.GenerateContext();

            //context.Member.AddRange();
            context.SaveChanges();
            return context;
        }

        public GameControllerTests(AutoMapperFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Create_GivenOneNewRecord_ShouldReturn200()
        {
            // Arrange
            var context = this.GenerateContext();
            var controller = new GameController(context, null);
            var model = new GameModel();

            // Act
            var result = (await controller.Create(model)).Value;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            var returnValue = result.RouteValues.Values.First().ToString();
            Assert.Single(context.Games.Where(x => x.Id.ToString() == returnValue)); 
        }
    }
}
