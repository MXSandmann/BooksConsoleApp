using BooksConsoleApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BooksConsoleApp.Context;

public class DataContext : DbContext
{
    private readonly string _connectionString;
    
    [Obsolete("Used only for migrations!")]
    public DataContext()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(Configurations.PathToAppsettings, optional: true, reloadOnChange: true)
            .Build();
        _connectionString = configuration.GetConnectionString("SqlServer")!;
    }

    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

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