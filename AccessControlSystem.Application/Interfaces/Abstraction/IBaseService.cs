using AccessControlSystem.Application.Dtos.Shared;

namespace AccessControlSystem.Application.Interfaces.Abstraction;

public interface IBaseService<TEntity, TEntityDto, TPrimaryKey>
    where TEntity : class
    where TEntityDto : class
{
    Task<TEntityDto> CreateAsync(TEntityDto entityDto);
    Task<bool> CreateRangeAsync(IEnumerable<TEntityDto> entitiesDtos);
    Task<TEntityDto> GetAsync(TPrimaryKey id);
    Task<IEnumerable<TEntityDto>> GetAllAsync();
    Task<IEnumerable<TEntityDto>> GetAllPaginatedAsync(PaginatedModelDto paginatedModelDto);
    Task<IEnumerable<TEntityDto>> GetAllFilteredAsync<TFilterDto>(TFilterDto filterDto);
    Task<TEntityDto> UpdateAsync(TEntityDto newEntityDto);
    Task<bool> UpdateRangeAsync(IEnumerable<TEntityDto> entitiesDtos);
    Task<TEntityDto> DeleteAsync(TPrimaryKey id);
    Task<bool> DeleteRangeAsync(IEnumerable<TEntityDto> entitiesDtos);
}
