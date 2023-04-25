using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces;

public interface ICategoryRepository
{
    ICollection<Category> GetCategories();
    Category GetCategory(int categoryId);
    ICollection<Pokemon> GetPokemonsByCategory(int categoryId);
    bool DoesCategoryExist(int categoryId);
    bool DoesCategoryExist(string categoryName);
    bool CreateCategory(Category category);
    bool UpdateCategory(Category category);
    bool Save();
}
