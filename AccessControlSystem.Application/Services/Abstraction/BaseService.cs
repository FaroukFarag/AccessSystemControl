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

    public virtual async Task<TEntityDto> CreateAsync(TEntityDto entityDto)
    {
        var entity = _mapper.Map<TEntity>(entityDto);

        await _repository.CreateAsync(entity);

        await _unitOfWork.Complete();

        return _mapper.Map<TEntityDto>(entity);
    }

    public virtual async Task<bool> CreateRangeAsync(IEnumerable<TEntityDto> entitiesDtos)
    {
        var entities = _mapper.Map<IReadOnlyList<TEntity>>(entitiesDtos);

        await _repository.CreateRangeAsync(entities);

        return await _unitOfWork.Complete();
    }

    public virtual async Task<TEntityDto> GetAsync(TPrimaryKey id)
    {
        var entity = await _repository.GetAsync(id);
        var entityDto = _mapper.Map<TEntityDto>(entity);

        return entityDto;
    }

    public virtual async Task<IEnumerable<TEntityDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        var entitiesDtos = _mapper.Map<IReadOnlyList<TEntityDto>>(entities);

        return entitiesDtos;
    }

    public virtual async Task<IEnumerable<TEntityDto>> GetAllPaginatedAsync(PaginatedModelDto paginatedModelDto)
    {
        var entities = await _repository.GetAllPaginatedAsync(_mapper.Map<PaginatedModel>(paginatedModelDto));
        var entitiesDtos = _mapper.Map<IReadOnlyList<TEntityDto>>(entities);

        return entitiesDtos;
    }

    public virtual async Task<IEnumerable<TEntityDto>> GetAllFilteredAsync<TFilterDto>(TFilterDto filterDto)
    {
        var entities = await _repository.GetAllFilteredAsync<TFilterDto>(filterDto);
        var entitiesDtos = _mapper.Map<IReadOnlyList<TEntityDto>>(entities);

        return entitiesDtos;
    }

    public virtual async Task<TEntityDto> UpdateAsync(TEntityDto newEntityDto)
    {
        var entity = _mapper.Map<TEntity>(newEntityDto);

        _repository.Update(entity);

        await _unitOfWork.Complete();

        return _mapper.Map<TEntityDto>(entity);
    }

    public virtual async Task<bool> UpdateRangeAsync(IEnumerable<TEntityDto> entitiesDtos)
    {
        var entities = _mapper.Map<IReadOnlyList<TEntity>>(entitiesDtos);

        _repository.UpdateRange(entities);

        return await _unitOfWork.Complete();
    }

    public virtual async Task<TEntityDto> DeleteAsync(TPrimaryKey id)
    {
        var entity = _repository.Delete(id);
        var entityDto = _mapper.Map<TEntityDto>(entity);

        await _unitOfWork.Complete();

        return entityDto;
    }

    public async virtual Task<bool> DeleteRangeAsync(IEnumerable<TEntityDto> entitiesDtos)
    {
        var entities = _mapper.Map<IReadOnlyList<TEntity>>(entitiesDtos);

        _repository.DeleteRange(entities);

        return await _unitOfWork.Complete();
    }
}
