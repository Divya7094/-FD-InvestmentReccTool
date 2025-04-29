using Microsoft.AspNetCore.Mvc;
using WisVestAPI.Models.DTOs;
using WisVestAPI.Services.Interfaces;
using System.Threading.Tasks;

namespace WisVestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllocationController : ControllerBase
    {
        private readonly IAllocationService _allocationService;

        public AllocationController(IAllocationService allocationService)
        {
            _allocationService = allocationService;
        }

        // GET: api/Allocation/compute
        [HttpPost("compute")]
        public async Task<ActionResult<AllocationResultDTO>> GetAllocation([FromBody] UserInputDTO input)
        {
            var allocation = await _allocationService.CalculateFinalAllocation(input);

            if (allocation == null)
            {
                return NotFound("Allocation could not be computed.");
            }

            var result = new AllocationResultDTO
            {
                Equity = allocation["equity"],
                FixedIncome = allocation["fixedIncome"],
                Commodities = allocation["commodities"],
                Cash = allocation["cash"],
                RealEstate = allocation["realEstate"]
            };

            return Ok(result);
        }
    }
}
