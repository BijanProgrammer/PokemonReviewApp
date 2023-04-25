using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewerController : Controller
{
    private readonly IReviewerRepository _reviewerRepository;
    private readonly IMapper _mapper;

    public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
    {
        _reviewerRepository = reviewerRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDto>))]
    public IActionResult GetReviewers()
    {
        var reviewers = _mapper.Map<List<ReviewerDto>>(_reviewerRepository.GetReviewers());

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(reviewers);
    }

    [HttpGet("{reviewerId:int}")]
    [ProducesResponseType(200, Type = typeof(ReviewerDto))]
    [ProducesResponseType(400)]
    public IActionResult GetReviewer(int reviewerId = 2)
    {
        if (!_reviewerRepository.DoesReviewerExist(reviewerId))
        {
            return NotFound();
        }

        var reviewer = _mapper.Map<ReviewerDto>(_reviewerRepository.GetReviewer(reviewerId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(reviewer);
    }

    [HttpGet("{reviewerId:int}/reviews")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
    [ProducesResponseType(400)]
    public IActionResult GetReviewsByReviewer(int reviewerId = 2)
    {
        if (!_reviewerRepository.DoesReviewerExist(reviewerId))
        {
            return NotFound();
        }

        var reviews = _mapper.Map<List<ReviewDto>>(
            _reviewerRepository.GetReviewsByReviewer(reviewerId)
        );

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(reviews);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateReviewer([FromBody] ReviewerDto? reviewerDto)
    {
        if (reviewerDto == null)
        {
            return BadRequest(ModelState);
        }

        if (_reviewerRepository.DoesReviewerExist(reviewerDto.FirstName, reviewerDto.LastName))
        {
            ModelState.AddModelError("Validation", "Reviewer already exists");
            return BadRequest(ModelState);
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var reviewer = _mapper.Map<Reviewer>(reviewerDto);
        if (!_reviewerRepository.CreateReviewer(reviewer))
        {
            ModelState.AddModelError("Unknown", "Something went wrong");
            return StatusCode(500, ModelState);
        }

        return Ok("Created successfully");
    }
}
