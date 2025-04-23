using AccessControlSystem.Application.Dtos.Abstraction;
using AccessControlSystem.Domain.Models.Abstraction;
using AutoMapper;

namespace AccessControlSystem.Application.AutoMapper.Abstraction;

public class BaseModelProfile : Profile
{
    public BaseModelProfile()
    {
        CreateMap(typeof(BaseModel<>), typeof(BaseModelDto<>)).ReverseMap();
    }
}
