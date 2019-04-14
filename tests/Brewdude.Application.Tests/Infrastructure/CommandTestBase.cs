using System;
using Brewdude.Persistence;
using Microsoft.Extensions.Logging;

namespace Brewdude.Application.Tests.Infrastructure
{
    public class CommandTestBase : IDisposable
    {
        protected readonly BrewdudeDbContext Context;
        protected string UserId;

        public CommandTestBase()
        {
        }

        public void Dispose()
        {
            BrewdudeDbContextFactory.Destroy(Context);
        }
    }
}