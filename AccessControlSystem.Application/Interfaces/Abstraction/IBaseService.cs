using AccessControlSystem.Application.Dtos.Shared;
using AccessControlSystem.Application.Services.Shared;

namespace AccessControlSystem.Application.Interfaces.Abstraction;

public interface IBaseService<TEntity, TEntityDto, TPrimaryKey>
    where TEntity : class
    where TEntityDto : class
{
    Task<ResultDto<TEntityDto>> CreateAsync(TEntityDto entityDto);
    Task<ResultDto<bool>> CreateRangeAsync(IEnumerable<TEntityDto> entitiesDtos);
    Task<ResultDto<TEntityDto>> GetAsync(TPrimaryKey id);
    Task<ResultDto<IEnumerable<TEntityDto>>> GetAllAsync();
    Task<ResultDto<IEnumerable<TEntityDto>>> GetAllPaginatedAsync(PaginatedModelDto paginatedModelDto);
    Task<ResultDto<IEnumerable<TEntityDto>>> GetAllFilteredAsync<TFilterDto>(TFilterDto filterDto);
    Task<ResultDto<TEntityDto>> UpdateAsync(TEntityDto newEntityDto);
    Task<ResultDto<bool>> UpdateRangeAsync(IEnumerable<TEntityDto> entitiesDtos);
    Task<ResultDto<TEntityDto>> DeleteAsync(TPrimaryKey id);
    Task<ResultDto<bool>> DeleteRangeAsync(IEnumerable<TEntityDto> entitiesDtos);
}
