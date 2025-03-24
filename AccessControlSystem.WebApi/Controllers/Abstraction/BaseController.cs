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
    public virtual async Task<IActionResult> Create(TEntityDto dto)
    {
        return Ok(await _service.CreateAsync(dto));
    }

    [HttpGet("Get")]
    public virtual async Task<IActionResult> Get(TPrimaryKey id)
    {
        var dto = await _service.GetAsync(id);

        if (dto == null)
            return NotFound();

        return Ok(dto);
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
    public virtual async Task<IActionResult> Update(TEntityDto dto)
    {
        return Ok(await _service.Update(dto));
    }

    [HttpDelete("Delete")]
    public virtual async Task<IActionResult> Delete(TPrimaryKey id)
    {
        var dto = await _service.Delete(id);

        if (dto == null)
            return NotFound();

        return Ok(dto);
    }

    [HttpDelete("DeleteRange")]
    public virtual async Task<IActionResult> DeleteRange(IEnumerable<TEntityDto> dtos)
    {
        return Ok(await _service.DeleteRange(dtos));
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
