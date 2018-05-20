using Blackmaw.Api.Core;
using Blackmaw.Api.Models;
using Blackmaw.Dal.DbContext;
using Blackmaw.Dal.Entities;
using Microsoft.Extensions.Logging;

namespace Blackmaw.Api.Controllers
{
    public class GameController : ControllerBase<Game, GameModel>
    {
        public GameController(BmDbContext context, ILogger logger) : base(context, logger)
        {
            
        }
    }
}
