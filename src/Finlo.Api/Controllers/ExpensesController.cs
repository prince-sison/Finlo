using Finlo.Application.Dtos.Expense;
using Finlo.Application.Features.Expenses.CreateExpense;
using Finlo.Application.Features.Expenses.DeleteExpense;
using Finlo.Application.Features.Expenses.GetAllExpenses;
using Finlo.Application.Features.Expenses.GetExpense;
using Finlo.Application.Features.Expenses.UpdateExpense;
using Microsoft.AspNetCore.Mvc;

namespace Finlo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpensesController(
        CreateExpense createExpense,
        GetExpense getExpense,
        GetAllExpenses getAllExpenses,
        UpdateExpense updateExpense,
        DeleteExpense deleteExpense) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var expenses = await getAllExpenses.HandleAsync(cancellationToken);
            return Ok(expenses);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var expense = await getExpense.HandleAsync(id, cancellationToken);

            if (expense is null)
                return NotFound();

            return Ok(expense);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateExpenseDto dto, CancellationToken cancellationToken)
        {
            var expense = await createExpense.HandleAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = expense.Id }, expense);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateExpenseDto dto, CancellationToken cancellationToken)
        {
            var expense = await updateExpense.HandleAsync(id, dto, cancellationToken);
            return Ok(expense);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await deleteExpense.HandleAsync(id, cancellationToken);
            return NoContent();
        }
    }
}