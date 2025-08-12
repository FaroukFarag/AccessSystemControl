using AccessControlSystem.Domain.Models.Shared;

namespace AccessControlSystem.Domain.Services.Shared;

public interface IPaginationService
{
    void ValidatePaginationModel(PaginatedModel model);
    IQueryable<T> ApplyPagination<T>(IQueryable<T> query, PaginatedModel model);
}
