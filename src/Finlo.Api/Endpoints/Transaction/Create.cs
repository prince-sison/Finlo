namespace Finlo.Api.Endpoints.Transaction;

public class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/transaction", async () =>
        {
            return Results.Ok();
        });
    }
}