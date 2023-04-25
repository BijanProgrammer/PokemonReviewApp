using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository;

public class ReviewerRepository : IReviewerRepository
{
    private readonly DataContext _context;

    public ReviewerRepository(DataContext context)
    {
        _context = context;
    }

    public ICollection<Reviewer> GetReviewers()
    {
        return _context.Reviewers.OrderBy(reviewer => reviewer.Id).ToList();
    }

    public Reviewer GetReviewer(int reviewerId)
    {
        return _context.Reviewers.FirstOrDefault(reviewer => reviewer.Id == reviewerId);
    }

    public ICollection<Review> GetReviewsByReviewer(int reviewerId)
    {
        return _context.Reviewers
            .Include(reviewer => reviewer.Reviews)
            .FirstOrDefault(reviewer => reviewer.Id == reviewerId)
            .Reviews;
    }

    public bool DoesReviewerExist(int reviewerId)
    {
        return _context.Reviewers.Any(reviewer => reviewer.Id == reviewerId);
    }

    public bool DoesReviewerExist(string reviewerFirstName, string reviewerLastName)
    {
        return _context.Reviewers.Any(
            reviewer =>
                string.Equals(reviewer.FirstName, reviewerFirstName)
                && string.Equals(reviewer.LastName, reviewerLastName)
        );
    }

    public bool CreateReviewer(Reviewer reviewer)
    {
        _context.Add(reviewer);
        return Save();
    }

    public bool Save()
    {
        var changedStatesCount = _context.SaveChanges();
        return changedStatesCount > 0;
    }
}
