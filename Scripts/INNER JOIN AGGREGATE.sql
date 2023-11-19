SELECT b.Title, b.Pages, a.Name, g.Name , p.Name, b.ReleaseDate 
from BooksDb.dbo.Books b
INNER JOIN BooksDb.dbo.Authors a ON b.AuthorId = a.Id
INNER JOIN BooksDb.dbo.Genres g ON b.GenreId = g.Id
INNER JOIN BooksDb.dbo.Publishers p ON b.PublisherId = p.Id;