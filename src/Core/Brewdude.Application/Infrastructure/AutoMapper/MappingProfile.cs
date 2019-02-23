using AutoMapper;
using Brewdude.Applicaio.Beer.Queries.GetAllBeers;
using Brewdude.Application.Beer.Queries.GetBeerById;
using Brewdude.Application.Brewery.Queries.GetBreweryById;
using Brewdude.Application.User.Commands.Models;
using Brewdude.Application.UserBeers.GetBeersByUserId;
using Brewdude.Application.UserBreweries.GetBreweriesByUserId;
using Brewdude.Domain.Entities;

namespace Brewdude.Application.Infrastructure.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.Entities.Beer, BeerViewModel>()
                .ForMember(b => b.BeerStyle, m => m.MapFrom(b => b.BeerStyle));
            CreateMap<Domain.Entities.Beer, BeerDto>()
                .ForMember(b => b.BeerStyle, m => m.MapFrom(b => b.BeerStyle));
            CreateMap<Domain.Entities.User, UserViewModel>();
            CreateMap<BrewdudeUser, UserViewModel>()
                .ForMember(u => u.Id, m => m.MapFrom(b => b.Id));
            CreateMap<BeerViewModel, Domain.Entities.Beer>();
            CreateMap<BeerDto, Domain.Entities.Beer>();
            CreateMap<Domain.Entities.Brewery, BreweryViewModel>();
            CreateMap<Domain.Entities.Beer, UserBeerDto>();
            CreateMap<Domain.Entities.Brewery, UserBreweryDto>();
        }
    }
}