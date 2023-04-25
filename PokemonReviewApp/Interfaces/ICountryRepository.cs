using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces;

public interface ICountryRepository
{
    ICollection<Country> GetCountries();
    Country GetCountry(int countryId);
    Country GetCountryByOwner(int ownerId);
    ICollection<Owner> GetOwnersByCountry(int countryId);
    bool DoesCountryExist(int countryId);
    bool DoesCountryExist(string countryName);
    bool CreateCountry(Country country);
    bool UpdateCountry(Country country);
    bool Save();
}
