namespace Brewdude.Application.Infrastructure.AutoMapper
{
    using Domain.Dtos;
    using Domain.Entities;
    using Domain.ViewModels;
    using global::AutoMapper;
    using UserBeers.Commands.CreateUserBeer;
    using UserBreweries.Commands.CreateUserBrewery;

    /// <summary>
    /// Constructs all the mapping profiles between entities and view models, data transfer objects, and simple MediatR requests.
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // View Models
            CreateMap<Beer, BeerViewModel>()
                .ForMember(b => b.BeerStyle, m => m.MapFrom(b => b.BeerStyle));
            CreateMap<BeerViewModel, Beer>();

            CreateMap<BrewdudeUser, UserViewModel>()
                .ForMember(u => u.Id, m => m.MapFrom(b => b.Id));

            CreateMap<UserViewModel, BrewdudeUser>();

            CreateMap<Brewery, BreweryViewModel>();

            CreateMap<BreweryViewModel, Beer>();

            // Dtos
            CreateMap<Beer, BeerDto>()
                .ForMember(b => b.BeerStyle, m => m.MapFrom(b => b.BeerStyle));

            CreateMap<BeerDto, Beer>();

            CreateMap<Address, AddressDto>()
                .ForMember(a => a.City, m => m.MapFrom(a => a.City))
                .ForMember(a => a.State, m => m.MapFrom(a => a.State))
                .ForMember(a => a.StreetAddress, m => m.MapFrom(a => a.StreetAddress))
                .ForMember(a => a.ZipCode, m => m.MapFrom(a => a.ZipCode));
            CreateMap<AddressDto, Address>()
                .ForMember(a => a.City, m => m.MapFrom(a => a.City))
                .ForMember(a => a.State, m => m.MapFrom(a => a.State))
                .ForMember(a => a.StreetAddress, m => m.MapFrom(a => a.StreetAddress))
                .ForMember(a => a.ZipCode, m => m.MapFrom(a => a.ZipCode));

            CreateMap<Beer, UserBeerDto>();

            CreateMap<Brewery, UserBreweryDto>();

            // MediatR Requests
            CreateMap<CreateUserBeerCommand, UserBeer>();

            CreateMap<CreateUserBreweryCommand, UserBrewery>();
        }
    }
}