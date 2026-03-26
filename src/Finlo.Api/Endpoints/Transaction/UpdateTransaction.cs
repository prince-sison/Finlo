using Finlo.Application.DTOs.Transactions;
using Finlo.Application.Features.Transactions.Commands.UpdateTransaction;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Finlo.Api.Endpoints.Transaction;

public class UpdateTransaction : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/transactions/{id:guid}", async (Guid id, [FromBody] UpdateTransactionDto request, ISender sender) =>
        {
            var command = new UpdateTransactionCommand(
                id,
                request.Amount,
                request.Type,
                request.Category,
                request.Date,
                request.Notes);

            var result = await sender.Send(command);

            if (result.IsFailure)
                return Results.BadRequest(new { error = result.Error.Code, message = result.Error.Description });

            return Results.NoContent();
        })
        .WithTags("Transactions")
        .WithName("UpdateTransaction");
    }
}