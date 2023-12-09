using Microsoft.AspNetCore.Mvc;
using PaymentServiceProvider.Application.Services.Abstractions;
using PaymentServiceProvider.Infrastructure.Dto.Transaction;

namespace PaymentServiceProvider.Controllers
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionsService;

        public TransactionsController(ITransactionService transactionsService)
        {
            _transactionsService = transactionsService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateTransactionAsync([FromBody] CreateTransactionDto request)
        {
            await _transactionsService.CreateTransactionAsync(request);
            return StatusCode(201);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TransactionDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllTransactionsAsync()
        {
            var transactions = await _transactionsService.GetAllTransactionsAsync();

            if (transactions == null || !transactions.Any())
            {
                return NoContent();
            }

            return Ok(transactions);
        }
    }
}