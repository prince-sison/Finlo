using Finlo.Application.DTOs.Budgets;
using Finlo.Application.Features.Budgets.Commands.CreateBudget;
using MediatR;

namespace Finlo.Api.Endpoints.Budget;

public class CreateBudget : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/budgets", async (CreateBudgetDto request, ISender sender) =>
        {
            var command = new CreateBudgetCommand(
                request.Category,
                request.Limit,
                request.Month,
                request.Year);

            var result = await sender.Send(command);

            if (result.IsFailure)
                return Results.BadRequest(new { error = result.Error.Code, message = result.Error.Description });

            return Results.Created($"/api/budgets/{result.Value.Id}", result.Value);
        })
        .WithTags("Budgets")
        .WithName("CreateBudget");
    }
}