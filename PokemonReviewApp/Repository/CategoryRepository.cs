using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository;

public class CategoryRepository : ICategoryRepository
{
    private readonly DataContext _context;

    public CategoryRepository(DataContext context)
    {
        _context = context;
    }

    public ICollection<Category> GetCategories()
    {
        return _context.Categories.OrderBy(c => c.Id).ToList();
    }

    public Category GetCategory(int id)
    {
        return _context.Categories.FirstOrDefault(c => c.Id == id);
    }

    public ICollection<Pokemon> GetPokemonsByCategory(int id)
    {
        var pokemonCategories = _context.PokemonCategories.Where(pc => pc.CategoryId == id);
        return pokemonCategories.Select(pc => pc.Pokemon).ToList();
    }

    public bool DoesCategoryExist(int id)
    {
        return _context.Categories.Any(c => c.Id == id);
    }
}
