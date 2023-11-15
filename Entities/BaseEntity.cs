using System.ComponentModel.DataAnnotations;

namespace BooksConsoleApp.Entities;

public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; set; }
}