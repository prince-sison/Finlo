using Finlo.Application.Features.Budgets.Commands.DeleteBudget;
using MediatR;

namespace Finlo.Api.Endpoints.Budget;

public class DeleteBudget : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/budgets/{id:guid}", async (Guid id, ISender sender) =>
        {
            var command = new DeleteBudgetCommand(id);

            var result = await sender.Send(command);

            if (result.IsFailure)
                return Results.BadRequest(new { error = result.Error.Code, message = result.Error.Description });

            return Results.NoContent();
        })
        .WithTags("Budgets")
        .WithName("DeleteBudget");
    }
}