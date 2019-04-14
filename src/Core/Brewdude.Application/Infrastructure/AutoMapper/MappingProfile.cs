using AutoMapper;
using Brewdude.Application.UserBeers.Commands.CreateUserBeer;
using Brewdude.Application.UserBreweries.Commands;
using Brewdude.Domain.Dtos;
using Brewdude.Domain.Entities;
using Brewdude.Domain.ViewModels;

namespace Brewdude.Application.Infrastructure.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // View Models
            CreateMap<Domain.Entities.Beer, BeerViewModel>()
                .ForMember(b => b.BeerStyle, m => m.MapFrom(b => b.BeerStyle));
            CreateMap<BeerViewModel, Domain.Entities.Beer>();
            
            CreateMap<BrewdudeUser, UserViewModel>()
                .ForMember(u => u.Id, m => m.MapFrom(b => b.Id));
            CreateMap<UserViewModel, BrewdudeUser>();

            CreateMap<Domain.Entities.Brewery, BreweryViewModel>();
            
            
            // Dtos
            CreateMap<Domain.Entities.Beer, BeerDto>()
                .ForMember(b => b.BeerStyle, m => m.MapFrom(b => b.BeerStyle));
            CreateMap<BeerDto, Domain.Entities.Beer>();

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
            
            CreateMap<Domain.Entities.Beer, UserBeerDto>();
            
            CreateMap<Domain.Entities.Brewery, UserBreweryDto>();
            
            
            // MediatR Requests
            CreateMap<CreateUserBeerCommand, UserBeer>();
            
            CreateMap<CreateUserBreweryCommand, UserBrewery>();
        }
    }
}