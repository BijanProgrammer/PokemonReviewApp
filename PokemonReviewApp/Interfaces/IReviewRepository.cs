using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces;

public interface IReviewRepository
{
    ICollection<Review> GetReviews();
    Review GetReview(int reviewId);
    bool DoesReviewExist(int reviewId);
    bool DoesReviewExist(string title);
    bool CreateReview(int reviewerId, int pokemonId, Review review);
    bool Save();
}
