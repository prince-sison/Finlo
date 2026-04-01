using Finlo.Application.Features.Transactions.Queries.GetTransactionById;
using Finlo.Domain.Enums;
using MediatR;

namespace Finlo.Api.Endpoints.Transaction;

public class GetTransactionById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/transactions/{id:guid}", async (Guid id, ISender sender) =>
        {
            var query = new GetTransactionByIdQuery(id);
            var result = await sender.Send(query);

            if (result.IsFailure)
                return result.Error.Type switch
                {
                    ErrorType.NotFound => Results.NotFound(new { error = result.Error.Code, message = result.Error.Description }),
                    ErrorType.Conflict => Results.Conflict(new { error = result.Error.Code, message = result.Error.Description }),
                    ErrorType.Validation => Results.BadRequest(new { error = result.Error.Code, message = result.Error.Description }),
                    _ => Results.Problem(detail: result.Error.Description, title: result.Error.Code)
                };

            return Results.Ok(result.Value);
        })
        .WithTags("Transactions")
        .WithName("GetTransactionById");
    }
}