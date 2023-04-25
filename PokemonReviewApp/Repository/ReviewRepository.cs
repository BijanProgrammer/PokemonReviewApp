using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository;

public class ReviewRepository : IReviewRepository
{
    private readonly DataContext _context;

    public ReviewRepository(DataContext context)
    {
        _context = context;
    }

    public ICollection<Review> GetReviews()
    {
        return _context.Reviews.OrderBy(review => review.Id).ToList();
    }

    public Review GetReview(int reviewId)
    {
        return _context.Reviews.FirstOrDefault(review => review.Id == reviewId);
    }

    public bool DoesReviewExist(int reviewId)
    {
        return _context.Reviews.Any(review => review.Id == reviewId);
    }

    public bool DoesReviewExist(string title)
    {
        return _context.Reviews.Any(review => string.Equals(review.Title, title));
    }

    public bool CreateReview(int reviewerId, int pokemonId, Review review)
    {
        var reviewer = _context.Reviewers.FirstOrDefault(reviewer => reviewer.Id == reviewerId);
        var pokemon = _context.Pokemons.FirstOrDefault(pokemon => pokemon.Id == pokemonId);

        review.Reviewer = reviewer;
        review.Pokemon = pokemon;

        _context.Add(review);

        return Save();
    }

    public bool Save()
    {
        var changedStatesCount = _context.SaveChanges();
        return changedStatesCount > 0;
    }
}
