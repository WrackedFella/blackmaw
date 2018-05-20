using AutoMapper;
using blackmaw.api.AutoMapper.Profiles;

namespace blackmaw.api.AutoMapper
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
