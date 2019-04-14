using System;
using System.Threading.Tasks;
using AutoMapper;
using Brewdude.Common;
using Brewdude.Persistence;
using Xunit;

namespace Brewdude.Application.Tests.Infrastructure
{
    public class TestFixture : IDisposable
    {
        public BrewdudeDbContext Context { get; }
        public IMapper Mapper { get; }
        public IDateTime MachineDateTime { get; }
        public string UserId { get; }        

        public TestFixture()
        {
            Context = BrewdudeDbContextFactory.Create();
            Mapper = AutoMapperFactory.Create();
            MachineDateTime = new DateTimeTest();
            UserId = BrewdudeDbContextFactory.UserId;
        }
        
        public void Dispose()
        {
            BrewdudeDbContextFactory.Destroy(Context);
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