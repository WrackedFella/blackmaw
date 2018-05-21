using System;
using AutoMapper;
using Blackmaw.Api.AutoMapper;
using Xunit;

namespace Blackmaw.UnitTests.Core
{
    public class AutoMapperFixture : IDisposable
    {
        public AutoMapperFixture()
        {
            AutoMapperConfig.RegisterAutoMapperProfiles();
        }

        public void Dispose()
        {
            Mapper.Reset();
        }
    }

    [CollectionDefinition("AutoMapper")]
    public class AutoMapperCollection : ICollectionFixture<AutoMapperFixture>
    {
        // Empty, used to attach new ICollectionFixture<> definitions to this collection. 
        //  i.e. you can have multiple Fixtures to a Collection. This class lets you do it.
    }
}
