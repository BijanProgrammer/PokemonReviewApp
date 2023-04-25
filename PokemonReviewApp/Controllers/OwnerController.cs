using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OwnerController : Controller
{
    private readonly ICountryRepository _countryRepository;
    private readonly IOwnerRepository _ownerRepository;
    private readonly IMapper _mapper;

    public OwnerController(
        ICountryRepository countryRepository,
        IOwnerRepository ownerRepository,
        IMapper mapper
    )
    {
        _countryRepository = countryRepository;
        _ownerRepository = ownerRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<OwnerDto>))]
    public IActionResult GetOwners()
    {
        var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(owners);
    }

    [HttpGet("{ownerId:int}")]
    [ProducesResponseType(200, Type = typeof(OwnerDto))]
    [ProducesResponseType(400)]
    public IActionResult GetOwner(int ownerId = 2)
    {
        if (!_ownerRepository.DoesOwnerExist(ownerId))
        {
            return NotFound();
        }

        var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(ownerId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(owner);
    }

    [HttpGet("{ownerId:int}/pokemons")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<PokemonDto>))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemonsByOwner(int ownerId = 2)
    {
        if (!_ownerRepository.DoesOwnerExist(ownerId))
        {
            return NotFound();
        }

        var pokemons = _mapper.Map<List<PokemonDto>>(_ownerRepository.GetPokemonsByOwner(ownerId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(pokemons);
    }

    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult CreateOwner([FromQuery] int countryId, [FromBody] OwnerDto? ownerDto)
    {
        if (ownerDto == null)
        {
            return BadRequest(ModelState);
        }

        if (!_countryRepository.DoesCountryExist(countryId))
        {
            ModelState.AddModelError("Validation", "Country does not exist");
            return StatusCode(404, ModelState);
        }

        if (_ownerRepository.DoesOwnerExist(ownerDto.FirstName, ownerDto.LastName))
        {
            ModelState.AddModelError("Validation", "Owner already exists");
            return BadRequest(ModelState);
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var owner = _mapper.Map<Owner>(ownerDto);
        if (!_ownerRepository.CreateOwner(owner))
        {
            ModelState.AddModelError("Unknown", "Something went wrong");
            return StatusCode(500, ModelState);
        }

        return Ok("Created successfully");
    }

    [HttpPut("{ownerId:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult UpdateOwner(int ownerId, [FromBody] OwnerDto? ownerDto)
    {
        if (ownerDto == null)
        {
            return BadRequest(ModelState);
        }

        if (ownerDto.Id != ownerId)
        {
            ModelState.AddModelError("Validation", "`ownerDto.Id` is not the same as `ownerId`");
            return BadRequest(ModelState);
        }

        if (!_ownerRepository.DoesOwnerExist(ownerDto.FirstName, ownerDto.LastName))
        {
            ModelState.AddModelError("Validation", "Owner does not exist");
            return StatusCode(404, ModelState);
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var owner = _mapper.Map<Owner>(ownerDto);
        if (!_ownerRepository.UpdateOwner(owner))
        {
            ModelState.AddModelError("Unknown", "Something went wrong");
            return StatusCode(500, ModelState);
        }

        return Ok("Updated successfully");
    }
}
