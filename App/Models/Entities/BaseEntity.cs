using System.ComponentModel.DataAnnotations;

namespace BooksConsoleApp.Models.Entities;

public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; set; }
}