using BooksConsoleApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BooksConsoleApp.Context;

public class DataContext : DbContext
{
    private readonly string _connectionString;

    public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration) : base(options)
    {
        _connectionString = configuration.GetConnectionString("SqlServer")!;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasOne(x => x.Author)
            .WithMany(x => x.Books)
            .HasForeignKey(x => x.AuthorId);

        modelBuilder.Entity<Book>()
            .HasOne(x => x.Genre)
            .WithMany(x => x.Books)
            .HasForeignKey(x => x.GenreId);

        modelBuilder.Entity<Book>()
            .HasOne(x => x.Publisher)
            .WithMany(x => x.Books)
            .HasForeignKey(x => x.PublisherId);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }

    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<Author> Authors { get; set; } = null!;
    public DbSet<Genre> Genres { get; set; } = null!;
    public DbSet<Publisher> Publishers { get; set; } = null!;
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(_connectionString);
}