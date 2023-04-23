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
        return _context.Pokemons.OrderBy(pokemon => pokemon.Id).ToList();
    }

    public Pokemon GetPokemon(int pokemonId)
    {
        return _context.Pokemons.FirstOrDefault(pokemon => pokemon.Id == pokemonId);
    }

    public Pokemon GetPokemon(string pokemonName)
    {
        return _context.Pokemons.FirstOrDefault(pokemon => pokemon.Name == pokemonName);
    }

    public decimal GetPokemonRating(int pokemonId)
    {
        var reviews = _context.Reviews.Where(review => review.Pokemon.Id == pokemonId);

        if (!reviews.Any())
        {
            return 0;
        }

        return reviews.Sum(review => review.Rating) / reviews.Count();
    }

    public bool DoesPokemonExist(int pokemonId)
    {
        return _context.Pokemons.Any(pokemon => pokemon.Id == pokemonId);
    }
}
