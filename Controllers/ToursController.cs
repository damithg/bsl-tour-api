using AutoMapper;
using BSLTours.API.Models;
using BSLTours.API.Models.Dtos;
using BSLTours.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BSLTours.API.Controllers
{
    [ApiController]
    [Route("api/tours")]
    public class ToursController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IStrapiService _strapiService;
        private readonly ILogger<ToursController> _logger;

        public ToursController(IMapper mapper, IStrapiService strapiService, ILogger<ToursController> logger)
        {
            _mapper = mapper;
            _strapiService = strapiService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<TourDto>>> GetTours()
        {
            var rawData = await _strapiService.GetToursAsync();
            var tours = _mapper.Map<List<TourDto>>(rawData);
            return Ok(tours);
        }

        [HttpGet("{slug}")]
        public async Task<ActionResult<TourDto>> GetTourBySlug(string slug)
        {
            var tour = await _strapiService.GetTourBySlugAsync(slug);
            if (tour == null)
                return NotFound();

            return Ok(tour);
        }

        [HttpGet("featured")]
        public async Task<ActionResult<List<TourDto>>> GetFeaturedTours()
        {
            var tours = await _strapiService.GetFeaturedToursAsync();
            return Ok(tours);
        }

        [HttpGet("card")]
        public async Task<ActionResult<List<TourSummaryCardDto>>> GetAllTourCards()
        {
            var tourCards = await _strapiService.GetFeaturedCardsAsync("tours", false);
            return Ok(tourCards);
        }

        [HttpGet("card/featured")]
        public async Task<ActionResult<List<TourSummaryCardDto>>> GetFeaturedTourCards()
        {
            var tourCards = await _strapiService.GetFeaturedCardsAsync("tours", true);
            return Ok(tourCards);
        }


        //[HttpPost]
        //public async Task<ActionResult<TourPackage>> CreateTourPackage(CreateTourPackageDto tourPackageDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var tourPackage = await _dataService.CreateTourPackageAsync(tourPackageDto);
        //    return CreatedAtAction(nameof(GetTourPackageById), new { id = tourPackage.Id }, tourPackage);
        //}
    }
}