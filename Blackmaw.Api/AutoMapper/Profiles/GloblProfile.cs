using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blackmaw.Api.Core;
using Blackmaw.Dal.Core;

namespace Blackmaw.Api.AutoMapper.Profiles
{
    public class GloblProfile : Profile
    {
        public GloblProfile()
        {
            CreateMap<ModelBase, EntityBase>();
        }
    }
}
