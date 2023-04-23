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
}
