using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces;

public interface IOwnerRepository
{
    ICollection<Owner> GetOwners();
    Owner GetOwner(int ownerId);
    ICollection<Pokemon> GetPokemonsByOwner(int ownerId);
    bool DoesOwnerExist(int ownerId);
    bool DoesOwnerExist(string ownerFirstName, string ownerLastName);
    bool CreateOwner(Owner owner);
    bool Save();
}
