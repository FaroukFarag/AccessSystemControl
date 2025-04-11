using AccessControlSystem.Application.Dtos.Shared;
using AccessControlSystem.Domain.Models.Shared;
using AutoMapper;

namespace AccessControlSystem.Application.AutoMapper.Shared;

public class PaginatedModelProfile : Profile
{
    public PaginatedModelProfile()
    {
        CreateMap<PaginatedModel, PaginatedModelDto>().ReverseMap();
    }
}
