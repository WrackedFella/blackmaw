using AutoMapper;
using blackmaw.api.Models;
using Blackmaw.Dal.Entities;

namespace blackmaw.api.AutoMapper.Profiles
{
    public class GameProfile : Profile
    {
        public GameProfile()
        {
            CreateMap<Game, GameModel>();
        }
    }
}
