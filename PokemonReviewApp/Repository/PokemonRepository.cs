using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository;

public class PokemonRepository : IPokemonRepository
{
    private readonly DataContext _context;

    public PokemonRepository(DataContext context)
    {
        _context = context;
    }

    public ICollection<Pokemon> GetPokemons()
    {
        return _context.Pokemons.OrderBy(p => p.Id).ToList();
    }

    public Pokemon GetPokemon(int id)
    {
        return _context.Pokemons.FirstOrDefault(p => p.Id == id);
    }

    public Pokemon GetPokemon(string name)
    {
        return _context.Pokemons.FirstOrDefault(p => p.Name == name);
    }

    public decimal GetPokemonRating(int id)
    {
        var reviews = _context.Reviews.Where(r => r.Pokemon.Id == id);

        if (!reviews.Any())
        {
            return 0;
        }

        return reviews.Sum(r => r.Rating) / reviews.Count();
    }

    public bool DoesPokemonExist(int id)
    {
        return _context.Pokemons.Any(p => p.Id == id);
    }
}
