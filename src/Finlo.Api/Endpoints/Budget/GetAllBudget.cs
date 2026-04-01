using Finlo.Application.DTOs.Common;
using Finlo.Application.Features.Budgets.Queries.GetAllBudgets;
using MediatR;

namespace Finlo.Api.Endpoints.Budget;

public class GetAllBudget : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/budgets", async ([AsParameters] PaginationParams paginationParams,ISender sender) =>
        {
            var query = new GetAllBudgetsQuery(paginationParams);

            var result = await sender.Send(query);

            if (result.IsFailure)
                return Results.BadRequest(new { error = result.Error.Code, message = result.Error.Description });

            return Results.Ok(result.Value);
        })
        .WithTags("Budgets")
        .WithName("GetAllBudgets");
    }
}