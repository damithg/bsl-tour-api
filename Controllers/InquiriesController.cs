using System.Collections.Generic;
using System.Threading.Tasks;
using BSLTours.API.Models;
using BSLTours.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BSLTours.API.Controllers
{
    [ApiController]
    [Route("api/inquiries")]
    public class InquiriesController : ControllerBase
    {
        private readonly IDataService _dataService;

        public InquiriesController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inquiry>>> GetAllInquiries()
        {
            var inquiries = await _dataService.GetInquiriesAsync();
            return Ok(inquiries);
        }

        [HttpPost]
        public async Task<ActionResult<Inquiry>> CreateInquiry(CreateInquiryDto inquiryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var inquiry = await _dataService.CreateInquiryAsync(inquiryDto);
            return CreatedAtAction(nameof(GetAllInquiries), null, inquiry);
        }
    }
}