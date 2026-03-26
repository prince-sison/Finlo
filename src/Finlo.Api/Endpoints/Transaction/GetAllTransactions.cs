using Finlo.Application.DTOs.Common;
using Finlo.Application.Features.Transactions.Queries.GetAllTransactions;
using MediatR;

namespace Finlo.Api.Endpoints.Transaction;

public class GetAllTransactions : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/transactions", async ([AsParameters] PaginationParams paginationParams, ISender sender) =>
        {
            var query = new GetAllTransactionsQuery(paginationParams);
            var result = await sender.Send(query);
            return Results.Ok(result);
        })
        .WithName("GetAllTransactions")
        .WithTags("Transactions");
    }
}