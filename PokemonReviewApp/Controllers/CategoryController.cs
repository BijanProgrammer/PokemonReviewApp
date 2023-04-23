﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;

namespace CategoryReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : Controller
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDto>))]
    public IActionResult GetCategories()
    {
        var categories = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories());

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(categories);
    }

    [HttpGet("{categoryId:int}")]
    [ProducesResponseType(200, Type = typeof(CategoryDto))]
    [ProducesResponseType(400)]
    public IActionResult GetCategory(int categoryId = 2)
    {
        if (!_categoryRepository.DoesCategoryExist(categoryId))
        {
            return NotFound();
        }

        var category = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(categoryId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(category);
    }

    [HttpGet("{categoryId:int}/pokemons")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<PokemonDto>))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemonsByCategory(int categoryId = 2)
    {
        if (!_categoryRepository.DoesCategoryExist(categoryId))
        {
            return NotFound();
        }

        var pokemons = _mapper.Map<List<PokemonDto>>(_categoryRepository.GetPokemonsByCategory(categoryId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(pokemons);
    }
}
