using BooksConsoleApp.Entities;
using BooksConsoleApp.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BooksConsoleApp.Context;

public class DataContext : DbContext
{
    private readonly string _connectionString;
    
    public DataContext() { }

    public DataContext(string connectionString) => _connectionString = connectionString;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasOne<Author>()
            .WithOne()
            .HasForeignKey<Book>(x => x.AuthorId);
        
        modelBuilder.Entity<Book>()
            .HasOne<Genre>()
            .WithOne()
            .HasForeignKey<Book>(x => x.GenreId);
        
        modelBuilder.Entity<Book>()
            .HasOne<Publisher>()
            .WithOne()
            .HasForeignKey<Book>(x => x.PublisherId);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }

    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<Author> Authors { get; set; } = null!;
    public DbSet<Genre> Genres { get; set; } = null!;
    public DbSet<Publisher> Publishers { get; set; } = null!;
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = ConfigurationHelper.GetConnectionString();
        optionsBuilder.UseSqlServer(connectionString);
    }
}