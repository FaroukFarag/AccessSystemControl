using AccessControlSystem.Application.Dtos.Shared;

namespace AccessControlSystem.Application.Interfaces.Abstraction;

public interface IBaseService<TEntity, TEntityDto, TPrimaryKey>
    where TEntity : class
    where TEntityDto : class
{
    Task<TEntityDto> CreateAsync(TEntityDto entityDto);
    Task<TEntityDto> GetAsync(TPrimaryKey id);
    Task<IEnumerable<TEntityDto>> GetAllAsync();
    Task<IEnumerable<TEntityDto>> GetAllPaginatedAsync(PaginatedModelDto paginatedModelDto);
    Task<IEnumerable<TEntityDto>> GetAllFilteredAsync<TFilterDto>(TFilterDto filterDto);
    Task<TEntityDto> Update(TEntityDto newEntityDto);
    Task<TEntityDto> Delete(TPrimaryKey id);
    Task<bool> DeleteRange(IEnumerable<TEntityDto> entitiesDtos);
}
