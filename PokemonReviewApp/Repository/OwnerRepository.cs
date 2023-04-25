using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository;

public class OwnerRepository : IOwnerRepository
{
    private readonly DataContext _context;

    public OwnerRepository(DataContext context)
    {
        _context = context;
    }

    public ICollection<Owner> GetOwners()
    {
        return _context.Owners.OrderBy(owner => owner.Id).ToList();
    }

    public Owner GetOwner(int ownerId)
    {
        return _context.Owners.FirstOrDefault(owner => owner.Id == ownerId);
    }

    public ICollection<Pokemon> GetPokemonsByOwner(int ownerId)
    {
        return _context.PokemonOwners
            .Where(pokemonOwner => pokemonOwner.OwnerId == ownerId)
            .Select(pokemonOwner => pokemonOwner.Pokemon)
            .ToList();
    }

    public bool DoesOwnerExist(int ownerId)
    {
        return _context.Owners.Any(owner => owner.Id == ownerId);
    }

    public bool DoesOwnerExist(string ownerFirstName, string ownerLastName)
    {
        return _context.Owners.Any(
            owner =>
                string.Equals(owner.FirstName, ownerFirstName)
                && string.Equals(owner.LastName, ownerLastName)
        );
    }

    public bool CreateOwner(Owner owner)
    {
        _context.Add(owner);
        return Save();
    }

    public bool Save()
    {
        var changedStatesCount = _context.SaveChanges();
        return changedStatesCount > 0;
    }
}
