﻿using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository;

public class CountryRepository : ICountryRepository
{
    private readonly DataContext _context;

    public CountryRepository(DataContext context)
    {
        _context = context;
    }

    public ICollection<Country> GetCountries()
    {
        return _context.Countries.OrderBy(country => country.Id).ToList();
    }

    public Country GetCountry(int countryId)
    {
        return _context.Countries.FirstOrDefault(country => country.Id == countryId);
    }

    public Country GetCountryByOwner(int ownerId)
    {
        return _context.Owners
            .Include(owner => owner.Country)
            .FirstOrDefault(owner => owner.Id == ownerId)
            .Country;
    }

    public ICollection<Owner> GetOwnersByCountry(int countryId)
    {
        return _context.Owners.Where(owner => owner.Country.Id == countryId).ToList();
    }

    public bool DoesCountryExist(int countryId)
    {
        return _context.Countries.Any(country => country.Id == countryId);
    }
}
