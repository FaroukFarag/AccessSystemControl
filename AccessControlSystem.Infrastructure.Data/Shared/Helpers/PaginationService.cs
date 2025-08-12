using AccessControlSystem.Domain.Models.Shared;
using AccessControlSystem.Domain.Services.Shared;

namespace AccessControlSystem.Infrastructure.Data.Shared.Helpers;

public class PaginationService : IPaginationService
{
    public void ValidatePaginationModel(PaginatedModel model)
    {
        if (model.PageNumber <= 0)
            model.PageNumber = 1;

        if (model.PageSize <= 0)
            throw new ArgumentException("PageSize must be greater than 0");
    }

    public IQueryable<T> ApplyPagination<T>(IQueryable<T> query, PaginatedModel model)
    {
        ValidatePaginationModel(model);
        return query
            .Skip((model.PageNumber - 1) * model.PageSize)
            .Take(model.PageSize);
    }
}
