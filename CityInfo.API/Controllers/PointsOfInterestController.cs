using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/[controller]")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointOfInterestDto> _logger;
        private readonly IMailService _mailservice;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public PointsOfInterestController(ILogger<PointOfInterestDto> logger, IMailService mailService, ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailservice = mailService ?? throw new ArgumentException(nameof(logger));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }

        [HttpGet]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
                if (!_cityInfoRepository.CityExists(cityId))
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found when accesing point of interest.");

                    return NotFound();
                }

                var cityPoi = _cityInfoRepository.GetPointOfInterestForCity(cityId);

                return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(cityPoi));
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Exception while getting point of interest for city with id:{cityId}", ex);

                return StatusCode(500, "A problem happened while handling your request");
            }
        }

        [HttpGet("{id}", Name = "GetPointsOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            if (!_cityInfoRepository.CityExists(cityId)) return NotFound();

            var poi = _cityInfoRepository.GetPointForInterestForCity(cityId, id);
            if (poi == null) return NotFound();

            return Ok(_mapper.Map<PointOfInterestDto>(poi));
        }

        [HttpPost]
        public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if (pointOfInterest.Description == pointOfInterest.Name) ModelState.AddModelError("Description", "The provided description should be different from the name.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_cityInfoRepository.CityExists(cityId)) return NotFound();

            var newpointOfInterest = _mapper.Map<Entities.PointOfInterest>(pointOfInterest);

            _cityInfoRepository.AddPointOfInterestForCity(cityId, newpointOfInterest);
            _cityInfoRepository.Save();

            var newpointOfInterestToReturn = _mapper.Map<Models.PointOfInterestDto>(newpointOfInterest);

            return CreatedAtRoute("GetPointsOfInterest", new { cityId, id = newpointOfInterestToReturn.Id }, newpointOfInterestToReturn);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id, [FromBody] PointOfInterestForUpdateDto updatedpointOfInterest)
        {
            if (updatedpointOfInterest.Description == updatedpointOfInterest.Name) ModelState.AddModelError("Description", "The provided description should be different from the name.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_cityInfoRepository.CityExists(cityId)) return NotFound();

            var entry = _cityInfoRepository.GetPointForInterestForCity(cityId, id);
            if (entry == null) return NotFound();

            _mapper.Map(updatedpointOfInterest, entry);
            _cityInfoRepository.UpdatePointOfInterestForCity(cityId, entry);
            _cityInfoRepository.Save();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PartialUpdatePointOfInterest(int cityId, int id, [FromBody] Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
        {
            if (!_cityInfoRepository.CityExists(cityId)) return NotFound();

            var entry = _cityInfoRepository.GetPointForInterestForCity(cityId, id);
            if (entry == null) return NotFound();

            var entryToPatch = _mapper.Map<PointOfInterestForUpdateDto>(entry);

            patchDoc.ApplyTo(entryToPatch, ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!TryValidateModel(entryToPatch)) return BadRequest();

            _mapper.Map(entryToPatch, entry);
            _cityInfoRepository.UpdatePointOfInterestForCity(cityId, entry);
            _cityInfoRepository.Save();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            if (!_cityInfoRepository.CityExists(cityId)) return NotFound();

            var entry = _cityInfoRepository.GetPointForInterestForCity(cityId, id);
            if (entry == null) return NotFound();

            _cityInfoRepository.DeletePointOfInterest(entry);
            _cityInfoRepository.Save();

            _mailservice.Send("Point of interest deleted", $"Point of interest {entry.Name} with id {entry.Id} was removed!");

            return NoContent();
        }
    }
}