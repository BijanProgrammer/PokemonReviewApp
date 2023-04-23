using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces;

public interface IPokemonRepository
{
    ICollection<Pokemon> GetPokemons();
    Pokemon GetPokemon(int pokemonId);
    Pokemon GetPokemon(string pokemonName);
    decimal GetPokemonRating(int pokemonId);
    bool DoesPokemonExist(int pokemonId);
}
