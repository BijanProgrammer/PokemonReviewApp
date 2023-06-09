﻿using PokemonReviewApp.Data;
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
        return _context.Categories.OrderBy(category => category.Id).ToList();
    }

    public Category GetCategory(int categoryId)
    {
        return _context.Categories.FirstOrDefault(category => category.Id == categoryId);
    }

    public ICollection<Pokemon> GetPokemonsByCategory(int categoryId)
    {
        var pokemonCategories = _context.PokemonCategories.Where(
            pokemonCategory => pokemonCategory.CategoryId == categoryId
        );
        return pokemonCategories.Select(pokemonCategory => pokemonCategory.Pokemon).ToList();
    }

    public bool DoesCategoryExist(int categoryId)
    {
        return _context.Categories.Any(category => category.Id == categoryId);
    }

    public bool DoesCategoryExist(string categoryName)
    {
        return _context.Categories.Any(
            category => string.Equals(category.Name.Trim(), categoryName.Trim())
        );
    }

    public bool CreateCategory(Category category)
    {
        _context.Add(category);
        return Save();
    }

    public bool UpdateCategory(Category category)
    {
        _context.Update(category);
        return Save();
    }

    public bool Save()
    {
        var changedStatesCount = _context.SaveChanges();
        return changedStatesCount > 0;
    }
}
