using Finlo.Application.Features.Budgets.Queries.GetBudgetById;
using MediatR;

namespace Finlo.Api.Endpoints.Budget;

public class GetBudgetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/budgets/{id:guid}", async (Guid id, ISender sender) =>
        {
            var query = new GetBudgetByIdQuery(id);

            var result = await sender.Send(query);

            if (result.IsFailure)
                return Results.BadRequest(new { error = result.Error.Code, message = result.Error.Description });

            return Results.Ok(result.Value);
        })
        .WithTags("Budgets")
        .WithName("GetBudgetById");
    }
}