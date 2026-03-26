using Finlo.Application.Features.Transactions.Queries.GetAllTransactions;
using MediatR;

namespace Finlo.Api.Endpoints.Transaction;

public class GetAllTransactions : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/transactions", async (GetAllTransactionsQuery request, ISender sender) =>
        {
            var result = await sender.Send(request);
            return Results.Ok(result);
        })
        .WithName("GetAllTransactions")
        .WithTags("Transactions");
    }
}