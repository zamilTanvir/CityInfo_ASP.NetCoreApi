using AutoMapper;
using CityInfo.API.DbContexts;
using CityInfo.API.Models;
using CityInfo.API.Models.Entities;
using CityInfo.API.Models.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/cities")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class CitesController : ControllerBase
    {
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;

        public CitesController(ICityRepository cityRepository, IMapper mapper)
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> GetCities(string? name,
            string? searchQuery)
        {
            var cityEntity = await _cityRepository.GetCitiesAsync(name, searchQuery);
            return Ok(_mapper.Map<IEnumerable<CityWithoutPointOfInterestDto>>(cityEntity));
    }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCity(int id, bool pointOfInterest) 
        {
            var City = await _cityRepository.GetCityAsync(id, pointOfInterest);

            if(City == null)
            {
                return NotFound();
            }
            if (pointOfInterest)
            {               
                return Ok(_mapper.Map<CityDto>(City));
            }

            return Ok(_mapper.Map<CityWithoutPointOfInterestDto>(City));
        }
    }
}
