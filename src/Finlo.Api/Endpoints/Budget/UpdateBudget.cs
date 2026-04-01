using Finlo.Application.DTOs.Budgets;
using Finlo.Application.Features.Budgets.Commands.UpdateBudget;
using MediatR;

namespace Finlo.Api.Endpoints.Budget;

public class UpdateBudget : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/budgets/{id:guid}", async (Guid id, UpdateBudgetDto request, ISender sender) =>
        {
            var command = new UpdateBudgetCommand(
                id,
                request.Category,
                request.Limit,
                request.Month,
                request.Year);

            var result = await sender.Send(command);

            if (result.IsFailure)
                return Results.BadRequest(new { error = result.Error.Code, message = result.Error.Description });

            return Results.Ok(result.Value);
        })
        .WithTags("Budgets")
        .WithName("UpdateBudget");
    }
}