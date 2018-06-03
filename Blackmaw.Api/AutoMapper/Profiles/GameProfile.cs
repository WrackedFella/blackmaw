using System.Linq;
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
            CreateMap<Game, GameModel>()
                .ForMember(dest => dest.CharacterNames, opt => opt.MapFrom(src => src.Characters.Select(x => x.Name).ToList()));
            CreateMap<GameModel, Game>()
                .IncludeBase<ModelBase, EntityBase>();
        }
    }
}
