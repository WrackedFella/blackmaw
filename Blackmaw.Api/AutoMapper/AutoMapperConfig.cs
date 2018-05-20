using AutoMapper;
using Blackmaw.Api.AutoMapper.Profiles;

namespace Blackmaw.Api.AutoMapper
{
    public class AutoMapperConfig
    {
        public static void RegisterAutoMapperProfiles()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<GameProfile>();
            });
        }
    }
}
