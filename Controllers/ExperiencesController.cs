using AutoMapper;
using BSLTours.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using BSLTours.API.Models;
using BSLTours.API.Models.Dtos;

namespace BSLTours.API.Controllers
{
    [ApiController]
    [Route("api/experiences")]
    public class ExperiencesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IStrapiService _strapiService;
        private readonly ILogger<ExperiencesController> _logger;

        public ExperiencesController(IMapper mapper, IStrapiService strapiService, ILogger<ExperiencesController> logger)
        {
            _mapper = mapper;
            _strapiService = strapiService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<ExperienceDto>>> GetExperiences()
        {
            var rawData = await _strapiService.GetExperiencesAsync();
            var experiences = _mapper.Map<List<ExperienceDto>>(rawData);
            return Ok(experiences);
        }

        [HttpGet("{slug}")]
        public async Task<ActionResult<ExperienceDto>> GetExperienceBySlug(string slug)
        {
            var experience = await _strapiService.GetExperienceBySlugAsync(slug);
            if (experience == null)
                return NotFound();

            return Ok(experience);
        }

        [HttpGet("featured")]
        public async Task<ActionResult<List<ExperienceDto>>> GetFeaturedExperiences()
        {
            var experiences = await _strapiService.GetFeaturedExperiencesAsync();
            return Ok(experiences);
        }

        [HttpGet("featured/card")]
        public async Task<ActionResult<List<SummaryCardDto>>> GetFeaturedDestinationCards()
        {
            var experienceCards = await _strapiService.GetFeaturedCardsAsync("experiences");
            return Ok(experienceCards);
        }
    }

}
