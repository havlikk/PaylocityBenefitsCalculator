using Api.Dtos.Paycheck;
using Api.Interfaces;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PaychecksController : ControllerBase
    {
        private readonly IPaycheckService _paycheckService;

        public PaychecksController(IPaycheckService paycheckService)
        {
            _paycheckService = paycheckService;
        }

        [SwaggerOperation(Summary = "Get paycheck by id")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<GetPaycheckDto>>> Get(int id)
        {
            var paycheck = await _paycheckService.GetByIdAsync(id);

            if (paycheck == null)
            {
                return NotFound();
            }

            var result = new ApiResponse<GetPaycheckDto>
            {
                Data = new GetPaycheckDto(paycheck),
                Success = true
            };

            return result;
        }
    }
}
