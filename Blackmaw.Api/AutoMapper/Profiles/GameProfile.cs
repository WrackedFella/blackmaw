using AutoMapper;
using Blackmaw.Api.Core;
using Blackmaw.Api.Models;
using Blackmaw.Dal.Core;
using Blackmaw.Dal.Entities;

namespace Blackmaw.Api.AutoMapper.Profiles
{
    public class GameProfile : Profile
    {
        public GameProfile()
        {
            CreateMap<Game, GameModel>();
            CreateMap<GameModel, Game>()
                .IncludeBase<ModelBase, EntityBase>();
        }
    }
}
