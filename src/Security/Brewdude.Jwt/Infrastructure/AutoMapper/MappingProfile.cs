using AutoMapper;
using Brewdude.Jwt.Models;

namespace Brewdude.Jwt.Infrastructure.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDto, UserViewModel>();
            CreateMap<UserViewModel, UserDto>();
        }
    }
}