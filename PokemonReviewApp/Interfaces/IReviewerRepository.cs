using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces;

public interface IReviewerRepository
{
    ICollection<Reviewer> GetReviewers();
    Reviewer GetReviewer(int reviewerId);
    ICollection<Review> GetReviewsByReviewer(int reviewerId);
    bool DoesReviewerExist(int reviewerId);
    bool DoesReviewerExist(string reviewerFirstName, string reviewerLastName);
    bool CreateReviewer(Reviewer reviewer);
    bool Save();
}
