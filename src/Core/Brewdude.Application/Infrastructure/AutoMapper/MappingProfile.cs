using AutoMapper;
using Brewdude.Applicaio.Beer.Queries.GetAllBeers;
using Brewdude.Application.Beer.Queries.GetBeerById;

namespace Brewdude.Application.Infrastructure.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.Entities.Beer, BeerViewModel>();
            CreateMap<Domain.Entities.Beer, BeerDto>();

            CreateMap<BeerViewModel, Domain.Entities.Beer>();
            CreateMap<BeerDto, Domain.Entities.Beer>();
        }
    }
}