using Finlo.Application.DTOs.Categories;
using Finlo.Application.DTOs.Common;
using Finlo.Application.Interfaces.Messaging;

namespace Finlo.Application.Features.Categories.Queries.GetAllCategories;

public sealed record GetAllCategoriesQuery(PaginationParams PaginationParams) : IQuery<IEnumerable<CategoryDto>>;