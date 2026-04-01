using Finlo.Domain.Primitives;
using MediatR;

namespace Finlo.Application.Interfaces.Messaging;

public interface ICommand : IRequest<Result>, IBaseCommand;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand;

public interface IBaseCommand;