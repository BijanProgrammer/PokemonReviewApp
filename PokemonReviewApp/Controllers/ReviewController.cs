using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewController : Controller
{
    private readonly IReviewerRepository _reviewerRepository;
    private readonly IPokemonRepository _pokemonRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;

    public ReviewController(
        IReviewerRepository reviewerRepository,
        IPokemonRepository pokemonRepository,
        IReviewRepository reviewRepository,
        IMapper mapper
    )
    {
        _reviewerRepository = reviewerRepository;
        _pokemonRepository = pokemonRepository;
        _reviewRepository = reviewRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
    public IActionResult GetReviews()
    {
        var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(reviews);
    }

    [HttpGet("{reviewId:int}")]
    [ProducesResponseType(200, Type = typeof(ReviewDto))]
    [ProducesResponseType(400)]
    public IActionResult GetReview(int reviewId = 2)
    {
        if (!_reviewRepository.DoesReviewExist(reviewId))
        {
            return NotFound();
        }

        var review = _mapper.Map<ReviewDto>(_reviewRepository.GetReview(reviewId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(review);
    }

    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult CreateReview(
        [FromQuery] int reviewerId,
        [FromQuery] int pokemonId,
        [FromBody] ReviewDto? reviewDto
    )
    {
        if (reviewDto == null)
        {
            return BadRequest(ModelState);
        }

        if (!_reviewerRepository.DoesReviewerExist(reviewerId))
        {
            ModelState.AddModelError("Validation", "Reviewer does not exist");
            return StatusCode(404, ModelState);
        }

        if (!_pokemonRepository.DoesPokemonExist(pokemonId))
        {
            ModelState.AddModelError("Validation", "Pokemon does not exist");
            return StatusCode(404, ModelState);
        }

        if (_reviewRepository.DoesReviewExist(reviewDto.Title))
        {
            ModelState.AddModelError("Validation", "Review already exists");
            return StatusCode(400, ModelState);
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var review = _mapper.Map<Review>(reviewDto);
        if (!_reviewRepository.CreateReview(reviewerId, pokemonId, review))
        {
            ModelState.AddModelError("Unknown", "Something went wrong");
            return StatusCode(500, ModelState);
        }

        return Ok("Created successfully");
    }
}
