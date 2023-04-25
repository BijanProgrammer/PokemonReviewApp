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

    public bool DoesPokemonExist(string pokemonName)
    {
        return _context.Pokemons.Any(
            pokemon => string.Equals(pokemon.Name.Trim(), pokemonName.Trim())
        );
    }

    public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
    {
        var owner = _context.Owners.FirstOrDefault(owner => owner.Id == ownerId);
        var pokemonOwner = new PokemonOwner { Owner = owner, Pokemon = pokemon };
        _context.Add(pokemonOwner);

        var category = _context.Categories.FirstOrDefault(category => category.Id == categoryId);
        var pokemonCategory = new PokemonCategory { Category = category, Pokemon = pokemon, };
        _context.Add(pokemonCategory);

        _context.Add(pokemon);

        return Save();
    }

    public bool Save()
    {
        var changedStatesCount = _context.SaveChanges();
        return changedStatesCount > 0;
    }
}
