using Finlo.Domain.Primitives;
using MediatR;

namespace Finlo.Application.Interfaces.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;