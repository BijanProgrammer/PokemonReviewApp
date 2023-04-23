using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PokemonController : Controller
{
    private readonly IPokemonRepository _pokemonRepository;
    private readonly IMapper _mapper;

    public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper)
    {
        _pokemonRepository = pokemonRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<PokemonDto>))]
    public IActionResult GetPokemons()
    {
        var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(pokemons);
    }

    [HttpGet("{pokemonId:int}")]
    [ProducesResponseType(200, Type = typeof(PokemonDto))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemon(int pokemonId = 2)
    {
        if (!_pokemonRepository.DoesPokemonExist(pokemonId))
        {
            return NotFound();
        }

        var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokemonId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(pokemon);
    }

    [HttpGet("{pokemonName}")]
    [ProducesResponseType(200, Type = typeof(PokemonDto))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemon(string pokemonName = "Squirtle")
    {
        var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokemonName));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(pokemon);
    }

    [HttpGet("{pokemonId:int}/rating")]
    [ProducesResponseType(200, Type = typeof(decimal))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemonRating(int pokemonId = 2)
    {
        if (!_pokemonRepository.DoesPokemonExist(pokemonId))
        {
            return NotFound();
        }

        var rating = _pokemonRepository.GetPokemonRating(pokemonId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(rating);
    }
}
