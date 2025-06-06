using AutoMapper;
using BSLTours.API.Models;
using BSLTours.API.Models.Dtos;
using BSLTours.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BSLTours.API.Controllers
{
    [ApiController]
    [Route("api/destinations")]
    public class DestinationsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IStrapiService _strapiService;

        public DestinationsController(IMapper mapper, IStrapiService strapiService)
        {
            _mapper = mapper;
            _strapiService = strapiService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rawData = await _strapiService.GetDestinationsAsync();
            var mapped = _mapper.Map<List<DestinationDto>>(rawData);
            return Ok(mapped);
        }

        [HttpGet("featured")]
        public async Task<ActionResult<List<DestinationDto>>> GetFeaturedDestinations()
        {
            var destinations = await _strapiService.GetFeaturedDestinationsAsync();
            return Ok(destinations);
        }

        [HttpGet("featured/card")]
        public async Task<ActionResult<List<DestinationCardDto>>> GetFeaturedDestinationCards()
        {
            var destinationCards = await _strapiService.GetFeaturedDestinationCardsAsync();
            return Ok(destinationCards);
        }


        [HttpGet("{slug}")]
        public async Task<ActionResult<DestinationDto>> GetDestinationBySlug(string slug)
        {
            var destination = await _strapiService.GetDestinationBySlugAsync(slug);

            if (destination == null)
            {
                return NotFound();
            }

            return Ok(destination);
        }
    }
}
