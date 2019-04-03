using System;
using AutoMapper;
using Brewdude.Persistence;
using Xunit;

namespace Brewdude.Application.Tests.Infrastructure
{
    public class QueryTestFixture : IDisposable
    {
        public BrewdudeDbContext Context { get; }
        public BrewdudeDbIdentityContext IdentityContext { get; }
        public IMapper Mapper { get; }
        public string UserId { get; }        

        public QueryTestFixture()
        {
            Context = BrewdudeDbContextFactory.Create();
            IdentityContext = BrewdudeDbIdentityContextFactory.Create(out string testUserId);
            Mapper = AutoMapperFactory.Create();
            UserId = testUserId;
        }
        
        public void Dispose()
        {
            BrewdudeDbContextFactory.Destroy(Context);
            BrewdudeDbIdentityContextFactory.Destroy(IdentityContext);
        }
    }

    [CollectionDefinition("QueryCollection")]    
    public class QueryCollection : ICollectionFixture<QueryTestFixture>
    {
    }
}