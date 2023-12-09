using Microsoft.AspNetCore.Mvc;
using PaymentServiceProvider.Application.Services.Abstractions;
using PaymentServiceProvider.Infrastructure.Dto.Payable;

namespace PaymentServiceProvider.Controllers
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PayablesController : ControllerBase
    {
        private readonly IPayableService _payablesService;

        public PayablesController(IPayableService payablesService)
        {
            _payablesService = payablesService;
        }

        [HttpGet("{cardHolderName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PayableDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetPayablesByCardHolderName(string cardHolderName)
        {
            var payableDto = await _payablesService.GetListByCardHolderNameAsync(cardHolderName);
       
            if (payableDto == null)
            {
                return NoContent();
            }

            return Ok(payableDto);
        }
    }
}