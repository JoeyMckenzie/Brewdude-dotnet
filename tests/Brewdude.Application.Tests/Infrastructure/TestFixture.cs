using System;
using AutoMapper;
using Brewdude.Common;
using Brewdude.Persistence;
using Xunit;

namespace Brewdude.Application.Tests.Infrastructure
{
    public class TestFixture : IDisposable
    {
        public BrewdudeDbContext Context { get; }
        public BrewdudeDbIdentityContext IdentityContext { get; }
        public IMapper Mapper { get; }
        public IDateTime MachineDateTime { get; }
        public string UserId { get; }        

        public TestFixture()
        {
            Context = BrewdudeDbContextFactory.Create();
            IdentityContext = BrewdudeDbIdentityContextFactory.Create(out string testUserId);
            Mapper = AutoMapperFactory.Create();
            MachineDateTime = new DateTimeTest();
            UserId = testUserId;
        }
        
        public void Dispose()
        {
            BrewdudeDbContextFactory.Destroy(Context);
            BrewdudeDbIdentityContextFactory.Destroy(IdentityContext);
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<TestFixture>
    {
    }

    [CollectionDefinition("CommandCollection")]    
    public class CommandCollection : ICollectionFixture<TestFixture>
    {
    }
    
}