using System.Collections.Generic;
using System.Threading.Tasks;
using BSLTours.API.Models;
using BSLTours.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BSLTours.API.Controllers
{
    [ApiController]
    [Route("api/testimonials")]
    public class TestimonialsController : ControllerBase
    {
        private readonly IDataService _dataService;

        public TestimonialsController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Testimonial>>> GetAllTestimonials()
        {
            var testimonials = await _dataService.GetTestimonialsAsync();
            return Ok(testimonials);
        }

        [HttpPost]
        public async Task<ActionResult<Testimonial>> CreateTestimonial(CreateTestimonialDto testimonialDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var testimonial = await _dataService.CreateTestimonialAsync(testimonialDto);
            return CreatedAtAction(nameof(GetAllTestimonials), null, testimonial);
        }
    }
}