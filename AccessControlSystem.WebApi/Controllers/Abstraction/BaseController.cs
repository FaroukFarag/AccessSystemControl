using AccessControlSystem.Application.Dtos.Shared;
using AccessControlSystem.Application.Interfaces.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace AccessControlSystem.WebApi.Controllers.Abstraction;

[Route("api/[controller]")]
[ApiController]
public abstract class BaseController<TService, TEntity, TEntityDto, TPrimaryKey> : ControllerBase
    where TEntity : class
    where TEntityDto : class
    where TService : IBaseService<TEntity, TEntityDto, TPrimaryKey>
{
    private readonly TService _service;

    protected BaseController(TService service)
    {
        _service = service;
    }

    [HttpPost("Create")]
    public virtual async Task<IActionResult> Create(TEntityDto entityDto)
    {
        return Ok(await _service.CreateAsync(entityDto));
    }

    [HttpPost("CreateRange")]
    public virtual async Task<IActionResult> CreateRange(IEnumerable<TEntityDto> entitiesDtos)
    {
        return Ok(await _service.CreateRangeAsync(entitiesDtos));
    }

    [HttpGet("Get")]
    public virtual async Task<IActionResult> Get(TPrimaryKey id)
    {
        var entityDto = await _service.GetAsync(id);

        if (entityDto == null)
            return NotFound();

        return Ok(entityDto);
    }

    [HttpGet("GetAll")]
    public virtual async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpPost("GetAllPaginated")]
    public virtual async Task<IActionResult> GetAllPaginated(PaginatedModelDto paginatedModelDto)
    {
        return Ok(await _service.GetAllPaginatedAsync(paginatedModelDto));
    }

    [HttpPut("Update")]
    public virtual async Task<IActionResult> Update(TEntityDto entityDto)
    {
        return Ok(await _service.UpdateAsync(entityDto));
    }

    [HttpPut("UpdateRange")]
    public virtual async Task<IActionResult> UpdateRange(IEnumerable<TEntityDto> entitiesDtos)
    {
        return Ok(await _service.UpdateRangeAsync(entitiesDtos));
    }

    [HttpDelete("Delete")]
    public virtual async Task<IActionResult> Delete(TPrimaryKey id)
    {
        var entityDto = await _service.DeleteAsync(id);

        if (entityDto == null)
            return NotFound();

        return Ok(entityDto);
    }

    [HttpDelete("DeleteRange")]
    public virtual async Task<IActionResult> DeleteRange(IEnumerable<TEntityDto> entitiesDtos)
    {
        return Ok(await _service.DeleteRangeAsync(entitiesDtos));
    }

    protected int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst("userId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            throw new UnauthorizedAccessException("User ID not found in claims.");
        }

        return int.Parse(userIdClaim);
    }
}
