using AccessControlSystem.Application.Dtos.Shared;
using AccessControlSystem.Application.Interfaces.Abstraction;
using AccessControlSystem.Domain.Interfaces.Repositories.Abstraction;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.Shared;
using AutoMapper;

namespace AccessControlSystem.Application.Services.Abstraction;

public class BaseService<TEntity, TEntityDto, TPrimaryKey>(
    IBaseRepository<TEntity, TPrimaryKey> repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IBaseService<TEntity, TEntityDto, TPrimaryKey>
    where TEntity : class
    where TEntityDto : class
{
    private readonly IBaseRepository<TEntity, TPrimaryKey> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public virtual async Task<ResultDto<TEntityDto>> CreateAsync(TEntityDto entityDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: $"Create {typeof(TEntity).Name}",
            action: async () =>
            {
                var entity = _mapper.Map<TEntity>(entityDto);
                await _repository.CreateAsync(entity);
                await _unitOfWork.Complete();
                return _mapper.Map<TEntityDto>(entity);
            });
    }

    public virtual async Task<ResultDto<bool>> CreateRangeAsync(IEnumerable<TEntityDto> entitiesDtos)
    {
        return await ExecuteServiceCallAsync(
            operationName: $"Create multiple {typeof(TEntity).Name}",
            action: async () =>
            {
                var entities = _mapper.Map<IReadOnlyList<TEntity>>(entitiesDtos);
                await _repository.CreateRangeAsync(entities);
                return await _unitOfWork.Complete();
            });
    }

    public virtual async Task<ResultDto<TEntityDto>> GetAsync(TPrimaryKey id)
    {
        return await ExecuteServiceCallAsync(
            operationName: $"Get {typeof(TEntity).Name} by ID",
            action: async () =>
            {
                var entity = await _repository.GetAsync(id);
                return _mapper.Map<TEntityDto>(entity);
            });
    }

    public virtual async Task<ResultDto<IEnumerable<TEntityDto>>> GetAllAsync()
    {
        return await ExecuteServiceCallAsync(
            operationName: $"Get all {typeof(TEntity).Name}",
            action: async () =>
            {
                var entities = await _repository.GetAllAsync();
                return _mapper.Map<IEnumerable<TEntityDto>>(entities);
            });
    }

    public virtual async Task<ResultDto<IEnumerable<TEntityDto>>> GetAllPaginatedAsync(PaginatedModelDto paginatedModelDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: $"Get paginated {typeof(TEntity).Name}",
            action: async () =>
            {
                var entities = await _repository.GetAllPaginatedAsync(_mapper.Map<PaginatedModel>(paginatedModelDto));
                return _mapper.Map<IEnumerable<TEntityDto>>(entities);
            });
    }

    public virtual async Task<ResultDto<IEnumerable<TEntityDto>>> GetAllFilteredAsync<TFilterDto>(TFilterDto filterDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: $"Get filtered {typeof(TEntity).Name}",
            action: async () =>
            {
                var entities = await _repository.GetAllFilteredAsync(filterDto);
                return _mapper.Map<IEnumerable<TEntityDto>>(entities);
            });
    }

    public virtual async Task<ResultDto<TEntityDto>> UpdateAsync(TEntityDto newEntityDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: $"Update {typeof(TEntity).Name}",
            action: async () =>
            {
                var entity = _mapper.Map<TEntity>(newEntityDto);
                _repository.Update(entity);
                await _unitOfWork.Complete();
                return _mapper.Map<TEntityDto>(entity);
            });
    }

    public virtual async Task<ResultDto<bool>> UpdateRangeAsync(IEnumerable<TEntityDto> entitiesDtos)
    {
        return await ExecuteServiceCallAsync(
            operationName: $"Update multiple {typeof(TEntity).Name}",
            action: async () =>
            {
                var entities = _mapper.Map<IReadOnlyList<TEntity>>(entitiesDtos);
                _repository.UpdateRange(entities);
                return await _unitOfWork.Complete();
            });
    }

    public virtual async Task<ResultDto<TEntityDto>> DeleteAsync(TPrimaryKey id)
    {
        return await ExecuteServiceCallAsync(
            operationName: $"Delete {typeof(TEntity).Name} by ID",
            action: async () =>
            {
                var entity = _repository.Delete(id);
                await _unitOfWork.Complete();
                return _mapper.Map<TEntityDto>(entity);
            });
    }

    public virtual async Task<ResultDto<bool>> DeleteRangeAsync(IEnumerable<TEntityDto> entitiesDtos)
    {
        return await ExecuteServiceCallAsync(
            operationName: $"Delete multiple {typeof(TEntity).Name}",
            action: async () =>
            {
                var entities = _mapper.Map<IReadOnlyList<TEntity>>(entitiesDtos);
                _repository.DeleteRange(entities);
                return await _unitOfWork.Complete();
            });
    }

    protected async Task<ResultDto<T>> ExecuteServiceCallAsync<T>(
        string operationName,
        Func<Task<T>> action)
    {
        try
        {
            var result = await action();

            return ResultDto<T>.CreateSuccessResult(result);
        }

        catch (Exception ex)
        {
            return ResultDto<T>.CreateFailResult($"{operationName} failed: {ex.Message}");
        }
    }
}
