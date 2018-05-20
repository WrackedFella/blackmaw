using AutoMapper;
using Blackmaw.Api.Models;
using Blackmaw.Dal.Entities;

namespace Blackmaw.Api.AutoMapper.Profiles
{
    public class GameProfile : Profile
    {
        public GameProfile()
        {
            CreateMap<Game, GameModel>();
        }
    }
}
