using Finlo.Domain.Primitives;
using MediatR;

namespace Finlo.Application.Interfaces.Messaging;

public interface IQueryHandler<in TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;