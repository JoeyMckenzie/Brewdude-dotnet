using AutoMapper;
using Brewdude.Applicaio.Beer.Queries.GetAllBeers;
using Brewdude.Application.Beer.Queries.GetBeerById;
using Brewdude.Application.Brewery.Queries.GetBreweryById;
using Brewdude.Application.User.Commands.Models;

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
            CreateMap<BeerViewModel, Domain.Entities.Beer>();
            CreateMap<BeerDto, Domain.Entities.Beer>();

            CreateMap<Domain.Entities.Brewery, BreweryViewModel>();
        }
    }
}