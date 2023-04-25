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
    private readonly IOwnerRepository _ownerRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IPokemonRepository _pokemonRepository;
    private readonly IMapper _mapper;

    public PokemonController(
        IOwnerRepository ownerRepository,
        ICategoryRepository categoryRepository,
        IPokemonRepository pokemonRepository,
        IMapper mapper
    )
    {
        _ownerRepository = ownerRepository;
        _categoryRepository = categoryRepository;
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

    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult CreatePokemon(
        [FromQuery] int ownerId,
        [FromQuery] int categoryId,
        [FromBody] PokemonDto? pokemonDto
    )
    {
        if (pokemonDto == null)
        {
            return BadRequest(ModelState);
        }

        if (!_ownerRepository.DoesOwnerExist(ownerId))
        {
            ModelState.AddModelError("Validation", "Owner does not exist");
            return StatusCode(404, ModelState);
        }

        if (!_categoryRepository.DoesCategoryExist(categoryId))
        {
            ModelState.AddModelError("Validation", "Category does not exist");
            return StatusCode(404, ModelState);
        }

        if (_pokemonRepository.DoesPokemonExist(pokemonDto.Name))
        {
            ModelState.AddModelError("Validation", "Pokemon already exists");
            return StatusCode(400, ModelState);
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var pokemon = _mapper.Map<Pokemon>(pokemonDto);
        if (!_pokemonRepository.CreatePokemon(ownerId, categoryId, pokemon))
        {
            ModelState.AddModelError("Unknown", "Something went wrong");
            return StatusCode(500, ModelState);
        }

        return Ok("Created successfully");
    }
}
