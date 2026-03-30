using Finlo.Application.Interfaces;
using Finlo.Application.Interfaces.Budgets;
using Finlo.Application.Interfaces.Messaging;
using Finlo.Domain.Primitives;

namespace Finlo.Application.Features.Budgets.Commands.DeleteBudget;

internal sealed class DeleteBudgetCommandHandler : ICommandHandler<DeleteBudgetCommand>
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBudgetCommandHandler(IBudgetRepository budgetRepository, IUnitOfWork unitOfWork)
    {
        _budgetRepository = budgetRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task<Result> Handle(DeleteBudgetCommand request, CancellationToken cancellationToken)
    {
        var budget = await _budgetRepository.GetByIdAsync(request.Id, cancellationToken);

        if (budget is null)
        {
            return Result.Failure(Error.NotFound("Budget.NotFound", "Budget not found."));
        }

        await _budgetRepository.RemoveAsync(budget, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}