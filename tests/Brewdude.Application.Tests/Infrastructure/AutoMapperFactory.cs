using AutoMapper;
using Brewdude.Application.Infrastructure.AutoMapper;

namespace Brewdude.Application.Tests.Infrastructure
{
    public static class AutoMapperFactory
    {
        public static IMapper Create()
        {
            var mappingConfig = new MapperConfiguration(configuration => configuration.AddProfile(new MappingProfile()));
            return mappingConfig.CreateMapper();
        }
    }
}