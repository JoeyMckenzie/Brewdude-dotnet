namespace Brewdude.Application
{
    using System.Reflection;
    using Beer.Commands.CreateBeer;
    using Beer.Commands.DeleteBeer;
    using Beer.Commands.UpdateBeer;
    using Beer.Queries.GetAllBeers;
    using Beer.Queries.GetBeerById;
    using Brewery.Commands.CreateBrewery;
    using Brewery.Commands.DeleteBrewery;
    using Brewery.Commands.UpdateBrewery;
    using Brewery.Queries.GetAllBreweries;
    using Brewery.Queries.GetBreweryById;
    using User.Commands.CreateUser;
    using User.Queries.GetUserById;
    using User.Queries.GetUserByUsername;
    using UserBeers.Commands.CreateUserBeer;
    using UserBeers.Commands.DeleteUserBeer;
    using UserBeers.Queries.GetBeersByUserId;
    using UserBreweries.Commands.CreateUserBrewery;

    public static class BrewdudeRequestHandlers
    {
        public static Assembly[] GetRequestHandlerAssemblies()
        {
            return new[]
            {
                typeof(GetAllBeersQueryHandler).GetTypeInfo().Assembly,
                typeof(GetBeerByIdQueryHandler).GetTypeInfo().Assembly,
                typeof(GetBeersByUserIdQueryHandler).GetTypeInfo().Assembly,
                typeof(CreateBeerCommandHandler).GetTypeInfo().Assembly,
                typeof(DeleteBeerCommandHandler).GetTypeInfo().Assembly,
                typeof(UpdateBeerCommandHandler).GetTypeInfo().Assembly,
                typeof(GetAllBreweriesQueryHandler).GetTypeInfo().Assembly,
                typeof(GetBreweryByIdQueryHandler).GetTypeInfo().Assembly,
                typeof(CreateUserCommandHandler).GetTypeInfo().Assembly,
                typeof(CreateBreweryCommandHandler).GetTypeInfo().Assembly,
                typeof(UpdateBreweryCommandHandler).GetTypeInfo().Assembly,
                typeof(DeleteBreweryCommandHandler).GetTypeInfo().Assembly,
                typeof(GetUserByIdCommandHandler).GetTypeInfo().Assembly,
                typeof(GetUserByUsernameCommandHandler).GetTypeInfo().Assembly,
                typeof(CreateUserBeerCommandHandler).GetTypeInfo().Assembly,
                typeof(CreateUserBreweryCommandHandler).GetTypeInfo().Assembly,
                typeof(DeleteUserBeerCommandHandler).GetTypeInfo().Assembly
            };
        }
    }
}