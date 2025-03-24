using AccessControlSystem.Application.Dtos.Users;
using AccessControlSystem.Domain.Models.Users;
using AutoMapper;

namespace AccessControlSystem.Application.AutoMapper.Users;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
    }
}
