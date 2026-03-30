using Finlo.Application.Features.Budgets.Queries.GetBudgetSummary;
using MediatR;

namespace Finlo.Api.Endpoints.Budget;

public class GetBudgetSummary : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/budgets/summary", async (int month, int year, ISender sender) =>
        {
            var query = new GetBudgetSummaryQuery(month, year);
            var result = await sender.Send(query);

            if (result.IsFailure)
                return Results.BadRequest(new { error = result.Error.Code, message = result.Error.Description });

            return Results.Ok(result.Value);
        })
        .WithTags("Budgets")
        .WithName("GetBudgetSummary");
    }
}
