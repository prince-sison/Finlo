using Finlo.Application.DTOs.Common;
using Finlo.Application.Features.Categories.Queries.GetAllCategories;
using MediatR;

namespace Finlo.Api.Endpoints.Category;

public class GetAllCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/categories", async ([AsParameters] PaginationParams paginationParams, ISender sender) =>
        {
            var query = new GetAllCategoriesQuery(paginationParams);

            var result = await sender.Send(query);

            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        })
        .WithTags("Categories")
        .WithName("GetAllCategories");
    }
}