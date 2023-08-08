using CityInfo.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using CityInfo.API.Models.Services;
using AutoMapper;
using CityInfo.API.Models.Entities;

namespace CityInfo.API.Controllers
{
    [Route("api/v{version:ApiVersion}/Cities/{cityId}/pointsofinterest")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly LocalMailService _localMailService;
        private readonly IMapper _mapper;
        private readonly ICityRepository _cityRepository;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
            LocalMailService localMailService, IMapper mapper, ICityRepository cityRepository)
        {
            _logger = logger;
            _localMailService = localMailService;
            _mapper = mapper;
            _cityRepository = cityRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId) 
        {
            if(!await _cityRepository.CityExistsAsync(cityId))
            {
                _logger.LogCritical($"cityId {cityId} is not found");
                return NotFound();
            }
            var PointsOfInterests = await _cityRepository.GetPointsOfInterestAsync(cityId);

            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(PointsOfInterests));
        }

        [HttpGet("{pointofinterestId}", Name = "GetpointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetpointOfInterest(int cityId, int pointofinterestId)
        {
            
            if (!await _cityRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }
            var pointOfInterest = await _cityRepository.GetPointOfInterestAsync(cityId, pointofinterestId);

            if (pointOfInterest == null) 
            { 
                return NotFound(); 
            }
            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
        }

        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(int cityId,
            PointOfInterestForCreationDto pointOfInterest)
        {
           if(!await _cityRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var finalPointOfInterest = _mapper.Map<Models.Entities.PointOfInterest>(pointOfInterest);

            await _cityRepository.AddPointOfInterestToCityAsync(cityId, finalPointOfInterest);

            await _cityRepository.SaveChangesAsync();

            var CreatedPointOfInterest = _mapper.Map<PointOfInterestDto>(finalPointOfInterest);

            return CreatedAtRoute("GetpointOfInterest",
                new
                {
                    cityId = cityId,
                    pointofinterestId = CreatedPointOfInterest.Id

                },
                CreatedPointOfInterest
                );
        }

        [HttpPut("{pointOfInterestId}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId,
            PointOfInterestForUpdateDto pointOfInterestForUpdate)
        {           
            if (!await _cityRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }
            var cityPointOfInterest = await _cityRepository.GetPointOfInterestAsync(cityId, pointOfInterestId);
            _mapper.Map(pointOfInterestForUpdate, cityPointOfInterest);

            await _cityRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{pointOfInterestId}")]
        public async Task<ActionResult> PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId,
           JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            if (!await _cityRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }
            var pointOfInterestEntity = await _cityRepository.GetPointOfInterestAsync(cityId, pointOfInterestId);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if(!ModelState.IsValid) 
            { 
               return BadRequest(ModelState);
            }

            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);

            await _cityRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]
        public async Task<ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)
        {

            
            if (!await _cityRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterestFromEntity = await _cityRepository.GetPointOfInterestAsync(cityId, pointOfInterestId);
            if (pointOfInterestFromEntity == null)
            {
                return NotFound();
            }

            _cityRepository.DeletePointOfInterestFromCity(pointOfInterestFromEntity);
            await _cityRepository.SaveChangesAsync();

            _localMailService.Send("PointOfInterest Deleted", 
                $"Name: {pointOfInterestFromEntity.Name} " + 
                $"Description: {pointOfInterestFromEntity.Name} " +
                $"has been deleted");

            return NoContent();
        }
    }
}
