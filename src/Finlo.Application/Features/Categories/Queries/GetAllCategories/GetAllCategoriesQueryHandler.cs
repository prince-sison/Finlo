using Finlo.Application.DTOs.Categories;
using Finlo.Application.DTOs.Common;
using Finlo.Application.Interfaces.Categories;
using Finlo.Application.Interfaces.Messaging;
using Finlo.Domain.Primitives;

namespace Finlo.Application.Features.Categories.Queries.GetAllCategories;

internal sealed class GetAllCategoriesQueryHandler : IQueryHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<IEnumerable<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var pagedResult = await _categoryRepository.GetAllAsync(new PaginationParams(), cancellationToken);
        var categoryDtos = pagedResult.Items.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            TransactionType = c.Type.ToString()
        }).ToList();

        return Result.Success<IEnumerable<CategoryDto>>(categoryDtos);
    }
}