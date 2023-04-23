﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;

namespace CountryReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountryController : Controller
{
    private readonly ICountryRepository _countryRepository;
    private readonly IMapper _mapper;

    public CountryController(ICountryRepository countryRepository, IMapper mapper)
    {
        _countryRepository = countryRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<CountryDto>))]
    public IActionResult GetCountries()
    {
        var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(countries);
    }

    [HttpGet("{countryId:int}")]
    [ProducesResponseType(200, Type = typeof(CountryDto))]
    [ProducesResponseType(400)]
    public IActionResult GetCountry(int countryId = 2)
    {
        if (!_countryRepository.DoesCountryExist(countryId))
        {
            return NotFound();
        }

        var country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(countryId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(country);
    }

    [HttpGet("owners/{ownerId:int}")]
    [ProducesResponseType(200, Type = typeof(CountryDto))]
    [ProducesResponseType(400)]
    public IActionResult GetCountryByOwner(int ownerId = 2)
    {
        var country = _mapper.Map<CountryDto>(_countryRepository.GetCountryByOwner(ownerId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(country);
    }

    [HttpGet("{countryId:int}/owners")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<OwnerDto>))]
    [ProducesResponseType(400)]
    public IActionResult GetOwnersByCountry(int countryId = 2)
    {
        if (!_countryRepository.DoesCountryExist(countryId))
        {
            return NotFound();
        }

        var owners = _mapper.Map<List<OwnerDto>>(_countryRepository.GetOwnersByCountry(countryId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(owners);
    }
}