using System.Threading.Tasks;
using BSLTours.API.Models;
using BSLTours.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BSLTours.API.Controllers
{
    [ApiController]
    [Route("api/subscribers")]
    public class SubscribersController : ControllerBase
    {
        private readonly IDataService _dataService;

        public SubscribersController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpPost]
        public async Task<ActionResult<Subscriber>> AddSubscriber(CreateSubscriberDto subscriberDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the email already exists
            var existingSubscriber = await _dataService.GetSubscriberByEmailAsync(subscriberDto.Email);
            if (existingSubscriber != null)
            {
                return Conflict(new { message = "This email is already subscribed to our newsletter." });
            }

            var subscriber = await _dataService.AddSubscriberAsync(subscriberDto);
            return StatusCode(StatusCodes.Status201Created, subscriber);
        }
    }
}