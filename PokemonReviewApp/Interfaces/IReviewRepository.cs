using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces;

public interface IReviewRepository
{
    ICollection<Review> GetReviews();
    Review GetReview(int reviewId);
    bool DoesReviewExist(int reviewId);
}
