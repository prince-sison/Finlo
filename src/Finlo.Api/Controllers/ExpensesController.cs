using Finlo.Application.Common.Interfaces.Services;
using Finlo.Application.Dtos.Expense;
using Finlo.Application.Features.Expenses.CreateExpense;
using Microsoft.AspNetCore.Mvc;

namespace Finlo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpensesController : ControllerBase
    {
        private readonly CreateExpense _createExpense;

        public ExpensesController(CreateExpense createExpense)
        {
            _createExpense = createExpense;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateExpenseDto dto, CancellationToken cancellationToken)
        {
            await _createExpense.HandleAsync(dto, cancellationToken);
            return Ok();
        }
    }
}