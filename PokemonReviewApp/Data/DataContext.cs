using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Owner> Owners { get; set; }
    public DbSet<Pokemon> Pokemons { get; set; }
    public DbSet<PokemonCategory> PokemonCategories { get; set; }
    public DbSet<PokemonOwner> PokemonOwners { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Reviewer> Reviewers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnPokemonCategoryCreating(modelBuilder);
        OnPokemonOwnerCreating(modelBuilder);
    }

    private static void OnPokemonCategoryCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PokemonCategory>()
            .HasKey(pokemonCategory => new {pokemonCategory.PokemonId, pokemonCategory.CategoryId});

        modelBuilder.Entity<PokemonCategory>()
            .HasOne(pokemonCategory => pokemonCategory.Pokemon)
            .WithMany(pokemon => pokemon.PokemonCategories)
            .HasForeignKey(pokemonCategory => pokemonCategory.PokemonId);

        modelBuilder.Entity<PokemonCategory>()
            .HasOne(pokemonCategory => pokemonCategory.Category)
            .WithMany(category => category.PokemonCategories)
            .HasForeignKey(pokemonCategory => pokemonCategory.CategoryId);
    }

    private static void OnPokemonOwnerCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PokemonOwner>()
            .HasKey(pokemonOwner => new {pokemonOwner.PokemonId, pokemonOwner.OwnerId});

        modelBuilder.Entity<PokemonOwner>()
            .HasOne(pokemonOwner => pokemonOwner.Pokemon)
            .WithMany(pokemon => pokemon.PokemonOwners)
            .HasForeignKey(pokemonOwner => pokemonOwner.PokemonId);

        modelBuilder.Entity<PokemonOwner>()
            .HasOne(pokemonOwner => pokemonOwner.Owner)
            .WithMany(owner => owner.PokemonOwners)
            .HasForeignKey(pokemonOwner => pokemonOwner.OwnerId);
    }
}
