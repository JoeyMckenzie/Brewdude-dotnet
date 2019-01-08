using System;
using AutoMapper;
using Brewdude.Persistence;
using Xunit;

namespace Brewdude.Application.Tests.Infrastructure
{
    public class QueryTestFixture : IDisposable
    {
        public BrewdudeDbContext Context { get; private set; }
        public IMapper Mapper { get; private set; }

        public QueryTestFixture()
        {
            Context = BrewdudeDbContextFactory.Create();
            Mapper = AutoMapperFactory.Create();
        }
        
        public void Dispose()
        {
            BrewdudeDbContextFactory.Destroy(Context);
        }
    }

    [CollectionDefinition("QueryCollection")]    
    public class QueryCollection : ICollectionFixture<QueryTestFixture>
    {
    }
}