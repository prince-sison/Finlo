using Finlo.Application.Features.Transactions.Commands.DeleteTransaction;
using MediatR;

namespace Finlo.Api.Endpoints.Transaction;

public class DeleteTransaction : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/transactions/{id:guid}", async (Guid id, ISender sender) =>
        {
            var command = new DeleteTransactionCommand(id);
            var result = await sender.Send(command);

            if (result.IsFailure)
                return Results.BadRequest(new { error = result.Error.Code, message = result.Error.Description });

            return Results.NoContent();
        })
        .WithTags("Transactions")
        .WithName("DeleteTransaction");
    }
}