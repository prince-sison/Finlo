using Finlo.Application.DTOs.Transactions;
using Finlo.Application.Features.Transactions.Commands.CreateTransaction;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Finlo.Api.Endpoints.Transaction;

public class CreateTransaction : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/transactions", async ([FromBody]CreateTransactionDto request, ISender sender) =>
        {
            var command = new CreateTransactionCommand(
                request.Amount,
                request.Type,
                request.Category,
                request.Date,
                request.Notes);

            var result = await sender.Send(command);

            if (result.IsFailure)
                return Results.BadRequest(new { error = result.Error.Code, message = result.Error.Description });

            return Results.Created($"/api/transactions/{result.Value.Id}", result.Value);
        })
        .WithTags("Transactions")
        .WithName("CreateTransaction");
    }
}