using Finlo.Application.Features.Transactions.Queries.GetTransactionById;
using MediatR;

namespace Finlo.Api.Endpoints.Transaction;

public class GetTransactionById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/transactions/{id:guid}", async (Guid id, ISender sender) =>
        {
            var query = new GetTransactionByIdQuery(id);
            var result = await sender.Send(query);

            if (result.IsFailure)
                return Results.BadRequest(new { error = result.Error.Code, message = result.Error.Description });

            return Results.Ok(result.Value);
        })
        .WithTags("Transactions")
        .WithName("GetTransactionById");
    }
}